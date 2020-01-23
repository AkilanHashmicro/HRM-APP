using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMAPP.DBModel;
using HRMAPP.Model;
using HRMAPP.Pages;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using static HRMAPP.Model.HRMModel;
    
namespace HRMAPP.Views
{
    public partial class TimesheetsPage : ContentPage
    {
        public TimesheetsPage()
        {
            InitializeComponent();
          
            List<TimesheetModel> tsresult = Controller.InstanceCreation().GetTimesheetData();

            if (CrossConnectivity.Current.IsConnected == true)
            {
                expenselist.ItemsSource = tsresult;
            }
            else
            {
               // expenselist.ItemsSource = App.expense_listDb;
            }

            //   expenselist.Refreshing += this.RefreshRequested;

            //var plusRecognizer = new TapGestureRecognizer();
            //plusRecognizer.Tapped += async (s, e) =>
            //{
            //    await PopupNavigation.Instance.PushAsync(new ExpenseCreation());
            //};
            //plus.GestureRecognizers.Add(plusRecognizer);

        }

        private void RefreshRequested(object sender, object e)
        {

            expenselist.IsRefreshing = true;

            if (CrossConnectivity.Current.IsConnected)
            {
                expenselist.ItemsSource = null;

                Device.BeginInvokeOnMainThread(() =>
                {
                    Task.Run(() => Controller.InstanceCreation().GetMasterData());
                    //  List<LeavesModel> expense = Controller.InstanceCreation().GetMasterData();
                    expenselist.ItemsSource = App.expense_list;
                    expenselist.IsRefreshing = false;

                });
            }
            else
            {
                expenselist.ItemsSource = App.expense_listDb;
                expenselist.IsRefreshing = false;
            }

        }

        private void Expenselist_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    TimesheetModel data = (TimesheetModel)e.Item;
                    Navigation.PushAsync(new TimeSheetDetailPage(data));
                }
                catch (Exception exce)
                {
                    int j = 0;
                    //ExpenseModelDB data = (ExpenseModelDB)e.Item;
                    //Navigation.PushAsync(new ExpenseDetailPage(data));
                }
            }
            else
            {
                //try
                //{
                //    ExpenseModelDB data = (ExpenseModelDB)e.Item;
                //    Navigation.PushAsync(new ExpenseDetailPage(data));
                //}
                //catch
                //{
                //    ExpenseModel data = (ExpenseModel)e.Item;
                //    Navigation.PushAsync(new ExpenseDetailPage(data));
                //}
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

       

    }
}