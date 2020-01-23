using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class PickerSelectionPage : PopupPage
	{
        List<ProductsList> presult = new List<ProductsList>();
        List<ExpenseProductsList> expense_presult = new List<ExpenseProductsList>();

        string page_name = "";

        public PickerSelectionPage ()
		{
			InitializeComponent ();

            if (App.NetAvailable == true)
            {
                presult = App.productList;
                pickerListView.ItemsSource = presult;
            }

            if (App.NetAvailable == false)
            {

                presult = App.ProductListDb;
                pickerListView.ItemsSource = presult;
            }

        }

        public PickerSelectionPage(string page)
        {
            InitializeComponent();
            page_name = page;
            if (App.NetAvailable == true)
            {
                expense_presult = App.expense_productList;
                pickerListView.ItemsSource = expense_presult;
            }

            if (App.NetAvailable == false)
            {

                expense_presult = App.expense_productListDb;
                pickerListView.ItemsSource = expense_presult;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (page_name == "")
            {

                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    pickerListView.ItemsSource = App.productList;
                }
                else
                {
                    pickerListView.ItemsSource = App.productList.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }

            else
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    pickerListView.ItemsSource = App.expense_productList;
                }
                else
                {
                    pickerListView.ItemsSource = App.expense_productList.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }
        }

        private void pickerListView_ItemTapped(object sender, ItemTappedEventArgs ea)
        {
            if (page_name == "")
            {
                ProductsList masterItemObj = (ProductsList)ea.Item;
                MessagingCenter.Send<string, int>("MyApp", "PickerMsg", masterItemObj.Id);
                PopupNavigation.Instance.PopAsync();
            }

            else
            {
                ExpenseProductsList masterItemObj = (ExpenseProductsList)ea.Item;
                MessagingCenter.Send<string, int>("MyApp", "PickerMsg", masterItemObj.id);
                PopupNavigation.Instance.PopAsync();
            }


        }


        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#f0eaea");
        }

    }
}