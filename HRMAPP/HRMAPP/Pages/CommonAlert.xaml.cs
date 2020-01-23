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
	public partial class CommonAlert : PopupPage
	{
        string msgtxt = "";
		public CommonAlert (string text)
		{
			InitializeComponent ();

            Title.Text = "Alert";
            body.Text = text;

            msgtxt = text;
        }

        private void okbtn_Clicked(object sender, EventArgs e)
        {           
            PopupNavigation.Instance.PopAsync();
        }

       
    }
}