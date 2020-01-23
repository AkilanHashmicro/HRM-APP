using HRMAPP.Model;
using HRMAPP.Pages;
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
	public partial class DashboardPage : ContentPage
	{

        public DashboardPage ()
		{
			InitializeComponent ();

          //List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();

          // act_ind.IsRunning = true;

            var leave_menu = new TapGestureRecognizer();
            leave_menu.Tapped += async (s, e) =>
            {

                act_ind.IsRunning = true;

                await Task.Run(() => App.Current.MainPage = new MasterPage(new LeavesPage()));

                act_ind.IsRunning = false;

            };
            leave_layout.GestureRecognizers.Add(leave_menu);

            var expense_menu = new TapGestureRecognizer();
            expense_menu.Tapped += async (s, e) =>
            {
               act_ind.IsRunning = true;

                await Task.Run(() => App.Current.MainPage = new MasterPage(new ExpensePage()));

               act_ind.IsRunning = false;

            };
            expense_layout.GestureRecognizers.Add(expense_menu);

            //var expense_menu1 = new TapGestureRecognizer();
            //expense_menu1.Tapped += async (s, e) =>
            //{
            ////    act_ind.IsRunning = true;

            //    await Task.Run(() => App.Current.MainPage = new MasterPage(new ExpensePage()));

            //  //  act_ind.IsRunning = false;

            //};
           // expense_layout1.GestureRecognizers.Add(expense_menu);

            var attendance_menu = new TapGestureRecognizer();
            attendance_menu.Tapped += async (s, e) =>
            {
                act_ind.IsRunning = true;

                await Task.Run(() => App.Current.MainPage = new MasterPage(new AttendancesPage()));
            
               act_ind.IsRunning = false;

            };
            attendance_layout.GestureRecognizers.Add(attendance_menu);

            var timesheet_menu = new TapGestureRecognizer();
            timesheet_menu.Tapped += async (s, e) =>
            {
               act_ind.IsRunning = true;

                await Task.Run(() => App.Current.MainPage = new MasterPage(new TimesheetsPage()));

                act_ind.IsRunning = false;

            };
            timesheet_layout.GestureRecognizers.Add(timesheet_menu);

           // act_ind.IsRunning = false;
        }

        //async void Loadingalertcall()
        //{
        //    await PopupNavigation.Instance.PopAsync();
        //}


        //async void newpagecall()
        //{
        //    await Navigation.PushAsync(new MasterPage(new LeavesPage()));
        //}

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}