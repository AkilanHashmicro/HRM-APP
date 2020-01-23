using System;
using System.Collections.Generic;
using HRMAPP.Model;
using Plugin.Connectivity;
using Xamarin.Forms;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Views
{
    public partial class TimeSheetActivitiesPage : ContentPage
    {
        public TimeSheetActivitiesPage()
        {            
            InitializeComponent();

            List<TimesheetDetailsModel> tsresult = Controller.InstanceCreation().GetTimesheetDetailsData();

            if (CrossConnectivity.Current.IsConnected == true)
            {
                expenselist.ItemsSource = tsresult;
            }
            else
            {
                // expenselist.ItemsSource = App.expense_listDb;
            }



        }

        //private void RefreshRequested(object sender, object e)
        //{

        //    expenselist.IsRefreshing = true;

        //    if (CrossConnectivity.Current.IsConnected)
        //    {
        //        expenselist.ItemsSource = null;

        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            Task.Run(() => Controller.InstanceCreation().GetMasterData());
        //            //  List<LeavesModel> expense = Controller.InstanceCreation().GetMasterData();
        //            expenselist.ItemsSource = App.expense_list;
        //            expenselist.IsRefreshing = false;

        //        });
        //    }
        //    else
        //    {
        //        expenselist.ItemsSource = App.expense_listDb;

        //        expenselist.IsRefreshing = false;

        //    }

        //}



        protected override bool OnBackButtonPressed()
        {
            return true;
        }



    }
}