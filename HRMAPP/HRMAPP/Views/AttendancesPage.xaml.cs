using HRMAPP.Model;
using HRMAPP.Pages;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttendancesPage : ContentPage
    {
        string checkIn_Time = "";
        string checkOut_Time = "";
        public AttendancesPage()
        {
            InitializeComponent();

            JObject attdetails = Controller.InstanceCreation().GetAttendanceData();

            //MapsAsync();

            string check_in = attdetails["check_in"].ToString();

            string check_out = attdetails["check_out"].ToString();

            // App.tagsDict = attdetails["all_tags"].ToObject<Dictionary<int, string>>();

            if (check_in != "" && check_out != "")
            {
                //inboxview.IsVisible = true;
                checkIn_layout.IsVisible = true;
                checkout_layout.IsVisible = false;
                //outboxview.IsVisible = false;
            }
            else if (check_in != "" && check_out == "")
            {
                checkIn_layout.IsVisible = false;
                checkout_layout.IsVisible = true;
                //inboxview.IsVisible = false;               
                //outboxview.IsVisible = true;
            }

            else if (check_in == "" && check_out == "")
            {
                checkIn_layout.IsVisible = true;
                checkout_layout.IsVisible = false;
            }

            var checkIN_recognizer = new TapGestureRecognizer();
            checkIN_recognizer.Tapped += async (s, e) =>
            {

                //try
                //{
                //    var locator1 = CrossGeolocator.Current;
                //    var position1 = await locator1.GetPositionAsync(timeout: new TimeSpan(20000));
                //}

                try
                {

                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;
                    var position = await locator.GetPositionAsync(timeout: new TimeSpan(20000));

                    string latitude = position.Latitude.ToString();
                    string longtitude = position.Longitude.ToString();

                    var address = await locator.GetAddressesForPositionAsync(position, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");

                    var data = address.FirstOrDefault();

                    string ads = "";

                    if (data.SubLocality != "")
                    {
                        ads += data.SubLocality + ", ";
                    }
                    if (data.Locality != "")
                    {
                        ads += data.Locality + ", ";
                    }
                    if (data.AdminArea != "")
                    {
                        ads += data.AdminArea + ", ";
                    }
                    if (data.CountryName != "")
                    {
                        ads += data.CountryName + ", ";
                    }
                    if (ads != "")
                    {
                        ads = ads.Substring(0, ads.Length - 2);
                    }

                    string full_address = ads;

                    Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
                    DateTime dt = DateTime.Now;
                    checkIn_Time = dt.ToString("HH:mm:ss");
                    string dat = dt.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

                    vals["employee_id"] = App.employee_id;
                    vals["check_in"] = dat;
                    vals["type"] = "check_in";
                    vals["sign_in_lat"] = latitude;
                    vals["sign_in_lng"] = longtitude;
                    vals["check_in_location"] = full_address;
                    int m = 0;

                //}

                //catch
                //{
                //    DependencyService.Get<Toast>().Show("Check your GPS connection...");
                //}

                //try
                //{
                    act_ind.IsRunning = true;

                    bool created = Controller.InstanceCreation().CreateCheckIn("hr.attendance", "app_checkin_checkout", vals);

                    //  var created = Controller.InstanceCreation().CreateCheckIn("hr.attendance", "create", vals);
                    if (created)
                    {
                        //inboxview.IsVisible = false;
                        checkIn_layout.IsVisible = false;

                        message_welcome.IsVisible = true;
                        check_in_time.Text = "Checked in at" + " " + checkIn_Time;
                        check_in_welcome.Text = "Welcome Administrator";

                        string timeslit = dt.ToString("HH");

                        if (Int32.Parse(timeslit) < 12)
                        {
                            check_in_message.Text = "Good Morning";
                        }

                        if (Int32.Parse(timeslit) < 16)
                        {
                            check_in_message.Text = "Good Afternoon";
                        }

                        else if (Int32.Parse(timeslit) < 20)
                        {
                            check_in_message.Text = "Good Evening";
                        }

                        else
                        {
                            check_in_message.Text = "Good Night";
                        }

                    }
                    else
                    {
                        string txt = "Already checked in. Please check";
                        await PopupNavigation.Instance.PushAsync(new CommonAlert(txt));
                        //await DisplayAlert("Alert", "Please try again", "Ok");
                    }

                    act_ind.IsRunning = false;
                }
                catch (Exception ex)
                {
                    int i = 0;
                    DependencyService.Get<Toast>().Show("Need GPS connection for Sign in...");
                    act_ind.IsRunning = false;
                }

            };
            checkIn_click.GestureRecognizers.Add(checkIN_recognizer);

            var checkOut_Recognizer = new TapGestureRecognizer();
            checkOut_Recognizer.Tapped += async (s, e) =>
            {
                try
                {

                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;
                    var position = await locator.GetPositionAsync(timeout: new TimeSpan(20000));

                    string latitude = position.Latitude.ToString();
                    string longtitude = position.Longitude.ToString();

                    var address = await locator.GetAddressesForPositionAsync(position, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");

                    var data = address.FirstOrDefault();

                    string ads = "";

                    if (data.SubLocality != "")
                    {
                        ads += data.SubLocality + ", ";
                    }
                    if (data.Locality != "")
                    {
                        ads += data.Locality + ", ";
                    }
                    if (data.AdminArea != "")
                    {
                        ads += data.AdminArea + ", ";
                    }
                    if (data.CountryName != "")
                    {
                        ads += data.CountryName + ", ";
                    }
                    if (ads != "")
                    {
                        ads = ads.Substring(0, ads.Length - 2);
                    }

                    string full_address = ads;

                    Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
                    DateTime dt = DateTime.Now;
                    string dat = dt.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                    checkOut_Time = dt.ToString("HH:mm:ss");

                    vals["check_out"] = dat;
                    vals["employee_id"] = App.employee_id;
                    vals["type"] = "check_out";
                    vals["sign_out_lat"] = latitude;
                    vals["sign_out_lng"] = longtitude;
                    vals["check_out_location"] = full_address;

                    try
                    {
                        act_ind.IsRunning = true;

                        bool updated = Controller.InstanceCreation().CreateCheckIn("hr.attendance", "app_checkin_checkout", vals);
                        if (updated)
                        {
                            checkout_layout.IsVisible = false;
                            //outboxview.IsVisible = false;

                            message_goodbye.IsVisible = true;

                            check_out_time.Text = "Checked out at" + " " + checkOut_Time;
                            check_out_welcome.Text = "Goodbye Administrator";
                            check_out_message.Text = "Have a good day!";

                        }
                        else
                        {
                            //await DisplayAlert("Alert", "Please try again", "Ok");
                            string txt = "Already checked out. Please check";
                            await PopupNavigation.Instance.PushAsync(new CommonAlert(txt));
                            act_ind.IsRunning = false;

                        }

                        act_ind.IsRunning = false;

                    }
                    catch (Exception ex)
                    {
                        int i = 0;
                        act_ind.IsRunning = false;
                    }

                }

                catch (Exception ex)
                {
                    int i = 0;
                    DependencyService.Get<Toast>().Show("Need GPS connection for Sign out...");
                    act_ind.IsRunning = false;
                }


            };
            checkout_click.GestureRecognizers.Add(checkOut_Recognizer);

        }


        private void Ok_checkIn_Btn_Clicked(object sender, EventArgs e)
        {
            message_welcome.IsVisible = false;

            checkout_layout.IsVisible = true;
            //outboxview.IsVisible = true;

            App.check_in_out = true;
            Settings.CheckIn_Out = "true";
        }

        private void Ok_checkOut_Btn_Clicked(object sender, EventArgs e)
        {
            message_goodbye.IsVisible = false;

            //inboxview.IsVisible = true;
            checkIn_layout.IsVisible = true;

            App.check_in_out = false;
            Settings.CheckIn_Out = "false";
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public void myattandancebtn__clicked(object sender, EventArgs args)
        {
            act_ind.IsRunning = true;
            Navigation.PushAsync(new MyAttendancesPage());
            act_ind.IsRunning = false;
        }

        //private async void MapsAsync()
        //{
        // var locator = CrossGeolocator.Current;
        //locator.DesiredAccuracy = 20;
        // var position = await locator.GetPositionAsync(timeout: new TimeSpan(20000));

        //      MyMap.MoveToRegion(
        //MapSpan.FromCenterAndRadius(
        //        new Position(position.Latitude, position.Longitude), Distance.FromMiles(0.05)));

        //var position1 = new Position(position.Latitude, position.Longitude);

        //var pin1 = new Pin
        //{
        //    Type = PinType.Place,
        //    Position = position1,
        //    Label = "Your Address",
        //    Address = "",
        //};

        //MyMap.Pins.Add(pin1);
        // }

    }
}