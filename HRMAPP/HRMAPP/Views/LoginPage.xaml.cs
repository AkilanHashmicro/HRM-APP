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
	public partial class LoginPage : ContentPage
	{
        private Controller controllerObj = Controller.InstanceCreation();

     //   ActivityIndicator activityIndicator;
        public LoginPage ()
		{
			InitializeComponent ();

            Label header = new Label
            {
                Text = "ActivityIndicator",
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            //activityIndicator = new ActivityIndicator
            //{
                 
            //    Color = Device.OnPlatform(Color.Black, Color.Default, Color.Default),
            //    IsRunning = false,
            //    VerticalOptions = LayoutOptions.CenterAndExpand
                                               
            //};

            UserTxt.Text = "admin@example.com";
            PassTxt.Text = "admin";
            UrlTxt.Text = "https://beta-dev1.hashmicro.com";    // http://192.168.1.228:8069 //http://beta-dev1.hashmicro.com

            var showgesture = new TapGestureRecognizer();
            showgesture.Tapped += (s, e) =>
            {
                show_password_layout.IsVisible = false;
                hide_password_layout.IsVisible = true;
                PassTxt.IsPassword = false;
            };
            eye_view.GestureRecognizers.Add(showgesture);

            var hidegesture = new TapGestureRecognizer();
            hidegesture.Tapped += (s, e) =>
            {
                show_password_layout.IsVisible = true;
                hide_password_layout.IsVisible = false;
                PassTxt.IsPassword = true;
            };
            eye_close.GestureRecognizers.Add(hidegesture);

        }

        private void UrlTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            string urlText = ((Entry)sender).Text;
            databasePic.Items.Clear();
            databasePic.IsVisible = false;

            if (Util.Net.ValidateURL(urlText) && urlText.Length > 10)
            {

                try
                {
                    String[] dbData = controllerObj.getDatabases(urlText);

                    foreach (String db in dbData)
                    {
                        databasePic.Items.Add(db);
                    }

                    if (dbData == null)
                    {
                        url_tickimg.IsVisible = false;
                        db_layout.IsVisible = false;
                        db_layout1.IsVisible = false;                        
                    }
                    else
                    {
                        url_tickimg.IsVisible = true;
                        db_layout.IsVisible = true;
                        db_layout1.IsVisible = true;                      
                    }


                    databasePic.SelectedIndex = 0;
                    if (dbData.Length >= 1)
                    {
                        databasePic.IsVisible = true;                       
                    }

                }
                catch (Exception ex)
                {
                    url_tickimg.IsVisible = false;
                    db_layout.IsVisible = false;
                    db_layout1.IsVisible = false;                  
                }
            }

        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                //var currentpage = new LoadingIndicator();
                //await PopupNavigation.Instance.PushAsync(currentpage);

                act_ind.IsRunning = true;

                string UserName = UserTxt.Text;
                string UserPassword = PassTxt.Text;
                string UserUrlName = UrlTxt.Text;
                string UserDbName = databasePic.SelectedItem.ToString();


              //  activityIndicator.IsRunning = true;


                string res = await Task.Run(() => controllerObj.login(UserUrlName, UserDbName, UserName, UserPassword));

          //      await Task.Delay((0)).ContinueWith((arg) =>
          //      {

          //          Device.BeginInvokeOnMainThread(() =>
          //          {
          //              activityIndicator.IsRunning = false;

          //          });
          //      }
          //);

                if(res == "false")
                {
                    loginfailedAlert.Text = "Invalid Username or Password.";
                    loginfailedAlert.IsVisible = true;
                 //   act_ind.IsRunning = false;
                  //  Loadingalertcall();
                }
                else
                {
                    Settings.UserName = UserTxt.Text;
                    Settings.UserPassword = PassTxt.Text;
                    Settings.UserUrlName = UrlTxt.Text;
                    Settings.UserDbName = databasePic.SelectedItem.ToString();

                    List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();

                    Page pageRef = new DashboardPage();
                    App.Current.MainPage = new MasterPage(pageRef);
                }

                act_ind.IsRunning = false;



              //  Loadingalertcall();

            }
            catch (Exception ex)
            {
                loginfailedAlert.Text = "Invalid Username or Password.";
                loginfailedAlert.IsVisible = true;
                act_ind.IsRunning = false;
               // Loadingalertcall();
            }

            
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.Instance.PopAsync();
        }


    }
}