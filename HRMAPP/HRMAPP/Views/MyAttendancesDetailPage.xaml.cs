using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Views
{
    public partial class MyAttendancesDetailPage : ContentPage
    {
        public MyAttendancesDetailPage(AttendanceModel modelobj)
        {
            InitializeComponent();

            emp_txt.Text = modelobj.employee_name;

            checkin_txt.Text = modelobj.check_in;
            checkin_lat_txt.Text = modelobj.sign_in_lat;
            checkin_long_txt.Text = modelobj.sign_in_lng;
            checkin_loc_txt.Text = modelobj.check_in_location;
            checkout_txt.Text = modelobj.check_out;
            checkout_lat_txt.Text = modelobj.sign_out_lat;
            checkout_long_txt.Text = modelobj.sign_out_lng;
            checkout_loc_txt.Text = modelobj.check_out_location;

            if (modelobj.sign_in_lat != "False")
            {
                Sign_in_map.MoveToRegion(
          MapSpan.FromCenterAndRadius(
                        new Position(Convert.ToDouble(modelobj.sign_in_lat), Convert.ToDouble(modelobj.sign_in_lng)), Distance.FromMiles(0.05)));

                var position1 = new Position(Convert.ToDouble(modelobj.sign_in_lat), Convert.ToDouble(modelobj.sign_in_lng));

                var pin1 = new Pin
                {
                    Type = PinType.Place,
                    Position = position1,
                    Label = "Your Address",
                    Address = "",
                };

                Sign_in_map.Pins.Add(pin1);
            }

            if (modelobj.sign_out_lat != "False")
            {
                sign_out_map.MoveToRegion(
          MapSpan.FromCenterAndRadius(
                        new Position(Convert.ToDouble(modelobj.sign_out_lat), Convert.ToDouble(modelobj.sign_out_lng)), Distance.FromMiles(0.05)));

                var position1 = new Position(Convert.ToDouble(modelobj.sign_in_lat), Convert.ToDouble(modelobj.sign_in_lng));

                var pin1 = new Pin
                {
                    Type = PinType.Place,
                    Position = position1,
                    Label = "Your Address",
                    Address = "",
                };

                sign_out_map.Pins.Add(pin1);
            }

        }

     }

}
