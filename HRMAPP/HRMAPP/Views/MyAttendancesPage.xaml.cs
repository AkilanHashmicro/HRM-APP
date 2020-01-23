using System;
using System.Collections.Generic;
using HRMAPP.Model;
using Plugin.Connectivity;
using Xamarin.Forms;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Views
{
    public partial class MyAttendancesPage : ContentPage
    {
        public MyAttendancesPage()
        {
            InitializeComponent();
            List<AttendanceModel> tsresult = Controller.InstanceCreation().GetAttendanceDetailsData();

            if (CrossConnectivity.Current.IsConnected == true)
            {
                expenselist.ItemsSource = tsresult;
            }
            else
            {
                // expenselist.ItemsSource = App.expense_listDb;
            }


        }


        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void myattendance_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            AttendanceModel data = (AttendanceModel)e.Item;
          //  Navigation.PushAsync(new ExpenseDetailPage(data));
            Navigation.PushAsync(new MyAttendancesDetailPage(data));
        }

    }
}