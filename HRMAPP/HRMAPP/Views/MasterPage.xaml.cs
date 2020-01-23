using HRMAPP.Model;
using HRMAPP.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRMAPP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        List<MasterPageItems> mlist = new List<MasterPageItems>();
        public MasterPage(Page Pageref)
        {
            InitializeComponent();

            mlist.Add(new MasterPageItems("DASHBOARD ", "dashblack.png", typeof(DashboardPage)));
            mlist.Add(new MasterPageItems("LEAVES ", "leavesblack.png", typeof(LeavesPage)));
            mlist.Add(new MasterPageItems("EXPENSE", "dollarsym.png", typeof(ExpensePage)));
            mlist.Add(new MasterPageItems("ATTENDANCE", "attendblack.png", typeof(AttendancesPage)));
            mlist.Add(new MasterPageItems("TIMESHEETS", "timesheet.png", typeof(TimesheetsPage)));
            mlist.Add(new MasterPageItems("TIMESHEET DETAILS", "timesheet.png", typeof(TimeSheetActivitiesPage)));
            mlist.Add(new MasterPageItems("LOG OUT", "signout.png", typeof(DashboardPage)));

            listItems.ItemsSource = mlist;
            Page Ref = Pageref;
            Detail = new NavigationPage(Ref);

            string tmpImage = App.user_image;
            masterPageName.Text = App.user_name;
            masterPageRole.Text = App.user_email;

            if(App.NetAvailable == true)
            {
                if (tmpImage.Equals("False"))
                {
                    UserImage.Source = "unknown.png";
                }
                else
                {
                    byte[] imageAsBytes = Encoding.UTF8.GetBytes(tmpImage);
                    byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
                    var stream = new MemoryStream(decodedByteArray);
                    UserImage.Source = ImageSource.FromStream(() => stream);
                }
            }
            else if(App.NetAvailable == false)
            {
                foreach (var obj in App.UserListDb)
                {
                    this.masterPageName.Text = obj.user_name;
                    this.masterPageRole.Text = obj.user_email;

                    if (tmpImage.Equals("False"))
                    {
                        UserImage.Source = "unknownPerson.png";
                    }
                    else
                    {
                        byte[] imageAsBytes = Encoding.UTF8.GetBytes(obj.user_image_medium);
                        byte[] decodedByteArray = System.Convert.FromBase64String(Encoding.UTF8.GetString(imageAsBytes, 0, imageAsBytes.Length));
                        var stream = new MemoryStream(decodedByteArray);
                        UserImage.Source = ImageSource.FromStream(() => stream);
                    }
                }
            }            

        }

        private async void menu_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            act_ind.IsRunning = true;
            act_layout.IsVisible = true;

            try
            {
                MasterPageItems obj = (MasterPageItems)e.Item;
                string name = obj.Title;

                if (name.Equals("LOG OUT"))
                {
                    string title = "Logout";
                    string txt = "Are you sure you want to log out?";
                    await PopupNavigation.Instance.PushAsync(new CustomAlert(txt,title));

                    //var data = await DisplayAlert("Alert", "Are you sure you want to log out?", "Ok", "Cancel");                    
                    //if (data)
                    //{
                    //    Settings.UserName = "";
                    //    Settings.UserPassword = "";
                    //    Settings.PrefKeyUserDetails = "";
                    //    Settings.UserUrlName = "";
                    //    Settings.UserDbName = "";
                    //    Settings.CheckIn_ID = "";
                    //    Settings.CheckIn_Out = "";
                    //    App.userid = 0;
                    //    App.Current.MainPage = new NavigationPage(new LoginPage());
                    //}
                }
                else
                {
                    Type Page = obj.TypeTarget;

                    //var currentpage = new LoadingIndicator();
                    //await PopupNavigation.Instance.PushAsync(currentpage);

                    await Task.Run(() => Detail = new NavigationPage((Page)Activator.CreateInstance(Page)));
                    //Detail = await Task.Run(() => new NavigationPage((Page)Activator.CreateInstance(Page)));
                    IsPresented = false;



                 //   Loadingalertcall();

                }

            }
            catch (Exception ex)
            {
                if (App.NetAvailable == false)
                {
                    await DisplayAlert("Alert", "Need Internet Connection", "Ok");
                    Loadingalertcall();
                }
            }

            act_ind.IsRunning = false;
            act_layout.IsVisible = false;

        }

        async void Loadingalertcall()
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string, bool>("MyApp", "logoutMsg", (sender, arg) =>
            {
                Settings.UserName = "";
                Settings.UserPassword = "";
                Settings.PrefKeyUserDetails = "";
                Settings.UserUrlName = "";
                Settings.UserDbName = "";
                Settings.CheckIn_ID = "";
                Settings.CheckIn_Out = "";
                App.userid = 0;
                App.Current.MainPage = new NavigationPage(new LoginPage());
            });
        }


        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#f6f6f6");
            //obj.View.BackgroundColor = Color.Transparent;   //Color.FromHex("#CB454C");
        }


    }
}