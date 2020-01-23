using HRMAPP.DBModel;
using HRMAPP.Model;
using HRMAPP.Views;
using Newtonsoft.Json.Linq;
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
	public partial class LeavesCreation : PopupPage
    {
        List<AttachmentFile> attach = new List<AttachmentFile>();
        Boolean half_day = false;
        public LeavesCreation ()
		{
			InitializeComponent ();


            start_date.Format = "dd/MM/yyyy";
            stop_date.Format = "dd/MM/yyyy";

            var closeRecognizer = new TapGestureRecognizer();
            closeRecognizer.Tapped += (s, e) =>
            {
                PopupNavigation.Instance.PopAllAsync();
                MessagingCenter.Send<string, string>("MyApp", "CreateMsg", "true");
            };
            overall_close.GestureRecognizers.Add(closeRecognizer);

        
            if(App.NetAvailable == true)
            {
                leaveType_picker.ItemsSource = App.leaveTypeList.Select(n => n.Name2).ToList();
                leaveType_picker.SelectedIndex = -1;

                hr_year_picker.ItemsSource = App.hr_yearList.Select(y => y.Name).ToList();
                hr_year_picker.SelectedIndex = 0;
            }
            else
            {
                leaveType_picker.ItemsSource = App.leaveTypeListDb.Select(n => n.Name2).ToList();
                leaveType_picker.SelectedIndex = -1;

                hr_year_picker.ItemsSource = App.hr_yearListDb.Select(y => y.Name).ToList();
                hr_year_picker.SelectedIndex = 0;               
            }
            
           
            var halfday = new List<string>();
            halfday.Add("AM");
            halfday.Add("PM");
            half_dayPicker.ItemsSource = halfday;
            half_dayPicker.SelectedIndex = 0;

            var uncheck_Img = new TapGestureRecognizer();
            uncheck_Img.Tapped += (s, e) =>
            {
                uncheckImg.IsVisible = false;
                checkImg.IsVisible = true;
                half_dayPicker.IsVisible = true;
                drdownImg.IsVisible = true;
                enddate_layout.IsVisible = false;
                half_day = true;
            };
            uncheckImg.GestureRecognizers.Add(uncheck_Img);

            var check_Img = new TapGestureRecognizer();
            check_Img.Tapped += (s, e) =>
            {
                checkImg.IsVisible = false;
                uncheckImg.IsVisible = true;
                half_dayPicker.IsVisible = false;
                drdownImg.IsVisible = false;
                enddate_layout.IsVisible = true;
                half_day = false;
            };
            checkImg.GestureRecognizers.Add(check_Img);

            var attach_file = new TapGestureRecognizer();
            attach_file.Tapped += async (s, e) =>
            {

                await PopupNavigation.Instance.PushAsync(new FileAttachmentPage("update", attach));

              //  await PopupNavigation.Instance.PushAsync(new FileAttachmentPage());

                //AttachmentFile data;

                //FileData fileData = new FileData();
                //FileData filedata = null;
                //try
                //{
                //    data = new AttachmentFile();

                //    filedata = await CrossFilePicker.Current.PickFile();
                //    if (!string.IsNullOrEmpty(filedata.FileName))   //Just the file name, it doesn't has the path
                //    {
                //        byte[] bydata = filedata.DataArray;
                //        string fileName = filedata.FileName;
                //        string path = filedata.FilePath;
                //        string contents = System.Text.Encoding.UTF8.GetString(filedata.DataArray);
                //        string base64 = Convert.ToBase64String(bydata);

                //        var dat = filedata.GetStream();

                //        if (fileName.Contains(".doc"))
                //        {
                //            data.filename = fileName;
                //            data.filepath = path;
                //            data.filedata = contents;
                //            data.filebase64 = base64;
                //            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                //            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                //        }
                //        else if (fileName.Contains(".png") || fileName.Contains(".jpg"))
                //        {
                //            data.filename = fileName;
                //            data.filepath = path;
                //            data.filedata = contents;
                //            data.filebase64 = base64;
                //            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                //            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                //        }
                //        else if( fileName.Contains(".pdf"))
                //        {
                //            data.filename = fileName;
                //            data.filepath = path;
                //            data.filedata = contents;
                //            data.filebase64 = base64;
                //            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                //            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                //        }
                //        else if(fileName.Contains(".txt"))
                //        {
                //            data.filename = fileName;
                //            data.filepath = path;
                //            data.filedata = contents;
                //            data.filebase64 = base64;
                //            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                //            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                //        }
                //        attach.Add(data);
                //    }
                //    attach = attach.GroupBy(i => i.filename).Select(g => g.First()).ToList();

                //    attachviewlist.HeightRequest = 45 * attach.Count;
                //    attachviewlist.IsVisible = true;
                //    attachviewlist.ItemsSource = null;
                //    attachviewlist.ItemsSource = attach;
                //    System.Diagnostics.Debug.WriteLine("fileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" + attach.Count);

                //}
                //catch (Exception ex)
                //{
                //    filedata = null;
                //    System.Diagnostics.Debug.WriteLine("Warning Exception :  " + ex.Message);
                //}

            };
            add_attachment.GestureRecognizers.Add(attach_file);

        }

        protected override bool OnBackButtonPressed()
        {
            PopupNavigation.Instance.PopAllAsync();
            MessagingCenter.Send<string, string>("MyApp", "CreateMsg", "true");
            return true;
        }

        private void discard_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync();
            MessagingCenter.Send<string, string>("MyApp", "CreateMsg", "true");
        }

        private void LeaveType_picker_SelectedIndexChanged(object sender, EventArgs e)
        {            
            halfday_layout.IsVisible = true;   
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

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#F0EEEF");
        }

        private async void create_Clicked(object sender, EventArgs e)
        {

            //var currentpage = new LoadingIndicator();
            //await PopupNavigation.Instance.PushAsync(currentpage);

            act_ind.IsRunning = true;

            try
            {
             //   List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();
                JObject attdetails = await Task.Run(() => Controller.InstanceCreation().GetAttendanceData());

            }
            catch
            {
                int jk = 0;
            }

            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
           
            string convertstartdate = start_date.Date.ToString("yyyy-MM-dd");
            var dt = start_time.Time.ToString();
            convertstartdate = convertstartdate + " " + dt.ToString();
            string convertstart_datetime = convertstartdate;
            DateTime startDate = DateTime.ParseExact(convertstart_datetime, "yyyy-MM-dd HH:mm:ss", null);
            convertstart_datetime = startDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

            string convertstopdate = stop_date.Date.ToString("yyyy-MM-dd");
            var dt1 = stop_time.Time.ToString();
            convertstopdate = convertstopdate + " " + dt1.ToString();
            string convertstop_datetime = convertstopdate;
            DateTime stopDate = DateTime.ParseExact(convertstop_datetime, "yyyy-MM-dd HH:mm:ss", null);
            convertstop_datetime = stopDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");

            TimeSpan diff;
            string dur = "";
            diff = stopDate - startDate;
            dur = diff.Days.ToString();

            
            List<dynamic> attfile = new List<dynamic>();
            foreach (var data in attach)
            {
                Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

                dict.Add("file_name", data.file_name);
                dict.Add("file_data", data.filebase64);
                attfile.Add(dict);
            }

            string am_or_pm = "";
            if (half_day == true)
            {
                am_or_pm = half_dayPicker.SelectedItem.ToString();
                var stopdate = startDate.AddHours(4);
                var loctime = stopdate.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                convertstop_datetime = stopdate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                diff = stopdate - startDate;
                dur = diff.Days.ToString();
            }
            else
            {
                am_or_pm = "";
            }

            if (App.NetAvailable == true)
            {

                if (leaveType_picker.SelectedItem == null)
                {
                    leavepick_alert.IsVisible = true;
                    act_ind.IsRunning = false;
                }
                else
                {

                    var leave_type_id = App.leaveTypeList.FirstOrDefault(x => x.Name2 == leaveType_picker.SelectedItem.ToString()).Id;
                    var hr_year_id = App.hr_yearList.FirstOrDefault(x => x.Name == hr_year_picker.SelectedItem.ToString()).Id;
                    var employee_id = App.employee_id;

                    leavepick_alert.IsVisible = false;

                    vals["name"] = reason.Text;
                    vals["holiday_status_id"] = leave_type_id;
                    vals["hr_year_id"] = hr_year_id;
                    vals["employee_id"] = employee_id;
                    vals["date_from"] = convertstart_datetime;
                    vals["date_to"] = convertstop_datetime;
                    vals["half_day_related"] = half_day;
                    vals["half_day"] = half_day;
                    vals["am_or_pm"] = am_or_pm;
                    vals["attachment"] = attfile;
                    vals["number_of_days_temp"] = dur;

                    var dic = vals;

                    // await Task.Run(() => App.Current.MainPage = new MasterPage(new AttendancesPage()));

                    var created = await Task.Run(() => Controller.InstanceCreation().CreateLeave("hr.holidays", "app_create_leave", vals));

                    if (created == "True")
                    {
                        await Task.Run(() => App.Current.MainPage = new MasterPage(new LeavesPage()));
                        Loadingalertcall();
                        DependencyService.Get<Toast>().Show("Created Successfully...");
                        act_ind.IsRunning = false;
                    }
                    else
                    {
                        Loadingalertcall();
                        DependencyService.Get<Toast>().Show(created);
                        act_ind.IsRunning = false;
                    }
                }
            }
            else
            {

                string reasontxt = reason.Text;
                string leavetype = leaveType_picker.SelectedItem.ToString();
                string year = hr_year_picker.SelectedItem.ToString();

                DateTime fromdt = DateTime.ParseExact(convertstart_datetime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                string datefrom = fromdt.ToLocalTime().ToString("MMM dd");

                DateTime todt = DateTime.ParseExact(convertstop_datetime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                string dateto = todt.ToLocalTime().ToString("MMM dd");

                DateTime fullstartdt = DateTime.ParseExact(convertstart_datetime, "yyyy-MM-dd HH:mm:ss", null);
                string startdate = fullstartdt.ToLocalTime().ToString("dd/MM/yyyy HH:mm");

                DateTime fullstopdt = DateTime.ParseExact(convertstop_datetime, "yyyy-MM-dd HH:mm:ss", null);
                string enddate = fullstopdt.ToLocalTime().ToString("dd/MM/yyyy HH:mm");

                var leave_id = App.leaveTypeListDb.FirstOrDefault(x => x.Name2 == leaveType_picker.SelectedItem.ToString()).Id;
                var year_id = App.hr_yearListDb.FirstOrDefault(x => x.Name == hr_year_picker.SelectedItem.ToString()).Id;
                var emp_id = App.employee_idDb;

                var jso_attchment_list = Newtonsoft.Json.JsonConvert.SerializeObject(attfile);

                var attach_list = Newtonsoft.Json.JsonConvert.SerializeObject(attach);

                var sample = new LeavesModelDB
                {
                    name = reasontxt,
                    holidays_status = leavetype,
                    year = year,
                    datefrom = datefrom,
                    dateto = dateto,
                    fromDate = startdate,
                    endDate = enddate,
                    date_from = convertstart_datetime,
                    date_to = convertstop_datetime,
                    leave_type_id = leave_id,
                    employee_id = emp_id,
                    year_id = year_id,
                    sync_string = "sync.png",
                    stage_colour = "#efb139",
                    state = "confirm",
                    state_name = "Waiting Pre-Approval",
                    attachment_list = jso_attchment_list,
                    no_of_days = dur,
                    is_half_day = half_day,
                    check_tt = am_or_pm,
                    employee = "Administrator",
                    check_rpc_condition = "true",
                    error_attachment_list = attach_list,
                };
                App._connection.Insert(sample);

                App._connection.CreateTable<LeavesModelDB>();
                try
                {
                    var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();
                    App.leaves_listDb = details;
                }
                catch (Exception ex)
                {
                    int i = 0;
                }

                await Task.Run(() => App.Current.MainPage = new MasterPage(new LeavesPage()));   

                DependencyService.Get<Toast>().Show("Created successfully need to sync with server...");

                act_ind.IsRunning = false;

             //   Loadingalertcall();

            }
        }

        async void Loadingalertcall()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string, bool>("MyApp", "galleryMsg", async (sender, arg)  =>
            {

                AttachmentFile data;

                FileData fileData = new FileData();
                FileData filedata = null;
                try
                {
                    data = new AttachmentFile();

                    filedata = await CrossFilePicker.Current.PickFile();
                    if (!string.IsNullOrEmpty(filedata.FileName))   //Just the file name, it doesn't has the path
                    {
                        byte[] bydata = filedata.DataArray;
                        string fileName = filedata.FileName;
                        string path = filedata.FilePath;
                        string contents = System.Text.Encoding.UTF8.GetString(filedata.DataArray);
                        string base64 = Convert.ToBase64String(bydata);

                        var dat = filedata.GetStream();

                        if (fileName.Contains(".doc"))
                        {
                            data.file_name = fileName;
                            data.filepath = path;
                            data.file_data = contents;
                            data.filebase64 = base64;
                            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                        }
                        else if (fileName.Contains(".png") || fileName.Contains(".jpg"))
                        {
                            data.file_name = fileName;
                            data.filepath = path;
                            data.file_data = contents;
                            data.filebase64 = base64;
                            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                        }
                        else if (fileName.Contains(".pdf"))
                        {
                            data.file_name = fileName;
                            data.filepath = path;
                            data.file_data = contents;
                            data.filebase64 = base64;
                            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                        }
                        else if (fileName.Contains(".txt"))
                        {
                            data.file_name = fileName;
                            data.filepath = path;
                            data.file_data = contents;
                            data.filebase64 = base64;
                            System.Diagnostics.Debug.WriteLine("File Name" + fileName);
                            System.Diagnostics.Debug.WriteLine("File Date" + contents);
                        }
                        attach.Add(data);
                    }
                    attach = attach.GroupBy(i => i.file_name).Select(g => g.First()).ToList();

                    attachviewlist.HeightRequest = 45 * attach.Count;
                    attachviewlist.IsVisible = true;
                    attachviewlist.ItemsSource = null;
                    attachviewlist.ItemsSource = attach;
                    System.Diagnostics.Debug.WriteLine("fileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" + attach.Count);

                }
                catch (Exception ex)
                {
                    filedata = null;
                    System.Diagnostics.Debug.WriteLine("Warning Exception :  " + ex.Message);
                }

            });

            MessagingCenter.Subscribe<string, bool>("MyApp", "cameraMsg", async (sender, arg) =>
            {
                AttachmentFile data;

                try
                {
                    data = new AttachmentFile();

                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

                    if (photo == null)
                    {

                    }

                    if (photo != null)
                    {
                        byte[] bydata;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            photo.GetStream().CopyTo(ms);
                            bydata = ms.ToArray();
                        }
                        string b64string = Convert.ToBase64String(bydata);
                        var path = photo.Path;
                        var filepath = photo.AlbumPath;
                        string filename = path.Substring(path.LastIndexOf("/") + 1);

                        data.file_name = filename;
                        data.filepath = filepath;
                        data.filebase64 = b64string;

                        attach.Add(data);
                    }

                    //attach = attach.GroupBy(i => i.filename).Select(g => g.First()).ToList();

                    attachviewlist.HeightRequest = 45 * attach.Count;
                    attachviewlist.IsVisible = true;
                    attachviewlist.ItemsSource = null;
                    attachviewlist.ItemsSource = attach;
                    System.Diagnostics.Debug.WriteLine("fileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee" + attach.Count);

                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }

                
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


    }
}