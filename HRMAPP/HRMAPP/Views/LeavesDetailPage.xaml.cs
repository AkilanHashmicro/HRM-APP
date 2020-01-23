
using HRMAPP.DBModel;
using HRMAPP.Model;
using HRMAPP.Pages;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
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

namespace HRMAPP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeavesDetailPage : ContentPage
    {
        LeavesModel levobj = new LeavesModel();
        List<string> halfday = new List<string>();

        LeavesModelDB levobjDb = new LeavesModelDB();
        Boolean check_half_day = false;

        List<AttachmentFile> attach = new List<AttachmentFile>();
        List<AttachmentFile> attachnew = new List<AttachmentFile>();

        List<int> delete_attach = new List<int>();

        bool leave_officer = false;
        bool leave_manager = false;

        //bool updated = Controller.InstanceCreation().submittomanager("hr.expense", "app_approve_expense_sheets", expobj.id);

        public LeavesDetailPage (LeavesModel obj)
        {
            InitializeComponent ();

            attach.Clear();

            start_date.Format = "dd/MM/yyyy";
            stop_date.Format = "dd/MM/yyyy";

            levobj = obj;

            leave_officer = App.access_dict.FirstOrDefault(x => x.Key == "holiday_officer").Value;

            leave_manager = App.access_dict.FirstOrDefault(x => x.Key == "holiday_manager").Value;

            //if (!leave_officer || !leave_manager)
            //{
            //    btnApprove.IsVisible = false;
            //    btnConfirm.IsVisible = false;
            //    btnRefuse.IsVisible = false;

            //}
           

            reasonlabel.Text = obj.name;
            leavetypelabel.Text = obj.holidays_status;
            hryearlabel.Text = obj.year;

            if(obj.check_rpc_condition != "false")
            {
                label_display_Name.Text = obj.display_name;
            }
            else
            {
                delete.IsVisible = true;
                label_display_Name.Text = obj.error_rpcTxt;
                label_display_Name.TextColor = Color.Red;
                durlayout.IsVisible = false;
                dur_boxlayout.IsVisible = false;
                state_layout.IsVisible = false;
                state_boxlayout.IsVisible = false;
                leavestructure_layout.IsVisible = false;
                leavestructure_boxlayout.IsVisible = false;
                department_layout.IsVisible = false;
                department_boxlayout.IsVisible = false;
                btnStack.IsVisible = false;

                //  attach = JsonConvert.DeserializeObject<List<AttachmentFile>>(obj.attachment);
                attach = obj.attachment;
                if(attach.Count > 0)
                {
                    attachviewlist.IsVisible = true;
                    attachviewlist.HeightRequest = 45 * attach.Count;
                    attachviewlist.ItemsSource = attach;
                }
            }

            attach = obj.attachment;
            if (attach.Count > 0)
            {
                attachviewlist.IsVisible = true;
                attachviewlist.HeightRequest = 45 * attach.Count;
                attachviewlist.ItemsSource = attach;
            }

            leave_pick.ItemsSource = App.leaveTypeList.Select(n => n.Name2).ToList();
            hr_year_picker.ItemsSource = App.hr_yearList.Select(y => y.Name).ToList();

            //employee_namelabel.Text = obj.employee;
            leave_structure_Namelabel.Text = obj.leave_structure;
            dept_namelabel.Text = obj.department;

            startdate_label.Text = obj.fromDate;
            enddate_label.Text = obj.endDate;
            duration_label.Text = obj.no_of_days.ToString();

            if (obj.is_half_day == false)
            {
                checkImg.IsVisible = false;
                uncheckImg.IsVisible = true;
                tt_label.Text = obj.check_tt;
                check_half_day = false;
                overall_enddate_layout.IsVisible = true;
            }
            else
            {
                checkImg.IsVisible = true;
                uncheckImg.IsVisible = false;
                tt_label.Text = obj.check_tt;
                check_half_day = true;
                overall_enddate_layout.IsVisible = false;
            }

            if (obj.state.Equals("confirm"))
            {
                btnApprove.IsVisible = true;
                btnRefuse.IsVisible = true;
                btnDraft.IsVisible = true;
                btnConfirm.IsVisible = false;
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.state_name;
            }
            else if(obj.state.Equals("draft"))
            {
                btnConfirm.IsVisible = true;
                btnApprove.IsVisible = false;
                btnRefuse.IsVisible = false;
                btnDraft.IsVisible = false;
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.state_name;
            }
            else if(obj.state.Equals("refuse"))
            {
                btnConfirm.IsVisible = false;
                btnApprove.IsVisible = false;
                btnRefuse.IsVisible = false;
                btnDraft.IsVisible = true;
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.state_name;
            }
            else if(obj.state.Equals("validate"))
            {
                btnConfirm.IsVisible = false;
                btnApprove.IsVisible = false;
                btnRefuse.IsVisible = true;
                btnDraft.IsVisible = false;
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.state_name;
            }


            if (!leave_officer || !leave_manager)
            {
                if (obj.state.Equals("confirm"))
                {
                    btnApprove.IsVisible = false;
                    btnRefuse.IsVisible = false;
                    btnDraft.IsVisible = true;
                    btnConfirm.IsVisible = false;
                    frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                    statelabel.Text = obj.state_name;
                }
                else if (obj.state.Equals("draft"))
                {
                    btnConfirm.IsVisible = true;
                    btnApprove.IsVisible = false;
                    btnRefuse.IsVisible = false;
                    btnDraft.IsVisible = false;
                    frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                    statelabel.Text = obj.state_name;
                }
            }

            
            halfday.Add("AM");
            halfday.Add("PM");
            half_dayPicker.ItemsSource = halfday;
            half_dayPicker.SelectedIndex = 0;
                  

            var deleteRecognizer = new TapGestureRecognizer();
            deleteRecognizer.Tapped  += async (s, e) =>
            {
                string title = "Alert";
                string txt = "Are you sure you want to delete this record?";
                await PopupNavigation.Instance.PushAsync(new CustomAlert(txt, title));
            };
            delete.GestureRecognizers.Add(deleteRecognizer);

            var editRecognizer = new TapGestureRecognizer();
            editRecognizer.Tapped += (s, e) =>
            {
                edit.IsVisible = false;
                add_attachment.IsEnabled = true;
                attachviewlist.IsEnabled = true;
                if (obj.check_rpc_condition != "false")
                {
                    updatebtn.IsVisible = true;
                }
                else
                {
                    createbtn.IsVisible = true;
                    durlayout.IsVisible = false;
                    dur_boxlayout.IsVisible = false;
                    delete.IsVisible = false;
                    state_layout.IsVisible = false;
                    state_boxlayout.IsVisible = false;
                    leavestructure_layout.IsVisible = false;
                    leavestructure_boxlayout.IsVisible = false;
                    department_layout.IsVisible = false;
                    department_boxlayout.IsVisible = false;
                    btnStack.IsVisible = false;
                }

                leave_pick.ItemsSource = App.leaveTypeList.Select(n => n.Name2).ToList();
                leave_pick.SelectedIndex = 0;
                leave_pick.SelectedItem = obj.holidays_status;

                hr_year_picker.ItemsSource = App.hr_yearList.Select(y => y.Name).ToList();
                hr_year_picker.SelectedIndex = 0;
                hr_year_picker.SelectedItem = obj.year;

                reasonlabel.IsVisible = false;
                reason_entry.IsVisible = true;
                reason_entry.Text = obj.name;

                leavetypelabel.IsVisible = false;
                leavetype_layout.IsVisible = true;

                //employee_namelabel.IsVisible = false;
                //employee_layout.IsVisible = true;

                hryearlabel.IsVisible = false;
                hryear_layout.IsVisible = true;

                startdate_label.IsVisible = false;
                startdate_layout.IsVisible = true;

                enddate_label.IsVisible = false;
                enddate_layout.IsVisible = true;

                // half day field
                uncheckImg.IsVisible = false;
                checkImg.IsVisible = false;
                tt_label.IsVisible = false;
                if (obj.is_half_day == false)
                {
                    editcheckImg.IsVisible = false;
                    edituncheckImg.IsVisible = true;
                    half_dayPicker.IsVisible = false;
                    drdownImg.IsVisible = false;
                    overall_enddate_layout.IsVisible = true;
                }
                else
                {
                    editcheckImg.IsVisible = true;
                    edituncheckImg.IsVisible = false;
                    half_dayPicker.IsVisible = true;
                    half_dayPicker.SelectedItem = obj.check_tt;
                    drdownImg.IsVisible = true;
                    overall_enddate_layout.IsVisible = false;
                }
                halfday_layout.IsVisible = true;

                //start date field
                DateTime start_dateTime = DateTime.ParseExact(obj.date_from, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToLocalTime();
                string start_datestring = start_dateTime.ToString("yyyy-MM-dd");
                string start_timestring = start_dateTime.ToString("HH:mm");
                string start_fullTime = start_datestring + " " + start_timestring;
                TimeSpan start_timespan = TimeSpan.Parse(start_timestring);
                start_date.Date = start_dateTime;
                start_time.Time = start_timespan;

                //stop date field
                DateTime stop_dateTime = DateTime.ParseExact(obj.date_to, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToLocalTime();
                string stop_datestring = stop_dateTime.ToString("yyyy-MM-dd");
                string stop_timestring = stop_dateTime.ToString("HH:mm");
                string stop_fullTime = stop_datestring + " " + stop_timestring;
                TimeSpan stop_timespan = TimeSpan.Parse(stop_timestring);                
                stop_date.Date = stop_dateTime;
                stop_time.Time = stop_timespan;

            };
            edit.GestureRecognizers.Add(editRecognizer);

            //// check box
            var uncheck_Img = new TapGestureRecognizer();
            uncheck_Img.Tapped += (s, e) =>
            {
                edituncheckImg.IsVisible = false;
                editcheckImg.IsVisible = true;
                half_dayPicker.IsVisible = true;
                drdownImg.IsVisible = true;
                check_half_day = true;
                overall_enddate_layout.IsVisible = false;
            };
            edituncheckImg.GestureRecognizers.Add(uncheck_Img);

            var check_Img = new TapGestureRecognizer();
            check_Img.Tapped += (s, e) =>
            {
                editcheckImg.IsVisible = false;
                edituncheckImg.IsVisible = true;
                half_dayPicker.IsVisible = false;
                drdownImg.IsVisible = false;
                check_half_day = false;
                overall_enddate_layout.IsVisible = true;
            };
            editcheckImg.GestureRecognizers.Add(check_Img);

            ///////Attachment
            var attach_file = new TapGestureRecognizer();
            attach_file.Tapped += async (s, e) =>
            {

                System.Diagnostics.Debug.WriteLine( "OUTT" + attach.Count());
               

                await PopupNavigation.Instance.PushAsync(new FileAttachmentPage("update",attach));

            };
            add_attachment.GestureRecognizers.Add(attach_file);

        }

        public LeavesDetailPage(LeavesModelDB obj)
        {
            InitializeComponent();

            levobjDb = obj;

            label_display_Name.Text = obj.display_name;
            reasonlabel.Text = obj.name;
            leavetypelabel.Text = obj.holidays_status;
            hryearlabel.Text = obj.year;
            leave_structure_Namelabel.Text = obj.leave_structure;
            dept_namelabel.Text = obj.department;

            startdate_label.Text = obj.fromDate;
            enddate_label.Text = obj.endDate;
            duration_label.Text = obj.no_of_days.ToString();

            if (obj.is_half_day == false)
            {
                check_half_day = false;
                checkImg.IsVisible = false;
                uncheckImg.IsVisible = true;
                tt_label.Text = obj.check_tt;
                overall_enddate_layout.IsVisible = true;
            }
            else
            {
                check_half_day = true;
                checkImg.IsVisible = true;
                uncheckImg.IsVisible = false;
                tt_label.Text = obj.check_tt;
                overall_enddate_layout.IsVisible = false;
            }

            if (obj.state.Equals("confirm"))
            {
                btnApprove.IsVisible = true;
                btnRefuse.IsVisible = true;
                btnDraft.IsVisible = true;
                btnConfirm.IsVisible = false;
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.state_name;
            }
            else if (obj.state.Equals("draft"))
            {
                btnConfirm.IsVisible = true;
                btnApprove.IsVisible = false;
                btnRefuse.IsVisible = false;
                btnDraft.IsVisible = false;
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.state_name;
            }
            else if (obj.state.Equals("refuse"))
            {
                btnConfirm.IsVisible = false;
                btnApprove.IsVisible = false;
                btnRefuse.IsVisible = false;
                btnDraft.IsVisible = true;
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.state_name;
            }
            else if (obj.state.Equals("validate"))
            {
                btnConfirm.IsVisible = false;
                btnApprove.IsVisible = false;
                btnRefuse.IsVisible = true;
                btnDraft.IsVisible = false;
                frame_state_color.BackgroundColor = Color.FromHex(obj.stage_colour);
                statelabel.Text = obj.state_name;
            }

           // List<AttachmentFile> addattachlistDb = JsonConvert.DeserializeObject<List<AttachmentFile>>(dbobj.attachment_list);
            attach = JsonConvert.DeserializeObject<List<AttachmentFile>>(obj.attachment_list);
           // attach = obj.attachment_list;
            if (attach.Count > 0)
            {
                attachviewlist.IsVisible = true;
                attachviewlist.HeightRequest = 45 * attach.Count;
                attachviewlist.ItemsSource = attach;
            }


            halfday.Add("AM");
            halfday.Add("PM");
            half_dayPicker.ItemsSource = halfday;
            //half_dayPicker.SelectedIndex = 0;


            var editRecognizer = new TapGestureRecognizer();
            editRecognizer.Tapped += (s, e) =>
            {
                edit.IsVisible = false;
                add_attachment.IsEnabled = true;
                attachviewlist.IsEnabled = true;
                if (obj.check_rpc_condition != "false")
                {
                    updatebtn.IsVisible = true;
                }
                else
                {
                    createbtn.IsVisible = true;
                    durlayout.IsVisible = false;
                    dur_boxlayout.IsVisible = false;
                    delete.IsVisible = false;
                    state_layout.IsVisible = false;
                    state_boxlayout.IsVisible = false;
                    leavestructure_layout.IsVisible = false;
                    leavestructure_boxlayout.IsVisible = false;
                    department_layout.IsVisible = false;
                    department_boxlayout.IsVisible = false;
                    btnStack.IsVisible = false;
                }

                leave_pick.ItemsSource = App.leaveTypeListDb.Select(n => n.Name2).ToList();
                leave_pick.SelectedIndex = 0;
                leave_pick.SelectedItem = levobjDb.holidays_status;

                hr_year_picker.ItemsSource = App.hr_yearListDb.Select(y => y.Name).ToList();
                hr_year_picker.SelectedIndex = 0;
                hr_year_picker.SelectedItem = levobjDb.year;

                reasonlabel.IsVisible = false;
                reason_entry.IsVisible = true;
                reason_entry.Text = levobjDb.name;

                leavetypelabel.IsVisible = false;
                leavetype_layout.IsVisible = true;

                //employee_namelabel.IsVisible = false;
                //employee_layout.IsVisible = true;

                hryearlabel.IsVisible = false;
                hryear_layout.IsVisible = true;

                startdate_label.IsVisible = false;
                startdate_layout.IsVisible = true;

                enddate_label.IsVisible = false;
                enddate_layout.IsVisible = true;

                // half day field
                uncheckImg.IsVisible = false;
                checkImg.IsVisible = false;
                tt_label.IsVisible = false;
                if (obj.is_half_day == false)
                {
                    editcheckImg.IsVisible = false;
                    edituncheckImg.IsVisible = true;
                    half_dayPicker.IsVisible = false;
                    drdownImg.IsVisible = false;
                    overall_enddate_layout.IsVisible = true;
                }
                else
                {
                    editcheckImg.IsVisible = true;
                    edituncheckImg.IsVisible = false;
                    half_dayPicker.IsVisible = true;
                    half_dayPicker.SelectedItem = levobjDb.check_tt;
                    drdownImg.IsVisible = true;
                    overall_enddate_layout.IsVisible = false;
                }

                if (levobjDb.is_half_day)
                {

                    halfday_layout.IsVisible = true;
                }

                else
                {
                    halfday_layout.IsVisible = false;
                }

                //start date field
                DateTime start_dateTime = DateTime.ParseExact(obj.date_from, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToLocalTime();
                string start_datestring = start_dateTime.ToString("yyyy-MM-dd");
                string start_timestring = start_dateTime.ToString("HH:mm");
                string start_fullTime = start_datestring + " " + start_timestring;
                TimeSpan start_timespan = TimeSpan.Parse(start_timestring);
                start_date.Date = start_dateTime;
                start_time.Time = start_timespan;

                //stop date field
                DateTime stop_dateTime = DateTime.ParseExact(obj.date_to, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToLocalTime();
                string stop_datestring = stop_dateTime.ToString("yyyy-MM-dd");
                string stop_timestring = stop_dateTime.ToString("HH:mm");
                string stop_fullTime = stop_datestring + " " + stop_timestring;
                TimeSpan stop_timespan = TimeSpan.Parse(stop_timestring);
                stop_date.Date = stop_dateTime;
                stop_time.Time = stop_timespan;


                //DependencyService.Get<Toast>().Show("Need internet connection...");

            };
            edit.GestureRecognizers.Add(editRecognizer);


            var attach_file = new TapGestureRecognizer();
            attach_file.Tapped += async (s, e) =>
            {

                System.Diagnostics.Debug.WriteLine("OUTT" + attach.Count());
                await PopupNavigation.Instance.PushAsync(new FileAttachmentPage("update", attach));

            };
            add_attachment.GestureRecognizers.Add(attach_file);

            //var uncheck_Img = new TapGestureRecognizer();
            //uncheck_Img.Tapped += (s, e) =>
            //{
            //    edituncheckImg.IsVisible = false;
            //    editcheckImg.IsVisible = true;
            //    half_dayPicker.IsVisible = true;
            //    drdownImg.IsVisible = true;
            //};
            //edituncheckImg.GestureRecognizers.Add(uncheck_Img);

            //var check_Img = new TapGestureRecognizer();
            //check_Img.Tapped += (s, e) =>
            //{
            //    editcheckImg.IsVisible = false;
            //    edituncheckImg.IsVisible = true;
            //    half_dayPicker.IsVisible = false;
            //    drdownImg.IsVisible = false;
            //};
            //editcheckImg.GestureRecognizers.Add(check_Img);


        }

       
        private void updatecancel_clickedAsync(object sender, EventArgs e)
        {
            edit.IsVisible = true;
            updatebtn.IsVisible = false;
            attachviewlist.IsEnabled = false;

            add_attachment.IsEnabled = false;

            reasonlabel.IsVisible = true;
            reason_entry.IsVisible = false;

            leavetypelabel.IsVisible = true;
            leavetype_layout.IsVisible = false;

            //employee_namelabel.IsVisible = true;
            //employee_layout.IsVisible = false;

            hryearlabel.IsVisible = true;
            hryear_layout.IsVisible = false;

            startdate_label.IsVisible = true;
            startdate_layout.IsVisible = false;

            enddate_label.IsVisible = true;
            enddate_layout.IsVisible = false;

            
            //Half day field
            tt_label.IsVisible = true;
            if (levobj.is_half_day == false)
            {
                checkImg.IsVisible = false;
                uncheckImg.IsVisible = true;
                tt_label.Text = levobj.check_tt;
                overall_enddate_layout.IsVisible = true;
            }
            else
            {
                checkImg.IsVisible = true;
                uncheckImg.IsVisible = false;
                tt_label.Text = levobj.check_tt;
                overall_enddate_layout.IsVisible = false;
            }
            halfday_layout.IsVisible = false;

        }


        protected override void OnAppearing()
        {
            try
            {
                if (attachnew.Count == 0)
                {
                    attachnew = levobj.attachment;
                }
            }
            catch
            {
                int j = 0; 
            }

            base.OnAppearing();
            MessagingCenter.Subscribe<string, bool>("MyApp", "deleteMsg", async (sender, arg) =>
             {
                 var currentpage = new LoadingIndicator();
                 await PopupNavigation.Instance.PushAsync(currentpage);

                 int db_id = levobj.dbid_error_txt;

                 App._connection.Query<LeavesModelDB>("DELETE FROM LeavesModelDB WHERE Dbid=?", db_id);

                 App._connection.CreateTable<LeavesModelDB>();
                 var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();
                 App.leaves_listDb = details;

                 var del = App.leaves_list.RemoveAll(x => x.dbid_error_txt == db_id);

                 App.Current.MainPage = new MasterPage(new LeavesPage());
                 Loadingalertcall();

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
            //var currentpage = new LoadingIndicator();
            //await PopupNavigation.Instance.PushAsync(currentpage);

            act_ind.IsRunning = true;

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

            var employee_id = App.employee_id;

            TimeSpan diff = stopDate - startDate;
            string dur = diff.Days.ToString();

            string am_or_pm = "";
            if (check_half_day == true)
            {
                am_or_pm = half_dayPicker.SelectedItem.ToString();
            }
            else
            {
                am_or_pm = "";
            }

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

            if (CrossConnectivity.Current.IsConnected)
            {

                var leave_type_id = App.leaveTypeList.FirstOrDefault(x => x.Name2 == leave_pick.SelectedItem.ToString()).Id;
                var hr_year_id = App.hr_yearList.FirstOrDefault(x => x.Name == hr_year_picker.SelectedItem.ToString()).Id;

            vals["name"] = reason_entry.Text;
            vals["holiday_status_id"] = leave_type_id;
            vals["hr_year_id"] = hr_year_id;
            vals["employee_id"] = employee_id;

            vals["date_from"] = convertstart_datetime;
            vals["date_to"] = convertstop_datetime;
            vals["number_of_days_temp"] = dur;
            vals["half_day_related"] = check_half_day;
            vals["half_day"] = check_half_day;
            vals["am_or_pm"] = am_or_pm;
            vals["add_attachment"] = attfile;
            vals["remove_attachment"] = delete_attach;

           
                var updated = Controller.InstanceCreation().UpdateLeave("hr.holidays", "app_update_leave", levobj.id, vals);

                if (updated == "True")
                {
                    App.Current.MainPage = new MasterPage(new LeavesPage());
                    act_ind.IsRunning = false;
                  //  Loadingalertcall();
                    //  attach.Clear();
                    DependencyService.Get<Toast>().Show("Updated successfully...");
                }
                else
                {
                  //  Loadingalertcall();
                    act_ind.IsRunning = false;
                    DependencyService.Get<Toast>().Show(updated);
                }
            }

            else
            {
                var itemToRemove = App.leaves_listDb.Single(r => r.id == levobjDb.id);
                App.leaves_listDb.Remove(itemToRemove);

                string jso_addattchment_list = "";
                string jso_removeattchment_list = "";

                int loc_leave_type_id = App.leaveTypeListDb.FirstOrDefault(x => x.Name2 == leave_pick.SelectedItem.ToString()).Id;
                int loc_hr_year_id = App.hr_yearListDb.FirstOrDefault(x => x.Name == hr_year_picker.SelectedItem.ToString()).Id;

                App._connection.Query<LeavesModelDB>("DELETE FROM [LeavesModelDB] WHERE [id] = " + levobjDb.id);

                try
                {
                    jso_addattchment_list = Newtonsoft.Json.JsonConvert.SerializeObject(attfile);
                }

                catch
                {
                    int j = 0;
                    act_ind.IsRunning = true;
                }

                try
                {
                    jso_removeattchment_list = Newtonsoft.Json.JsonConvert.SerializeObject(delete_attach);
                }
                catch
                {
                    int jk = 0;
                    act_ind.IsRunning = true;
                }
               


                DateTime edit_dt = DateTime.ParseExact(convertstart_datetime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                string date_from = edit_dt.ToLocalTime().ToString("MMM dd");

                DateTime edit_dt1 = DateTime.ParseExact(convertstop_datetime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                string date_to = edit_dt.ToLocalTime().ToString("MMM dd");

                var sample = new LeavesModelDB
                {
                    id = levobjDb.id,
                    name = reason_entry.Text,
                    holidays_status = leave_pick.SelectedItem.ToString(),
                 //   holiday_status_id = leave_type_id,
                    year = hr_year_picker.SelectedItem.ToString(),
                    year_id = loc_hr_year_id,
                    datefrom = date_from,
                    dateto = date_to,
                  //  fromDate = startdate,
                  //  endDate = enddate,
                    date_from = convertstart_datetime,
                    date_to = convertstop_datetime,
                    leave_type_id = loc_leave_type_id,
                    employee_id = App.employee_id,                   
                    sync_string = "editexp.png",
                    stage_colour = levobjDb.stage_colour,
                    state = levobjDb.state,
                    state_name = levobjDb.state_name,
                    attachment_list = jso_addattchment_list,
                    remove_attachment = jso_removeattchment_list,
                    no_of_days = dur,
                    is_half_day = check_half_day,
                    check_tt = am_or_pm,
                    employee = levobjDb.employee,
                    check_rpc_condition = "edit",
                  //  error_attachment_list = attach_list,
                };
                App._connection.Insert(sample);

                App._connection.CreateTable<LeavesModelDB>();
                try
                {
                    var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();
                    App.leaves_listDb = details;

                    App.Current.MainPage = new MasterPage(new LeavesPage());
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

        private async void create_Button_Clicked(object sender, EventArgs e)
        {
            var currentpage = new LoadingIndicator();
            await PopupNavigation.Instance.PushAsync(currentpage);

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

            var leave_type_id = App.leaveTypeList.FirstOrDefault(x => x.Name2 == leave_pick.SelectedItem.ToString()).Id;
            var hr_year_id = App.hr_yearList.FirstOrDefault(x => x.Name == hr_year_picker.SelectedItem.ToString()).Id;
            var employee_id = App.employee_id;

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
            if (check_half_day == true)
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

            vals["name"] = reason_entry.Text;
            vals["holiday_status_id"] = leave_type_id;
            vals["hr_year_id"] = hr_year_id;
            vals["employee_id"] = employee_id;

            vals["date_from"] = convertstart_datetime;
            vals["date_to"] = convertstop_datetime;
            vals["number_of_days_temp"] = dur;
            vals["half_day_related"] = check_half_day;
            vals["half_day"] = check_half_day;
            vals["am_or_pm"] = am_or_pm;
            vals["add_attachment"] = attfile;


            var created = Controller.InstanceCreation().CreateLeave("hr.holidays","app_create_leave", vals);

            if (created == "True")
            {
                int db_id = levobj.dbid_error_txt;

                App._connection.Query<LeavesModelDB>("DELETE FROM LeavesModelDB WHERE Dbid=?", db_id);

                App._connection.CreateTable<LeavesModelDB>();
                var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();
                App.leaves_listDb = details;

                App.Current.MainPage = new MasterPage(new LeavesPage());
                Loadingalertcall();
                DependencyService.Get<Toast>().Show("Created successfully...");
            }
            else
            {
                Loadingalertcall();
                DependencyService.Get<Toast>().Show(created);
            }


        }

        private void createcancel_Button_Clicked(object sender, EventArgs e)
        {
            edit.IsVisible = true;

            add_attachment.IsEnabled = false;
            attachviewlist.IsEnabled = false;

            createbtn.IsVisible = false;
            delete.IsVisible = true;
            reasonlabel.IsVisible = true;
            reason_entry.IsVisible = false;

            leavetypelabel.IsVisible = true;
            leavetype_layout.IsVisible = false;

            //employee_namelabel.IsVisible = true;
            //employee_layout.IsVisible = false;

            hryearlabel.IsVisible = true;
            hryear_layout.IsVisible = false;

            startdate_label.IsVisible = true;
            startdate_layout.IsVisible = false;

            enddate_label.IsVisible = true;
            enddate_layout.IsVisible = false;

            //Half day field
            tt_label.IsVisible = true;
            if (levobj.is_half_day == false)
            {
                checkImg.IsVisible = false;
                uncheckImg.IsVisible = true;
                tt_label.Text = levobj.check_tt;
                overall_enddate_layout.IsVisible = true;
            }
            else
            {
                checkImg.IsVisible = true;
                uncheckImg.IsVisible = false;
                tt_label.Text = levobj.check_tt;
                overall_enddate_layout.IsVisible = false;
            }
            halfday_layout.IsVisible = false;

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

        public void productentry(object sender, EventArgs args)
        {
            // PopupNavigation.Instance.PushAsync(new PickerSelectionPage());
            PopupNavigation.Instance.PushAsync(new PickerSelectionPage("expense"));
        }

       
        private async void btnConfirm_Clicked(object sender, EventArgs e)
        {
            //var currentpage = new LoadingIndicator();
            //await PopupNavigation.Instance.PushAsync(currentpage);
            act_ind.IsRunning = true;

            bool updated = Controller.InstanceCreation().submittomanager("hr.holidays", "app_action_confirm", levobj.id);

          //  Loadingalertcall();

            if (updated)
            {
                DependencyService.Get<Toast>().Show("Updated Successfully...");

                btnRefuse.IsVisible = true;
                btnApprove.IsVisible = true;
                btnConfirm.IsVisible = false;
                btnDraft.IsVisible = true;

                statelabel.Text = "Waiting Pre-Approval";
                frame_state_color.BackgroundColor = Color.FromHex("#efb139");

                // btnApprove.

                act_ind.IsRunning = false;
            }
            else
            {
                DependencyService.Get<Toast>().Show("Try Again...");
                act_ind.IsRunning = false;
            }

            act_ind.IsRunning = false;
        }


        private async void btnApprove_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                act_ind.IsRunning = true;

                bool updated = Controller.InstanceCreation().submittomanager("hr.holidays", "app_action_approve", levobj.id);             
                if (updated)
                {
                    DependencyService.Get<Toast>().Show("Updated Successfully...");

                    btnRefuse.IsVisible = true;
                    btnApprove.IsVisible = false;
                    btnConfirm.IsVisible = false;
                    btnDraft.IsVisible = false;
                    statelabel.Text = "Approved";
                    frame_state_color.BackgroundColor = Color.FromHex("#2fb25f");
                    act_ind.IsRunning = false;

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


        private async void btnRefuse_Clicked(object sender, EventArgs e)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                //var currentpage = new LoadingIndicator();
                //await PopupNavigation.Instance.PushAsync(currentpage);

                act_ind.IsRunning = true;

                bool updated = Controller.InstanceCreation().submittomanager("hr.holidays", "app_action_refuse", levobj.id);

              //  Loadingalertcall();

                if (updated)
                {
                    DependencyService.Get<Toast>().Show("Updated Successfully...");

                    btnDraft.IsVisible = true;

                    btnApprove.IsVisible = false;
                    btnConfirm.IsVisible = false;
                    btnRefuse.IsVisible = false;

                    statelabel.Text = "Refused";
                    frame_state_color.BackgroundColor = Color.FromHex("#ea1621");
                    // btnApprove.
                    act_ind.IsRunning = false;
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

        private async void btnDraft_Clicked(object sender, EventArgs e)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                //var currentpage = new LoadingIndicator();
                //await PopupNavigation.Instance.PushAsync(currentpage);

                act_ind.IsRunning = true;

                bool updated = Controller.InstanceCreation().submittomanager("hr.holidays", "app_action_draft", levobj.id);

              //  Loadingalertcall();

                if (updated)
                {
                    DependencyService.Get<Toast>().Show("Updated Successfully...");

                    btnConfirm.IsVisible = true;
                    btnApprove.IsVisible = false;
                    btnRefuse.IsVisible = false;
                    btnDraft.IsVisible = false;

                    statelabel.Text = "New";
                    frame_state_color.BackgroundColor = Color.FromHex("#008FD3");
                    act_ind.IsRunning = false;
                    // btnApprove.
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

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            ViewCell obj = (ViewCell)sender;
            obj.View.BackgroundColor = Color.FromHex("#F0EEEF");
        }

    }
}