using HRMAPP.DBModel;
using HRMAPP.Model;
using HRMAPP.Pages;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExpensePage : ContentPage
	{
        List<ExpenseModelDB> expenseDb = new List<ExpenseModelDB>();

        async Task RefreshData()
        {
            //if(App.NetAvailable == true)
            //{
            //    List<LeavesModel> expense = Controller.InstanceCreation().GetMasterData();
            //}
            //else
            //{
            //   expenseDb =  App.expense_listDb;
            //}

            //if (CrossConnectivity.Current.IsConnected == true)
            //{
            //    await Task.Run(() => Controller.InstanceCreation().GetMasterData());
            //    //  leavelist.EndRefresh();
            //    expenselist.ItemsSource = null;
            //    expenselist.ItemsSource = App.expense_list;
            //}
            //else
            //{
            //    //  holidayDb = App.leaves_listDb;   
            //    expenselist.ItemsSource = null;
            //    expenselist.ItemsSource = App.expense_listDb;
            //    // leavelist.EndRefresh();
            //}

        }

        public ExpensePage ()
		{
			InitializeComponent ();

           // List<LeavesModel> expense = Controller.InstanceCreation().GetMasterData();

            List<ExpenseModel> expense = Controller.InstanceCreation().GetExpenseData();


            if(CrossConnectivity.Current.IsConnected == true)
            {
                expenselist.ItemsSource = expense;
            }
            else
            {
                expenselist.ItemsSource = App.expense_listDb;
            }

         //   expenselist.Refreshing += this.RefreshRequested;

            var plusRecognizer = new TapGestureRecognizer();
            plusRecognizer.Tapped += async (s, e) =>
            {
                await PopupNavigation.Instance.PushAsync(new ExpenseCreation());
            };
            plus.GestureRecognizers.Add(plusRecognizer);

        }

        private async void RefreshRequested(object sender, object e)
        {
          //  act_ind.IsRunning = true;
          // expenselist.IsRefreshing = true;
            //await RefreshData();

            expenselist.IsRefreshing = true;

            if(CrossConnectivity.Current.IsConnected)
            {
                expenselist.ItemsSource = null;

                Device.BeginInvokeOnMainThread( () =>
                         {                 
                     //  Task.Run(() => Controller.InstanceCreation().GetMasterData());

                    Task.Run(() => App.expense_list = Controller.InstanceCreation().GetExpenseData());
                        //  List<LeavesModel> expense = Controller.InstanceCreation().GetMasterData();
                          expenselist.ItemsSource = App.expense_list;
                        expenselist.IsRefreshing = false;

                         });               
            }
            else
            {
                expenselist.ItemsSource = App.expense_listDb;

                //Device.BeginInvokeOnMainThread(() =>
                    //    {
                   

                    //        expenselist.ItemsSource = null;
                    //      //  expenselist.ItemsSource = expenseDb;
                    //expenselist.ItemsSource =  App.expense_listDb;
                    //expenselist.IsRefreshing = false;
                            
                expenselist.IsRefreshing = false;
                        //});
            }
           // expenselist.IsRefreshing = false;
           // act_ind.IsRunning = false;
        }

        private void Expenselist_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected) 
            { 
                try
                {
                    
                    ExpenseModel data = (ExpenseModel)e.Item;
                    Navigation.PushAsync(new ExpenseDetailPage(data));
                }
                catch(Exception exce)
                {
                    ExpenseModelDB data = (ExpenseModelDB)e.Item;
                    Navigation.PushAsync(new ExpenseDetailPage(data));
                }
            }
            else
            {
                try
                {
                    ExpenseModelDB data = (ExpenseModelDB)e.Item;
                    Navigation.PushAsync(new ExpenseDetailPage(data));
                }
                catch
                {
                    ExpenseModel data = (ExpenseModel)e.Item;
                    Navigation.PushAsync(new ExpenseDetailPage(data));
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
                      
            MessagingCenter.Subscribe<string, string>("MyApp", "ExpenseListUpdated", (sender, arg) =>
            {              
                // Controller.InstanceCreation().GetMasterData();
                //expenselist.ItemsSource = null;
                //expenselist.ItemsSource = App.expense_list;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Run(() => Controller.InstanceCreation().GetMasterData());
                    expenselist.ItemsSource = App.expense_list;
                    expenselist.IsRefreshing = false;
                });

            //  RefreshData();

            });

        }

    }
}