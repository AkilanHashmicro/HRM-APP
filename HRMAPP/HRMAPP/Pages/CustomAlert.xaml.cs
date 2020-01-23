using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HRMAPP.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomAlert : PopupPage
    {
        string msgtxt = "";       
        public CustomAlert (string text,string title)
		{
			InitializeComponent ();

            Title.Text = title;
            body.Text = text;

            msgtxt = text;


        }

        private void Okbtn_Clicked(object sender, EventArgs e)
        {
            if(msgtxt == "Are you sure you want to log out?")
            {
                MessagingCenter.Send<string, bool>("MyApp", "logoutMsg", true);
                PopupNavigation.Instance.PopAsync();
            }
            else
            {
                MessagingCenter.Send<string, bool>("MyApp", "deleteMsg", true);
                PopupNavigation.Instance.PopAsync();
            }

        }

        private void Cancelbtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}