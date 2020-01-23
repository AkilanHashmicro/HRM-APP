using HRMAPP.DBModel;
using HRMAPP.Model;
using HRMAPP.Views;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseCreation : PopupPage
    {

        List<AttachmentFile> attach = new List<AttachmentFile>();
        List<taxes> taxlist = new List<taxes>();
        double total = 0;
        int uom_id = 0;
        int exp_prod_id = 0;
        public ExpenseCreation()
        {
            InitializeComponent();

            var closeRecognizer = new TapGestureRecognizer();
            closeRecognizer.Tapped += (s, e) =>
            {
                PopupNavigation.Instance.PopAllAsync();
            };
            overall_close.GestureRecognizers.Add(closeRecognizer);

            //var addRecognizer = new TapGestureRecognizer();
            //addRecognizer.Tapped += (s, e) =>
            //{
            //    tax_stacklayout.IsVisible = true;
            //    Addtax_line.IsVisible = false;
            //};
            //Addtax_line.GestureRecognizers.Add(addRecognizer);


            var dropdownImgRecognizer = new TapGestureRecognizer();
            dropdownImgRecognizer.Tapped += (s, e) =>
            {
                PopupNavigation.Instance.PushAsync(new PickerSelectionPage());
            };
            pickerdropimg.GestureRecognizers.Add(dropdownImgRecognizer);

            if(App.NetAvailable == true)
            {
             //   product_UOM_picker.ItemsSource = App.product_Uom.Select(n => n.Name).ToList();
             //  // product_UOM_picker.SelectedIndex = 0;

             //var key =   App.product_Uom.Select(n => n.Name).ToList().IndexOf("Unit(s)");

                //product_UOM_picker.SelectedIndex = key;

                currency_picker.ItemsSource = App.currencyList.Select(n => n.Name).ToList();
                currency_picker.SelectedIndex = 0;

                //taxes_picker.ItemsSource = App.taxList.Select(t => t.Name).ToList();
                //taxes_picker.SelectedIndex = -1;

                employee_picker.ItemsSource = App.employee_list.Select(t => t.name).ToList();
                employee_picker.SelectedIndex = 0;
            }
            else
            {
                //product_UOM_picker.ItemsSource = App.product_UomDb.Select(n => n.Name).ToList();
                //product_UOM_picker.SelectedIndex = 0;

                currency_picker.ItemsSource = App.currencyListDb.Select(n => n.Name).ToList();
                currency_picker.SelectedIndex = 0;

                employee_picker.ItemsSource = App.employee_listDb.Select(t => t.name).ToList();
                employee_picker.SelectedIndex = 0;

                //taxes_picker.ItemsSource = App.taxListDb.Select(t => t.Name).ToList();
                //taxes_picker.SelectedIndex = -1;
            }
          

            //var employee = new List<string>();
            //employee.Add("Test");
            //employee.Add("Test1");
            //employee.Add("Test2");
            //employee_picker.ItemsSource = employee;
            //employee_picker.SelectedIndex = 0;

            //var rbtnRecognizer = new TapGestureRecognizer();
            //rbtnRecognizer.Tapped += (s, e) =>
            //{
            //    fillimg1.IsVisible = true;
            //    fillimg2.IsVisible = false;
            //};
            //empimg1.GestureRecognizers.Add(rbtnRecognizer);

            //var rbtnRecognizer1 = new TapGestureRecognizer();
            //rbtnRecognizer1.Tapped += (s, e) =>
            //{
            //    fillimg2.IsVisible = true;
            //    fillimg1.IsVisible = false;
            //};
            //empimg2.GestureRecognizers.Add(rbtnRecognizer1);

            var attach_file = new TapGestureRecognizer();
            attach_file.Tapped += async (s, e) =>
            {
               // await PopupNavigation.Instance.PushAsync(new FileAttachmentPage());

                await PopupNavigation.Instance.PushAsync(new FileAttachmentPage("update", attach));

            };
            add_attachment.GestureRecognizers.Add(attach_file);
           
        }

        private void discardbtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        //private void Taxes_picker_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    taxes data = new taxes("");

        //    try
        //    {
        //        data.Name = taxes_picker.SelectedItem.ToString();
        //        data.ID = taxes_picker.SelectedIndex;

        //        taxlist.Add(data);

        //        taxlist = taxlist.GroupBy(i => i.Name).Select(g => g.First()).ToList();

        //        taxListView.HeightRequest = 45 * taxlist.Count;
        //        taxListView.ItemsSource = null;
        //        taxListView.ItemsSource = taxlist;
        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //    Addtax_line.IsVisible = true;
        //    list_tax_layout.IsVisible = true;
        //    tax_stacklayout.IsVisible = false;
        //    taxes_picker.SelectedIndex = -1;
        //}

        private void cancelGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //try
            //{
            //    var args = (TappedEventArgs)e;

            //    taxes t2 = args.Parameter as taxes;

            //    var itemToRemove = taxlist.Single(r => r.Name == t2.Name);

            //    taxlist.Remove(itemToRemove);

            //    taxListView.ItemsSource = null;
            //    taxListView.ItemsSource = taxlist;
            //    taxListView.HeightRequest = 45 * taxlist.Count;

            //    if (taxlist.Count == 0)
            //    {
            //        tax_stacklayout.IsVisible = false;
            //        Addtax_line.IsVisible = true;
            //        list_tax_layout.IsVisible = false;
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
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
                        unit_price.Text = prodresults.list_price;
                        uom_mesuremnt.Text = prodresults.uom_name;
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
                        unit_price.Text = prodresults.list_price;
                        uom_mesuremnt.Text = prodresults.uom_name;
                        uom_id = prodresults.uom_id;
                        exp_prod_id = prodresults.id;
                    }

                }

                int i = 0;

            });


            //MessagingCenter.Subscribe<string, bool>("MyApp", "galleryMsg", async (sender, arg) =>
            //{

            //    AttachmentFile data;

            //    FileData fileData = new FileData();
            //    FileData filedata = null;
            //    try
            //    {
            //        data = new AttachmentFile();

            //        filedata = await CrossFilePicker.Current.PickFile();
            //        if (!string.IsNullOrEmpty(filedata.FileName))   //Just the file name, it doesn't has the path
            //        {
            //            byte[] bydata = filedata.DataArray;
            //            string fileName = filedata.FileName;
            //            string path = filedata.FilePath;
            //            string contents = System.Text.Encoding.UTF8.GetString(filedata.DataArray);
            //            string base64 = Convert.ToBase64String(bydata);

            //            var dat = filedata.GetStream();

            //            if (fileName.Contains(".doc"))
            //            {
            //                data.file_name = fileName;
            //                data.filepath = path;
            //                data.file_data = contents;
            //                data.filebase64 = base64;
            //                System.Diagnostics.Debug.WriteLine("File Name" + fileName);
            //                System.Diagnostics.Debug.WriteLine("File Date" + contents);
            //            }
            //            else if (fileName.Contains(".png") || fileName.Contains(".jpg"))
            //            {
            //                data.file_name = fileName;
            //                data.filepath = path;
            //                data.file_data = contents;
            //                data.filebase64 = base64;
            //                System.Diagnostics.Debug.WriteLine("File Name" + fileName);
            //                System.Diagnostics.Debug.WriteLine("File Date" + contents);
            //            }
            //            else if (fileName.Contains(".pdf"))
            //            {
            //                data.file_name = fileName;
            //                data.filepath = path;
            //                data.file_data = contents;
            //                data.filebase64 = base64;
            //                System.Diagnostics.Debug.WriteLine("File Name" + fileName);
            //                System.Diagnostics.Debug.WriteLine("File Date" + contents);
            //            }
            //            else if (fileName.Contains(".txt"))
            //            {
            //                data.file_name = fileName;
            //                data.filepath = path;
            //                data.file_data = contents;
            //                data.filebase64 = base64;
            //                System.Diagnostics.Debug.WriteLine("File Name" + fileName);
            //                System.Diagnostics.Debug.WriteLine("File Date" + contents);
            //            }
            //            attach.Add(data);
            //        }
            //        attach = attach.GroupBy(i => i.file_name).Select(g => g.First()).ToList();

            //        attachviewlist.HeightRequest = 45 * attach.Count;
            //        attachviewlist.IsVisible = true;
            //        attachviewlist.ItemsSource = null;
            //        attachviewlist.ItemsSource = attach;
            //        System.Diagnostics.Debug.WriteLine("fileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" + attach.Count);

            //    }
            //    catch (Exception ex)
            //    {
            //        filedata = null;
            //        System.Diagnostics.Debug.WriteLine("Warning Exception :  " + ex.Message);
            //    }

            //});

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

            //MessagingCenter.Subscribe<string, bool>("MyApp", "cameraMsg", async (sender, arg) =>
            //{
            //    AttachmentFile data;

            //    try
            //    {
            //        data = new AttachmentFile();

            //        var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            //        if (photo == null)
            //        {

            //        }

            //        if (photo != null)
            //        {
            //            byte[] bydata;
            //            using (MemoryStream ms = new MemoryStream())
            //            {
            //                photo.GetStream().CopyTo(ms);
            //                bydata = ms.ToArray();
            //            }
            //            string b64string = Convert.ToBase64String(bydata);
            //            var path = photo.Path;
            //            var filepath = photo.AlbumPath;
            //            string filename = path.Substring(path.LastIndexOf("/") + 1);

            //            data.file_name = filename;
            //            data.filepath = filepath;
            //            data.filebase64 = b64string;

            //            attach.Add(data);
            //        }

            //        //attach = attach.GroupBy(i => i.filename).Select(g => g.First()).ToList();

            //        attachviewlist.HeightRequest = 45 * attach.Count;
            //        attachviewlist.IsVisible = true;
            //        attachviewlist.ItemsSource = null;
            //        attachviewlist.ItemsSource = attach;
            //        System.Diagnostics.Debug.WriteLine("fileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" + attach.Count);

            //    }
            //    catch (Exception ex)
            //    {
            //        ex.Message.ToString();
            //    }


            //});



        }

        private void unitprice_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                total = Convert.ToDouble(unit_price.Text) * Convert.ToDouble(qty.Text);
                //  total =   Math.Round(total, 2);

                if (currency_picker.SelectedIndex == 0)
                {
                    total_value.Text = string.Format("{0:0.00}", total) + "" + "€";
                }
                else if(currency_picker.SelectedIndex == 1)
                {
                    total_value.Text = string.Format("{0:0.00}", total) + "" + "Rp";
                }
                else if (currency_picker.SelectedIndex == 2)
                {
                    total_value.Text = string.Format("{0:0.00}", total) + "" + "$";
                }

               // total_value.Text = total.ToString();
            }
            catch
            {
                int j = 0;
            }
        }

        private void qty_TextChanged(object sender, TextChangedEventArgs e)
        {
           


            try
            {
                total = Convert.ToDouble(unit_price.Text) * Convert.ToDouble(qty.Text);

                if (currency_picker.SelectedIndex == 0)
                {
                    total_value.Text = string.Format("{0:0.00}", total) + "" + "€";
                }
                else if (currency_picker.SelectedIndex == 1)
                {
                    total_value.Text = string.Format("{0:0.00}", total) + "" + "Rp";
                }
                else if (currency_picker.SelectedIndex == 2)
                {
                    total_value.Text = string.Format("{0:0.00}", total) + "" + "$";
                }
            }
            catch
            {
                int j = 0;
            }
        }

        void currency_indexChanged(object sender, System.EventArgs e)
        {
            if (currency_picker.SelectedIndex == 0)
            {
                total_value.Text = string.Format("{0:0.00}", total) + "" + "€";
            }
            else if (currency_picker.SelectedIndex == 1)
            {
                total_value.Text = string.Format("{0:0.00}", total) + "" + "Rp";
            }
            else if (currency_picker.SelectedIndex == 2)
            {
                total_value.Text = string.Format("{0:0.00}", total) + "" + "$";
            }
        }


        private async void createbtn_ClickedAsync(object sender, EventArgs e)
        {
            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

            int emp_id = (App.employee_list.AsEnumerable().Where(p => p.name == employee_picker.SelectedItem.ToString()).Select(p => p.id)).FirstOrDefault();
            int cur_id = (App.currencyList.AsEnumerable().Where(p => p.Name == currency_picker.SelectedItem.ToString()).Select(p => p.id)).FirstOrDefault();

            List<dynamic> attfile = new List<dynamic>();
            foreach (var data in attach)
            {
                Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

                dict.Add("file_name", data.file_name);
                dict.Add("file_data", data.file_data);
                attfile.Add(dict);
            }

            if (description.Text == "")
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
                if (App.NetAvailable == true)
                {

                    //var currentpage = new LoadingIndicator();
                    //await PopupNavigation.Instance.PushAsync(currentpage);
                    act_ind.IsRunning = true;

                    product_alert.IsVisible = false;
                    exdes_alert.IsVisible = false;

                    vals["name"] = description.Text;
                    vals["product_id"] = exp_prod_id;
                    vals["unit_amount"] = unit_price.Text;
                    vals["quantity"] = qty.Text;
                    vals["product_uom_id"] = uom_id;
                    vals["reference"] = bill_ref.Text;
                    vals["date"] = start_date.Date.ToString("yyyy-MM-dd");
                    vals["employee_id"] = emp_id;
                    vals["currency_id"] = cur_id;
                    vals["total_amount"] = total;
                    vals["attachment"] = attfile;
                    var created = Controller.InstanceCreation().CreateLeave("hr.expense", "app_create_expense", vals);

                    if (created == "True")
                    {
                        await Task.Run(() => App.Current.MainPage = new MasterPage(new ExpensePage()));                       
                        Loadingalertcall();
                        act_ind.IsRunning = false;
                        DependencyService.Get<Toast>().Show("Created Successfully...");
                    }
                    else
                    {
                      //  Loadingalertcall();
                        act_ind.IsRunning = false;
                        DependencyService.Get<Toast>().Show(created);
                    }
                }
              else
                {
                    act_ind.IsRunning = true;
                    var jso_attchment_list = Newtonsoft.Json.JsonConvert.SerializeObject(attfile);

                    DateTime dt = DateTime.ParseExact(start_date.Date.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
                    string convert_halfdate = dt.ToLocalTime().ToString("MMMM dd");
                
                    var sample = new ExpenseModelDB
                    {
                        name = description.Text,
                        product = searchprod.Text,
                        product_id = exp_prod_id,
                        unit_price = float.Parse(unit_price.Text),
                        uom_id = uom_id,
                        quantity = float.Parse(qty.Text),
                        reference = bill_ref.Text,
                        sync_string = "sync.png",
                        stage_colour = "#008FD3",
                        stage_name = "To Submit",
                        state = "draft",
                        attachment = jso_attchment_list,
                        date = start_date.Date.ToString("yyyy-MM-dd"),
                        half_date = convert_halfdate,
                        employee_id = employee_picker.SelectedItem.ToString(),
                        emp_id = emp_id,
                        currency = currency_picker.SelectedItem.ToString(),
                        currency_id = cur_id,
                        total = total,
                        check_rpc_condition = "true",
                       // error_attachment_list = attach_list,
                    };
                    App._connection.Insert(sample);

                    App._connection.CreateTable<ExpenseModelDB>();
                    try
                    {
                        var details = (from y in App._connection.Table<ExpenseModelDB>() select y).ToList();
                        App.expense_listDb = details;
                    }
                    catch (Exception ex)
                    {
                        int i = 0;
                    }
                    await Task.Run(() => App.Current.MainPage = new MasterPage(new ExpensePage()));          
                    DependencyService.Get<Toast>().Show("Created successfully need to sync with server...");
                    act_ind.IsRunning = false;
                    Loadingalertcall();
                }

            }

          //  PopupNavigation.Instance.PopAsync();
        }


        async void Loadingalertcall()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }


        private void delete_Tapped(object sender, EventArgs e)
        {
            try
            {
                var args = (TappedEventArgs)e;

                AttachmentFile t2 = args.Parameter as AttachmentFile;

                var itemToRemove = attach.Single(r => r.file_name == t2.file_name);

                attach.Remove(itemToRemove);

                attachviewlist.ItemsSource = null;
                attachviewlist.ItemsSource = attach;
                attachviewlist.HeightRequest = 50 * attach.Count;

                if (attach.Count == 0)
                {
                    attachviewlist.IsVisible = false;
                }

            }
            catch (Exception ex)
            {

            }

        }

    }
}

