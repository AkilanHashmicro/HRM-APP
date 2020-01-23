using HRMAPP.DBModel;
using HRMAPP.Model;
using HRMAPP.Pages;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExpenseDetailPage : ContentPage
	{
        ExpenseModel expobj = new ExpenseModel();
        ExpenseModelDB expobjDb = new ExpenseModelDB();

        List<AttachmentFile> attach = new List<AttachmentFile>();
        List<int> delete_attach = new List<int>();

        int uom_id = 0;
        int exp_prod_id = 0;

        bool exp_officer = true;
        bool exp_manager = true;

		public ExpenseDetailPage (ExpenseModel obj)
		{
			InitializeComponent ();

            expobj = obj;
            NavigationPage.SetHasNavigationBar(this, false);

            if(obj.state == "draft")
            {
                btnsubmit.IsVisible = true;
               
            }

            else if(obj.state == "reported")
            {
                btnApprove.IsVisible = true;
                btnApprove.BackgroundColor = Color.FromHex(obj.stage_colour);
            }

            uom_id = App.product_Uom.FirstOrDefault(x => x.Name == obj.product_uom).Id;

            exp_prod_id = App.expense_productList.FirstOrDefault(x => x.Name == obj.product).id;

          //  countryid = App.countrydict.FirstOrDefault(x => x.Value == country_picker.SelectedItem.ToString()).Key;

            description_Name.Text = obj.name;
            productlabel.Text = obj.product;
            qtylabel.Text = obj.product_uom;

            //if(obj.currencyName.Equals("IDR"))
            //{
            //    pricelabel.Text = obj.unit_amount + " " + "RP";
            //}
            //else 
            //{
            //    pricelabel.Text = obj.unit_amount.ToString();
            //}

            pricelabel.Text = obj.unit_price.ToString();

            qtylabel.Text = obj.quantity +" "+ obj.product_uom;
            referencelabel.Text = obj.reference;
            datelabel.Text = obj.full_date;
          //  accountlabel.Text = obj.account;
            employeelabel.Text = obj.employee_id; 
            currencylabel.Text = obj.currency;


            attach = obj.attachment;
            if (attach.Count > 0)
            {
                attachviewlist.IsVisible = true;
                attachviewlist.HeightRequest = 45 * attach.Count;
                attachviewlist.ItemsSource = attach;
            }


            if (obj.state.Equals("draft"))
            {
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.stage_name;
            }
            else if(obj.state.Equals("reported"))
            {
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.stage_name;
            }
            else if(obj.state.Equals("done"))
            {
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.stage_name;
            }
            else
            {
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.stage_name;
            }


             exp_officer = App.access_dict.FirstOrDefault(x => x.Key == "expense_officer").Value;

             exp_manager = App.access_dict.FirstOrDefault(x => x.Key == "expense_manager").Value;


            if (!exp_officer || !exp_manager)
            {
                if (obj.state.Equals("draft"))
                {
                    frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                    statelabel.Text = obj.stage_name;
                }

                else if (obj.state.Equals("reported"))
                {
                    btnStack.IsVisible = false;
                }

                else
                {
                    btnStack.IsVisible = false;
                }
                //else if (obj.state.Equals("draft"))
                //{
                //    btnConfirm.IsVisible = true;
                //    btnApprove.IsVisible = false;
                //    btnRefuse.IsVisible = false;
                //    btnDraft.IsVisible = false;
                //    frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                //    statelabel.Text = obj.state_name;
                //}
            }

            var editRecognizer = new TapGestureRecognizer();
            editRecognizer.Tapped += (s, e) =>
            {
                edit.IsVisible = false;
                updatebtn.IsVisible = true;

                searchprod.Text = obj.product;

                attachviewlist.IsEnabled = true;
                add_attachment.IsEnabled = true;

                des_layout.IsVisible = true;
                description_Name.IsVisible = false;
                expdescription_entry.Text = obj.name;

                productlabel.IsVisible = false;
                product_layout.IsVisible = true;

                pricelabel.IsVisible = false;
                price_entry.IsVisible = true;
                price_entry.Text = obj.unit_price.ToString();

                qtylabel.IsVisible = false;
                qty_layout.IsVisible = true;
                qty.Text = obj.quantity.ToString();
                qtyvalue.Text = obj.product_uom;

                referencelabel.IsVisible = false;
                reference_entry.IsVisible = true;
                reference_entry.Text = obj.reference;

                datelabel.IsVisible = false;
                date_layout.IsVisible = true;
                DateTime date = DateTime.ParseExact(obj.date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                start_date.Date = date;

                employeelabel.IsVisible = false;
                employee_layout.IsVisible = true;

                //product_picker.ItemsSource = App.expense_productList.Select(n => n.Name).ToList();
                //product_picker.SelectedIndex = 0;
                //product_picker.SelectedItem = obj.product;

                employee_picker.ItemsSource = App.employee_list.Select(n => n.name).ToList();
                employee_picker.SelectedIndex = 0;
                employee_picker.SelectedItem = obj.employee_id;

                currencylabel.IsVisible = false;
                currency_layout.IsVisible = true;

                currency_picker.ItemsSource = App.currencyList.Select(n => n.Name).ToList();
                currency_picker.SelectedIndex = 0;
                currency_picker.SelectedItem = obj.currency;

            };
            edit.GestureRecognizers.Add(editRecognizer);


            var attach_file = new TapGestureRecognizer();
            attach_file.Tapped += async (s, e) =>
            {

                System.Diagnostics.Debug.WriteLine("OUTT" + attach.Count());
                await PopupNavigation.Instance.PushAsync(new FileAttachmentPage("update", attach));

            };
            add_attachment.GestureRecognizers.Add(attach_file);



            var backImgRecognizer = new TapGestureRecognizer();
            backImgRecognizer.Tapped += (s, e) => {
                // handle the tap              
                // Navigation.PopAllPopupAsync();
                act_ind.IsRunning = true;

              //  Controller.InstanceCreation().GetExpenseData();

                App.Current.MainPage = new MasterPage(new ExpensePage());

             //   act_ind.IsRunning = false;
            };
            backImg.GestureRecognizers.Add(backImgRecognizer);

        }


        public ExpenseDetailPage(ExpenseModelDB obj)
        {
            InitializeComponent();

            expobjDb = obj;

            description_Name.Text = obj.name;
            productlabel.Text = obj.product;
      

         //  datelabel.IsVisible = true;

            //DateTime dt = DateTime.ParseExact(obj.date, "yyyy-MM-dd", null);
            //string convert = dt.ToLocalTime().ToString("dd/MM/yyyy");

          //  dateentry.Text = obj.date;

            datelabel.Text = obj.date;

            referencelabel.Text = obj.reference;

            searchprod.Text = expobjDb.product;

            //if(obj.currencyName.Equals("IDR"))
            //{
            //    pricelabel.Text = obj.unit_amount + " " + "RP";
            //}
            //else 
            //{
            //    pricelabel.Text = obj.unit_amount.ToString();
            //}

            pricelabel.Text = obj.unit_price.ToString();

            qtylabel.Text = obj.quantity + " " + obj.product_uom;
            referencelabel.Text = obj.reference;
           // datelabel.Text = obj.full_date;
          //  accountlabel.Text = obj.account;
            employeelabel.Text = obj.employee_id;
            currencylabel.Text = obj.currency;

            if (obj.state.Equals("draft"))
            {
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.stage_name;
            }
            else if (obj.state.Equals("reported"))
            {
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.stage_name;
            }
            else if (obj.state.Equals("done"))
            {
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.stage_name;
            }
            else
            {
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.stage_name;
            }

            attachviewlist.IsEnabled = true;
            add_attachment.IsEnabled = true;


         //   attach = expobjDb.attachment;
            if (attach.Count > 0)
            {
                attachviewlist.IsVisible = true;
                attachviewlist.HeightRequest = 45 * attach.Count;
                attachviewlist.ItemsSource = attach;
            }

            var editRecognizer = new TapGestureRecognizer();
            editRecognizer.Tapped += (s, e) =>
            {
                if (CrossConnectivity.Current.IsConnected) 
              { 
                    attachviewlist.IsEnabled = true;

                //  attach_layout.IsEnabled = true;

                    edit.IsVisible = false;
                    updatebtn.IsVisible = true;

                    productlabel.IsVisible = false;
                    product_layout.IsVisible = true;

                    pricelabel.IsVisible = false;
                    price_entry.IsVisible = true;
                    price_entry.Text = obj.unit_price.ToString();

                    qtylabel.IsVisible = false;
                    qty_layout.IsVisible = true;
                    qty.Text = obj.quantity.ToString();

                    referencelabel.IsVisible = false;
                    reference_entry.IsVisible = true;
                    reference_entry.Text = obj.reference;

                    datelabel.IsVisible = false;
                    date_layout.IsVisible = true;
                    DateTime date = DateTime.ParseExact(obj.date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    start_date.Date = date;

                    employeelabel.IsVisible = false;
                    employee_layout.IsVisible = true;

                                     
                }
                else
                {
                    attachviewlist.IsEnabled = true;

                    des_layout.IsVisible = true;
                    description_Name.IsVisible = false;
                    expdescription_entry.Text = obj.name;

                //  attach_layout.IsEnabled = true;

                    edit.IsVisible = false;
                    updatebtn.IsVisible = true;

                    productlabel.IsVisible = false;
                    product_layout.IsVisible = true;

                    pricelabel.IsVisible = false;
                    price_entry.IsVisible = true;
                    price_entry.Text = obj.unit_price.ToString();

                    qtylabel.IsVisible = false;
                    qty_layout.IsVisible = true;
                    qty.Text = obj.quantity.ToString();

                    referencelabel.IsVisible = false;
                    reference_entry.IsVisible = true;
                    reference_entry.Text = obj.reference;

                    datelabel.IsVisible = false;
                    date_layout.IsVisible = true;
                    DateTime date = DateTime.ParseExact(obj.date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    start_date.Date = date;

                    employeelabel.IsVisible = false;
                    employee_layout.IsVisible = true;
                                                                                                                                    
                    employee_picker.ItemsSource = App.employee_listDb.Select(n => n.name).ToList();
                    employee_picker.SelectedIndex = 0;
                    employee_picker.SelectedItem = expobjDb.employee_id;

                    currencylabel.IsVisible = false;
                    currency_layout.IsVisible = true;

                    currency_picker.ItemsSource = App.currencyListDb.Select(n => n.Name).ToList();
                    currency_picker.SelectedIndex = 0;
                    currency_picker.SelectedItem = obj.currency;


                    List < AttachmentFile > attachlistdb = JsonConvert.DeserializeObject<List<AttachmentFile>>(expobjDb.attachment);

                    attach = attachlistdb;
                    if (attach.Count > 0)
                    {
                        attachviewlist.IsVisible = true;
                        attachviewlist.HeightRequest = 45 * attach.Count;
                        attachviewlist.ItemsSource = attach;
                    }

                  //  DependencyService.Get<Toast>().Show("Need internet connection...");
                }

            };
            edit.GestureRecognizers.Add(editRecognizer);

            var attach_file = new TapGestureRecognizer();
            attach_file.Tapped += async (s, e) =>
            {
                
                System.Diagnostics.Debug.WriteLine("OUTT" + attach.Count());
                await PopupNavigation.Instance.PushAsync(new FileAttachmentPage("update", attach));

            };
            add_attachment.GestureRecognizers.Add(attach_file);

        }

        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                //var currentpage = new LoadingIndicator();
                //await PopupNavigation.Instance.PushAsync(currentpage);

                act_ind.IsRunning = true;

                //submit to manager method
                            
                bool updated = Controller.InstanceCreation().submittomanager("hr.expense", "expense_submit_to_manager", expobj.id);
             //   Loadingalertcall();

                if (updated)
                {
                    DependencyService.Get<Toast>().Show("Updated Successfully...");

                    statelabel.Text = "Posted";

                    if (!exp_officer || !exp_manager)
                    {
                        btnStack.IsVisible = false;
                    }

                    else
                    {
                        btnApprove.IsVisible = true;
                    }
                   // btnApprove.
                    act_ind.IsRunning = false;
                }
                else
                {
                    DependencyService.Get<Toast>().Show("Try Again...");
                    act_ind.IsRunning = false;
                }
            }

            else
            {
                DependencyService.Get<Toast>().Show("Need internet connection...");
                act_ind.IsRunning = false;
            }

            act_ind.IsRunning = false;
        }


        private async void btnApprove_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                //var currentpage = new LoadingIndicator();
                //await PopupNavigation.Instance.PushAsync(currentpage);

                act_ind.IsRunning = true;

                //submit to manager method

                //bool exp_officer = App.access_dict.FirstOrDefault(x => x.Key == "expense_officer").Value;

                //bool exp_manager = App.access_dict.FirstOrDefault(x => x.Key == "expense_manager").Value;


                bool updated = Controller.InstanceCreation().submittomanager("hr.expense", "app_approve_expense_sheet", expobj.id);
              //  Loadingalertcall();

                if (updated)
                {
                    DependencyService.Get<Toast>().Show("Updated Successfully...");
                    btnStack.IsVisible = false;
                    act_ind.IsRunning = false;

                    statelabel.Text = "Approved";
                }
                else
                {
                    DependencyService.Get<Toast>().Show("Try Again...");
                    act_ind.IsRunning = false;
                }
                act_ind.IsRunning = false;
            }

            else
            {
                DependencyService.Get<Toast>().Show("Need internet connection...");
            }
        }

        private void delete_Tapped(object sender, EventArgs e)
        {
            try
            {
                var args = (TappedEventArgs)e;

                AttachmentFile t2 = args.Parameter as AttachmentFile;

                var itemToRemove = attach.Single(r => r.file_name == t2.file_name);

                attach.Remove(itemToRemove);

                if (t2.id != 0)
                {
                    try
                    {
                        delete_attach.Add(t2.id);

                       // var updated = Controller.InstanceCreation().deleteattachment("hr.expense", "remove_attachment", t2.id);
                    }
                    catch
                    {
                        int j = 0;
                    }
                }

                attachviewlist.ItemsSource = null;
                attachviewlist.ItemsSource = attach;
                attachviewlist.HeightRequest = 45 * attach.Count;

                if (attach.Count == 0)
                {
                    attachviewlist.IsVisible = false;
                }

            }
            catch (Exception ex)
            {

            }
        }


        private void updatecancel_clickedAsync(object sender, EventArgs e)
        {
            edit.IsVisible = true;
            updatebtn.IsVisible = false;

            description_Name.IsVisible = true;
            des_layout.IsVisible = false;

            attachviewlist.IsEnabled = false;

            currency_layout.IsVisible = false;
            currencylabel.IsVisible = true;
            
            productlabel.IsVisible = true;
            product_layout.IsVisible = false;

            pricelabel.IsVisible = true;
            price_entry.IsVisible = false;

            qtylabel.IsVisible = true;
            qty_layout.IsVisible = false;

            referencelabel.IsVisible = true;
            reference_entry.IsVisible = false;

            datelabel.IsVisible = true;
            date_layout.IsVisible = false;

            employeelabel.IsVisible = true;
            employee_layout.IsVisible = false;

        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#F0EEEF");
        }

        public void productentry(object sender, EventArgs args)
        {
            // PopupNavigation.Instance.PushAsync(new PickerSelectionPage());
            PopupNavigation.Instance.PushAsync(new PickerSelectionPage("expense"));
        }

        protected override void OnAppearing()
        {
           
            base.OnAppearing();

            MessagingCenter.Subscribe<string, int>("MyApp", "PickerMsg", (sender, arg) =>
            {

                if (App.productList.Count != 0)
                {
                    var productlis = from pro in App.expense_productList
                                     where pro.id == arg
                                     select pro;

                    foreach (var prodresults in productlis)
                    {
                        searchprod.Text = prodresults.Name;
                        price_entry.Text = prodresults.list_price;
                        qtyvalue.Text = prodresults.uom_name;
                        uom_id = prodresults.uom_id;
                        exp_prod_id = prodresults.id;

                    }
                }
                else
                {
                    var productlis = from pro in App.expense_productListDb
                                     where pro.id == arg
                                     select pro;
                    foreach (var prodresults in productlis)
                    {
                        searchprod.Text = prodresults.Name;
                        price_entry.Text = prodresults.list_price;
                        qtyvalue.Text = prodresults.uom_name;
                        uom_id = prodresults.uom_id;
                        exp_prod_id = prodresults.id;
                    }

                }

                int i = 0;

            });

            MessagingCenter.Subscribe<string, List<AttachmentFile>>("MyApp", "updategalleryMsg", async (sender, arg) =>
            {
                attachviewlist.IsVisible = true;
                attachviewlist.ItemsSource = null;
                attachviewlist.HeightRequest = 45 * arg.Count;
                attachviewlist.ItemsSource = arg;
                // System.Diagnostics.Debug.WriteLine("fileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" + attachviewlist.HeightRequest);    
            });

            MessagingCenter.Subscribe<string, List<AttachmentFile>>("MyApp", "updatecameraMsg", async (sender, arg) =>
            {

                attachviewlist.IsVisible = true;
                //  attachviewlist.ItemsSource = attachnew;
                attachviewlist.ItemsSource = null;
                attachviewlist.ItemsSource = arg;
                attachviewlist.HeightRequest = 45 * arg.Count;
                System.Diagnostics.Debug.WriteLine("fileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" + attach.Count);


            });

        }


        private async void update_clickedAsync(object sender, EventArgs e)
        {
            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

            if (expdescription_entry.Text == "")
            {
                exdes_alert.IsVisible = true;
                product_alert.IsVisible = false;
            }

            else if (searchprod.Text == "")
            {
                product_alert.IsVisible = true;
                exdes_alert.IsVisible = false;
            }

            else
            {
                act_ind.IsRunning = true;
                //var currentpage = new LoadingIndicator();
                //await PopupNavigation.Instance.PushAsync(currentpage);

                product_alert.IsVisible = false;
                exdes_alert.IsVisible = false;

                vals["name"] = expdescription_entry.Text;
                vals["product_id"] = exp_prod_id;
                vals["unit_amount"] = price_entry.Text;
                vals["quantity"] = qty.Text;
                vals["product_uom_id"] = uom_id;
                vals["reference"] = reference_entry.Text;

                vals["date"] = start_date.Date.ToString("yyyy-MM-dd");

                //  vals["total_amount"] = total;

                List<dynamic> attfile = new List<dynamic>();
                foreach (var data in attach)
                {

                    if (data.id == 0)
                    {
                        Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

                        dict.Add("file_name", data.file_name);
                        // dict.Add("file_data", data.filebase64);
                        dict.Add("file_data", data.file_data);
                        attfile.Add(dict);
                    }                   
                }

                vals["add_attachment"] = attfile;

                vals["remove_attachment"] = delete_attach;

                if (CrossConnectivity.Current.IsConnected)
                {

                    int emp_id = (App.employee_list.AsEnumerable().Where(p => p.name == employee_picker.SelectedItem.ToString()).Select(p => p.id)).FirstOrDefault();
                    int cur_id = (App.currencyList.AsEnumerable().Where(p => p.Name == currency_picker.SelectedItem.ToString()).Select(p => p.id)).FirstOrDefault();

                    vals["employee_id"] = emp_id;
                    vals["currency_id"] = cur_id;

                    var created = Controller.InstanceCreation().UpdateLeave("hr.expense", "app_update_expense", expobj.id, vals);

                    if (created == "True")
                    {
                        App.Current.MainPage = new MasterPage(new ExpensePage());

                       // Loadingalertcall();

                        act_ind.IsRunning = false;

                        DependencyService.Get<Toast>().Show("Updated Successfully...");
                    }
                    else
                    {
                      //  Loadingalertcall();
                        DependencyService.Get<Toast>().Show(created);

                        act_ind.IsRunning = false;
                    }

                }

                else 
                {
                    int emp_id = (App.employee_listDb.AsEnumerable().Where(p => p.name == employee_picker.SelectedItem.ToString()).Select(p => p.id)).FirstOrDefault();
                    int cur_id = (App.currencyListDb.AsEnumerable().Where(p => p.Name == currency_picker.SelectedItem.ToString()).Select(p => p.id)).FirstOrDefault();
                    int edit_exp_prod_id = (App.expense_productListDb.AsEnumerable().Where(p => p.Name == searchprod.Text.ToString()).Select(p => p.id)).FirstOrDefault();
                    int edit_exp_uom_id = (App.expense_productListDb.AsEnumerable().Where(p => p.Name == searchprod.Text.ToString()).Select(p => p.uom_id)).FirstOrDefault();

                    string jso_addattchment_list = "";
                    string jso_removeattchment_list = "";

                    try
                    {
                         jso_addattchment_list = Newtonsoft.Json.JsonConvert.SerializeObject(attfile);

                    }

                    catch
                    {
                        int j = 0;
                    }

                    try
                    {
                        jso_removeattchment_list = Newtonsoft.Json.JsonConvert.SerializeObject(delete_attach);
                    }
                    catch
                    {
                        int jk = 0;
                    }

                    DateTime dt = DateTime.ParseExact(start_date.Date.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
                    string convert_halfdate = dt.ToLocalTime().ToString("MMMM dd");
                                       
                    var itemToRemove = App.expense_listDb.Single(r => r.id == expobjDb.id);
                    App.expense_listDb.Remove(itemToRemove);

                    App._connection.Query<ExpenseModelDB>("DELETE FROM [ExpenseModelDB] WHERE [id] = " + expobjDb.id);

                    var sample = new ExpenseModelDB
                    {
                        id = expobjDb.id,
                        name = expdescription_entry.Text,
                        product = searchprod.Text,
                        product_id = edit_exp_prod_id,
                        unit_price = float.Parse(price_entry.Text),
                        uom_id = edit_exp_uom_id,
                        quantity = float.Parse(qty.Text),
                        reference = reference_entry.Text,
                        sync_string = "editexp.png",
                        stage_colour = expobjDb.stage_colour,
                        stage_name = expobjDb.stage_name,
                        state = expobjDb.state,
                        attachment = jso_addattchment_list,
                        remove_attachment = jso_removeattchment_list,
                        date = start_date.Date.ToString("yyyy-MM-dd"),
                        half_date = convert_halfdate,
                        employee_id = employee_picker.SelectedItem.ToString(),
                        emp_id = emp_id,
                        currency = currency_picker.SelectedItem.ToString(),
                        currency_id = cur_id,
                     //   total = total,
                        check_rpc_condition = "edit",
                        // error_attachment_list = attach_list,
                    };
                    App._connection.Insert(sample);

                    App._connection.CreateTable<ExpenseModelDB>();
                    try
                    {
                        var details = (from y in App._connection.Table<ExpenseModelDB>() select y).ToList();
                        App.expense_listDb = details;

                        App.Current.MainPage = new MasterPage(new ExpensePage());
                        DependencyService.Get<Toast>().Show("Updated locally...");
                    }
                    catch (Exception ex)
                    {
                        int i = 0;
                        act_ind.IsRunning = false;
                    }

                    act_ind.IsRunning = false;

                }

            }
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
        
    }
}