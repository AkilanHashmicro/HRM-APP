using HRMAPP.DBModel;
using HRMAPP.Model;
using HRMAPP.Pages;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfCalendar.XForms;
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
	public partial class LeavesPage : ContentPage
	{
        List<LeavesModel> holiday = new List<LeavesModel>();
        List<LeavesModelDB> holidayDb = new List<LeavesModelDB>();
        ViewCell lastCell;
        string filter_state = "";
        Boolean check_filter = false;

        
        async Task RefreshData()
        {
            if(CrossConnectivity.Current.IsConnected == true)
            {
                holiday = await Task.Run(() => Controller.InstanceCreation().GetMasterData());             
                       //  leavelist.EndRefresh();
                leavelist.ItemsSource = null;
                leavelist.ItemsSource = App.leaves_list;
            }
            else
            {
              //  holidayDb = App.leaves_listDb;   
                leavelist.ItemsSource = null;
                leavelist.ItemsSource = App.leaves_listDb;
               // leavelist.EndRefresh();
            }

        }

        public LeavesPage ()
		{
			InitializeComponent ();

            calendar.HeightRequest = 300;

            if(CrossConnectivity.Current.IsConnected == true)
            {
                //  holiday = Controller.InstanceCreation().GetMasterData();
                Task.Run(() => Controller.InstanceCreation().GetMasterData());
                try
                {
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = App.leaves_list;

                 
                }
                catch
                {
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = App.leaves_listDb;

                }

                //Controller.InstanceCreation().Getleaves_data();
            }
            else
            {
                try
                {
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = App.leaves_listDb;
                }
                catch
                {
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = App.leaves_list;
                                    
                }
            }

            leavelist.IsEnabled = true;

            var plusRecognizer = new TapGestureRecognizer();
            plusRecognizer.Tapped += async (s, e) =>
            {
                await PopupNavigation.Instance.PushAsync(new LeavesCreation());

            };
            plus.GestureRecognizers.Add(plusRecognizer);

            //leavelist.RefreshCommand = new Command(() => {
            //    //Do your stuff.
            //    Leavelist_Refreshing();
            //    leavelist.IsRefreshing = false;
            //});


            var draftRecognizer = new TapGestureRecognizer();
            draftRecognizer.Tapped += (s, e) =>
            {
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if (App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data1 = new List<LeavesModel>();
                        data1 = App.leaves_list.Where(p => p.state_name == "New").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data1;
                    });
                }
                else
                {
                    List<LeavesModelDB> data1 = new List<LeavesModelDB>();
                    data1 = App.leaves_listDb.Where(p => p.state_name == "New").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data1;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "New";

            };
            draft.GestureRecognizers.Add(draftRecognizer);


            var draftlabelRecognizer = new TapGestureRecognizer();
            draftlabelRecognizer.Tapped += (s, e) =>
            {
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if (App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data1 = new List<LeavesModel>();
                        data1 = App.leaves_list.Where(p => p.state_name == "New").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data1;
                    });
                }
                else
                {
                    List<LeavesModelDB> data1 = new List<LeavesModelDB>();
                    data1 = App.leaves_listDb.Where(p => p.state_name == "New").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data1;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "New";

            };
            draftlabel.GestureRecognizers.Add(draftlabelRecognizer);

            var confirmRecognizer = new TapGestureRecognizer();
            confirmRecognizer.Tapped += (s, e) =>
            {
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if(App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data2 = new List<LeavesModel>();
                        data2 = App.leaves_list.Where(p => p.state_name == "Waiting Pre-Approval").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data2;
                    });
                }
                else
                {
                    List<LeavesModelDB> data2 = new List<LeavesModelDB>();
                    data2 = App.leaves_listDb.Where(p => p.state_name == "Waiting Pre-Approval").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data2;
                }              
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "Waiting Pre-Approval";
                check_filter = false;

            };
            confirm.GestureRecognizers.Add(confirmRecognizer);

            var confirmlabelRecognizer = new TapGestureRecognizer();
            confirmlabelRecognizer.Tapped += (s, e) =>
            {
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if (App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data2 = new List<LeavesModel>();
                        data2 = App.leaves_list.Where(p => p.state_name == "Waiting Pre-Approval").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data2;
                    });
                }
                else
                {
                    List<LeavesModelDB> data2 = new List<LeavesModelDB>();
                    data2 = App.leaves_listDb.Where(p => p.state_name == "Waiting Pre-Approval").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data2;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "Waiting Pre-Approval";
                check_filter = false;

            };
            confirmlabel.GestureRecognizers.Add(confirmlabelRecognizer);

            var refuseRecognizer = new TapGestureRecognizer();
            refuseRecognizer.Tapped += (s, e) =>
            {
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if(App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data3 = new List<LeavesModel>();
                        data3 = App.leaves_list.Where(p => p.state_name == "Refused").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data3;
                    });
                }
                else
                {
                    List<LeavesModelDB> data3 = new List<LeavesModelDB>();
                    data3 = App.leaves_listDb.Where(p => p.state_name == "Refused").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data3;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "Refused";
                check_filter = false;

            };
            refuse.GestureRecognizers.Add(refuseRecognizer);

            var refuselabelRecognizer = new TapGestureRecognizer();
            refuselabelRecognizer.Tapped += (s, e) =>
            {
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if (App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data3 = new List<LeavesModel>();
                        data3 = App.leaves_list.Where(p => p.state_name == "Refused").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data3;
                    });
                }
                else
                {
                    List<LeavesModelDB> data3 = new List<LeavesModelDB>();
                    data3 = App.leaves_listDb.Where(p => p.state_name == "Refused").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data3;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "Refused";
                check_filter = false;

            };
            refuselabel.GestureRecognizers.Add(refuselabelRecognizer);

            var validateRecognizer = new TapGestureRecognizer();
            validateRecognizer.Tapped += (s, e) =>
            {

                plus.IsVisible = true;              
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if(App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data4 = new List<LeavesModel>();
                        data4 = App.leaves_list.Where(p => p.state_name == "Approved").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data4;
                    });
                }
                else
                {
                    List<LeavesModelDB> data4 = new List<LeavesModelDB>();
                    data4 = App.leaves_listDb.Where(p => p.state_name == "Approved").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data4;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "Approved";
                check_filter = false;

            };
            validate.GestureRecognizers.Add(validateRecognizer);

            var validatelabelRecognizer = new TapGestureRecognizer();
            validatelabelRecognizer.Tapped += (s, e) =>
            {

                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if (App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data4 = new List<LeavesModel>();
                        data4 = App.leaves_list.Where(p => p.state_name == "Approved").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data4;
                    });
                }
                else
                {
                    List<LeavesModelDB> data4 = new List<LeavesModelDB>();
                    data4 = App.leaves_listDb.Where(p => p.state_name == "Approved").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data4;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "Approved";
                check_filter = false;

            };
            validatelabel.GestureRecognizers.Add(validatelabelRecognizer);

            var CancelRecognizer = new TapGestureRecognizer();
            CancelRecognizer.Tapped += (s, e) =>
            {

                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if(App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data5 = new List<LeavesModel>();
                        data5 = App.leaves_list.Where(p => p.state_name == "Cancelled").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data5;
                    });
                }
                else
                {
                    List<LeavesModelDB> data5 = new List<LeavesModelDB>();
                    data5 = App.leaves_listDb.Where(p => p.state_name == "Cancelled").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data5;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "Cancelled";
                check_filter = false;
            };
            cancel.GestureRecognizers.Add(CancelRecognizer);

            var CancellabelRecognizer = new TapGestureRecognizer();
            CancellabelRecognizer.Tapped += (s, e) =>
            {

                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if (App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModel> data5 = new List<LeavesModel>();
                        data5 = App.leaves_list.Where(p => p.state_name == "Cancelled").ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data5;
                    });
                }
                else
                {
                    List<LeavesModelDB> data5 = new List<LeavesModelDB>();
                    data5 = App.leaves_listDb.Where(p => p.state_name == "Cancelled").ToList();
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = data5;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "Cancelled";
                check_filter = false;
            };
            cancellabel.GestureRecognizers.Add(CancellabelRecognizer);

            var allRecognizer = new TapGestureRecognizer();
            allRecognizer.Tapped += (s, e) =>
            {
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if(App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //leavelist.ItemsSource = null;
                        //leavelist.ItemsSource = holiday;                      
                        leavelist.ItemsSource = App.leaves_list;
                        leavelist.IsRefreshing = false;
                    });
                   
                }
                else
                {
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = App.leaves_listDb;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "";
                check_filter = false;
            };
            all.GestureRecognizers.Add(allRecognizer);


            var all_labelRecognizer = new TapGestureRecognizer();
            all_labelRecognizer.Tapped += (s, e) =>
            {
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                if (App.NetAvailable == true)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        leavelist.ItemsSource = App.leaves_list;
                        leavelist.IsRefreshing = false;
                    });
                }
                else
                {
                    leavelist.ItemsSource = null;
                    leavelist.ItemsSource = App.leaves_listDb;
                }
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
                filter_state = "";
                check_filter = false;
            };
            alllabel.GestureRecognizers.Add(all_labelRecognizer);

        }



        private void calendar_MonthChangedAsync(object sender, MonthChangedEventArgs args)
        {


            CalendarEventCollection appointments = new CalendarEventCollection();

            
            if (CrossConnectivity.Current.IsConnected)
            {
                string seldate = calendar.DisplayDate.ToString("MM-yy");
                List<LeavesModel> monthdata = new List<LeavesModel>();
                monthdata = App.leaves_list.Where(s => s.date == seldate).ToList();
                leavelist.ItemsSource = null;
               
                leavelist.ItemsSource = monthdata;

                //foreach (var data in monthdata)
                //{

                //    appointments.Add(new CalendarInlineEvent()
                //    {
                //        StartTime = Convert.ToDateTime(data.date_from.ToString()).ToLocalTime(),
                //        EndTime = Convert.ToDateTime(data.dateto.ToString()).ToLocalTime(),
                //        // Subject = cal.meeting_subject.ToString(),
                //        Color = Color.FromHex(data.stage_colour),
                //    });
                //}
                //calendar.DataSource = appointments;
            }
            else
            {
                string seldate = calendar.DisplayDate.ToString("MM-yy");
                List<LeavesModelDB> monthdata = new List<LeavesModelDB>();
                monthdata = App.leaves_listDb.Where(s => s.date == seldate).ToList();
                leavelist.ItemsSource = null;
                leavelist.ItemsSource = monthdata;

              //  CalendarEventCollection appointments = new CalendarEventCollection();

                //foreach (var data in App.leaves_listDb)
                //{

                //    appointments.Add(new CalendarInlineEvent()
                //    {
                //        StartTime = Convert.ToDateTime(data.date_from.ToString()).ToLocalTime(),
                //        EndTime = Convert.ToDateTime(data.dateto.ToString()).ToLocalTime(),
                //        // Subject = cal.meeting_subject.ToString(),
                //        Color = Color.FromHex(data.stage_colour),
                //    });
                //}
                //calendar.DataSource = appointments;

            }

           // leavelist.ItemsSource = App.leaves_listDb;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<string, string>("MyApp", "CreateMsg", (sender, arg) =>
            {
                string data = arg;
                plus.IsVisible = true;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
            });

            MessagingCenter.Subscribe<string, string>("MyApp", "LeavesListUpdated", (sender, arg) =>
            {
                //leavelist.ItemsSource = null;
                //leavelist.ItemsSource = App.leaves_list;

                //DependencyService.Get<Toast>().Show(arg);

              //  Leavelist_Refreshing();
                  RefreshData();

            });

            MessagingCenter.Subscribe<string, string>("MyApp", "ErrorListUpdated", (sender, arg) =>
            {
                //leavelist.ItemsSource = null;
                //leavelist.ItemsSource = App.leaves_list;

                DependencyService.Get<Toast>().Show(arg);
                RefreshData();

                 // Leavelist_Refreshing();
            });

        }


        private void Leavelist_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (CrossConnectivity.Current.IsConnected) 
            {  
                    try
                    {
                        LeavesModel data = (LeavesModel)e.Item;
                        Navigation.PushAsync(new LeavesDetailPage(data));
                    }

                    catch
                    {
                        LeavesModelDB data = (LeavesModelDB)e.Item;
                        Navigation.PushAsync(new LeavesDetailPage(data));
                    }
            } 
            else 
            {  
                try
                {
                   LeavesModelDB data = (LeavesModelDB)e.Item;
                    Navigation.PushAsync(new LeavesDetailPage(data));
                }
                catch
               {
                   LeavesModel data = (LeavesModel)e.Item;
                    Navigation.PushAsync(new LeavesDetailPage(data));
               }
          }
        
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("373D4B");
                lastCell = viewCell;
            }
        }

        protected override bool OnBackButtonPressed()
        {       
            return true;
        }

        private async void Leavelist_Refreshing(object sender, object e)
        {
            leavelist.IsRefreshing = true;

           // act_ind.IsRunning = true;

            await Task.Run(() => holiday = Controller.InstanceCreation().GetMasterData());     

            if (CrossConnectivity.Current.IsConnected == true)
             {
                //Device.BeginInvokeOnMainThread(async () =>
                //{
                    // await Task.Run(() => App.Current.MainPage = new MasterPage(new LeavesPage()));
                    leavelist.ItemsSource = null;

                 //   await Task.Run(() => holiday = Controller.InstanceCreation().GetMasterData());     

                    //  Task.Run(() => holiday = Controller.InstanceCreation().GetMasterData());
                    //  leavelist.EndRefresh();

                    if (filter_state.Equals("New"))
                    {
                        List<LeavesModel> data1 = new List<LeavesModel>();
                        data1 = holiday.Where(s => s.state_name == filter_state).ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data1;
                    }
                    else if (filter_state.Equals("Waiting Pre-Approval"))
                    {
                        List<LeavesModel> data2 = new List<LeavesModel>();
                        data2 = holiday.Where(s => s.state_name == filter_state).ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data2;
                    }
                    else if (filter_state.Equals("Refused"))
                    {
                        List<LeavesModel> data3 = new List<LeavesModel>();
                        data3 = holiday.Where(s => s.state_name == filter_state).ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data3;
                    }
                    else if (filter_state.Equals("Approved"))
                    {
                        List<LeavesModel> data4 = new List<LeavesModel>();
                        data4 = holiday.Where(s => s.state_name == filter_state).ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data4;
                    }
                    else if (filter_state.Equals("Cancelled"))
                    {
                        List<LeavesModel> data5 = new List<LeavesModel>();
                        data5 = holiday.Where(s => s.state_name == filter_state).ToList();
                        leavelist.ItemsSource = null;
                        leavelist.ItemsSource = data5;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                            {

                                leavelist.ItemsSource = null;
                                leavelist.ItemsSource = holiday;
                        leavelist.IsRefreshing = false;
                            });
                    }
                    leavelist.IsRefreshing = false;

               // act_ind.IsRunning = false;
                //});

        }
             else
             {
                    if (filter_state.Equals("New"))
              {
                  List<LeavesModelDB> data1 = new List<LeavesModelDB>();
                 // data1 = holidayDb.Where(s => s.state_name == filter_state).ToList();
                    data1 = App.leaves_listDb.Where(s => s.state_name == filter_state).ToList();
                  leavelist.ItemsSource = null;
                  leavelist.ItemsSource = data1;
              }
              else if (filter_state.Equals("Waiting Pre-Approval"))
              {
                  List<LeavesModelDB> data2 = new List<LeavesModelDB>();
                    data2 = App.leaves_listDb.Where(s => s.state_name == filter_state).ToList();
                  leavelist.ItemsSource = null;
                  leavelist.ItemsSource = data2;
              }
              else if (filter_state.Equals("Refused"))
              {
                  List<LeavesModelDB> data3 = new List<LeavesModelDB>();
                    data3 = App.leaves_listDb.Where(s => s.state_name == filter_state).ToList();
                  leavelist.ItemsSource = null;
                  leavelist.ItemsSource = data3;
              }
              else if (filter_state.Equals("Approved"))
              {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        List<LeavesModelDB> data4 = new List<LeavesModelDB>();
                        data4 = App.leaves_listDb.Where(s => s.state_name == filter_state).ToList();
                        //  leavelist.ItemsSource = null;
                   
                  leavelist.ItemsSource = data4;

                    });
              }
              else if (filter_state.Equals("Cancelled"))
              {
                  List<LeavesModelDB> data5 = new List<LeavesModelDB>();
                    data5 = App.leaves_listDb.Where(s => s.state_name == filter_state).ToList();
                 // leavelist.ItemsSource = null;
                  leavelist.ItemsSource = data5;
              }
              else
              {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        leavelist.ItemsSource = App.leaves_listDb;
                        leavelist.IsRefreshing = false;
                    });

                    //Device.BeginInvokeOnMainThread(async () =>
                    //{
                    //    leavelist.ItemsSource = null;
                    //  //  leavelist.ItemsSource = holidayDb;
                    //    leavelist.ItemsSource = App.leaves_listDb;
                    //    leavelist.IsRefreshing = false;

                    //});
              }


              //   holidayDb = App.leaves_listDb;
                  // leavelist.EndRefresh();
              }          

          //  await RefreshData();

            leavelist.IsRefreshing = false;
           
        }

        private void FilterbarItem_Activated(object sender, EventArgs e)
        {

            if(check_filter == false)
            {
                check_filter = true;
            }
            else
            {
                check_filter = false;
            }

            if(check_filter == true)
            {
                draft.IsVisible = true;
                draftlabel.IsVisible = true;
                confirm.IsVisible = true;
                confirmlabel.IsVisible = true;
                cancel.IsVisible = true;
                cancellabel.IsVisible = true;
                validate.IsVisible = true;
                validatelabel.IsVisible = true;
                refuse.IsVisible = true;
                refuselabel.IsVisible = true;
                all.IsVisible = true;
                alllabel.IsVisible = true;
                mainlayout.BackgroundColor = Color.FromHex("#F2F2F2");
                leavelist.IsEnabled = false;
                leavelist.Opacity = 0.25;
                calendar_layout.Opacity = 0.25;
                leavelist.IsEnabled = false;
                plus.Opacity = 0.25;
            }
            else
            {
                draft.IsVisible = false;
                draftlabel.IsVisible = false;
                confirm.IsVisible = false;
                confirmlabel.IsVisible = false;
                cancel.IsVisible = false;
                cancellabel.IsVisible = false;
                validate.IsVisible = false;
                validatelabel.IsVisible = false;
                refuse.IsVisible = false;
                refuselabel.IsVisible = false;
                all.IsVisible = false;
                alllabel.IsVisible = false;
                mainlayout.BackgroundColor = Color.Transparent;
                leavelist.IsEnabled = true;
                leavelist.Opacity = 1;
                calendar_layout.Opacity = 1;
                plus.Opacity = 1;
                leavelist.IsEnabled = true;
            }


        }
    }
}