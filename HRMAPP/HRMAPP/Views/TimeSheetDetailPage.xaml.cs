using System; using System.Collections.Generic; using System.Diagnostics; using System.Linq; using HRMAPP.DBModel; using HRMAPP.Model; using Xamarin.Forms; using static HRMAPP.Model.HRMModel;  namespace HRMAPP.Views {     public partial class TimeSheetDetailPage : ContentPage     {
       
        // Stopwatch timer = new Stopwatch();         int project_id = 0;         TimesheetModel timesheetobj = new TimesheetModel();         List<TimesheetModelDB> timesheet_details = new List<TimesheetModelDB>();         List<ProjectsModel> tsresult = new List<ProjectsModel>();          public TimeSheetDetailPage(TimesheetModel obj)         {             InitializeComponent();              timesheetobj = obj;              tsresult = Controller.InstanceCreation().GetProjectsData();              project_picker.ItemsSource = tsresult.Select(n => n.display_name).ToList();             project_picker.SelectedIndex = 0;              project_id   = (tsresult.AsEnumerable().Where(p => p.display_name == project_picker.SelectedItem.ToString()).Select(p => p.id)).FirstOrDefault();               if (App.timesheet_listDb.Count != 0)             {
                timesheet_details = App._connection.Query<TimesheetModelDB>("SELECT * from TimesheetModelDB where timesheet_id = ? AND project_id =?", timesheetobj.id,project_id);             }              try             {

                if (timesheet_details.Count != 0)
                {

                    if (timesheet_details[0].start_btn && !timesheet_details[0].pause_btn)
                    {
                        start_layout.IsVisible = false;
                        pause_stop_layout.IsVisible = true;
                        DateTime dt1 = DateTime.ParseExact(timesheet_details[0].start_time, "yyyy-MM-dd HH:mm:ss", null);
                        DateTime dt2 = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", null);
                        TimeSpan ts = dt2.Subtract(dt1);
                        string elapsedTime = String.Format("{0:00}:{1:00}", ts.Hours, ts.Minutes);

                        rel_time.Text = elapsedTime;
                        pause_time.Text = "00:00";

                        pause_btn.IsVisible = true;
                        resume_btn.IsVisible = false;

                    }

                    else if (timesheet_details[0].start_btn && timesheet_details[0].pause_btn)
                    {
                        DateTime start_dt1 = DateTime.ParseExact(timesheet_details[0].start_time, "yyyy-MM-dd HH:mm:ss", null);
                        DateTime start_dt2 = DateTime.ParseExact(timesheet_details[0].pause_time, "yyyy-MM-dd HH:mm:ss", null);
                        TimeSpan start_ts = start_dt2.Subtract(start_dt1);
                        string elapsedTime = String.Format("{0:00}:{1:00}", start_ts.Hours, start_ts.Minutes);
                        rel_time.Text = elapsedTime;


                        start_layout.IsVisible = false;
                        pause_stop_layout.IsVisible = true;
                        DateTime dt1 = DateTime.ParseExact(timesheet_details[0].pause_time, "yyyy-MM-dd HH:mm:ss", null);
                        DateTime dt2 = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", null);
                        TimeSpan ts = dt2.Subtract(dt1);
                        string elapsedTime1 = String.Format("{0:00}:{1:00}", ts.Hours, ts.Minutes);
                        pause_time.Text = elapsedTime1;

                        start_layout.IsVisible = false;
                        resume_btn.IsVisible = true;
                        pause_btn.IsVisible = false;
                    }
                }             }              catch(Exception e)             {                 int j = 0;             }          }          public void start_layout_clicked(object sender, EventArgs args)         {                 start_layout.IsVisible = false;                pause_stop_layout.IsVisible = true;              //    timer.Start();              //// Get the elapsed time as a TimeSpan value.             //TimeSpan ts = timer.Elapsed;              //// Format and display the TimeSpan value.              //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);                       //   App._connection.Query<TimesheetModelDB>("DELETE from TimesheetModelDB");              var sample = new TimesheetModelDB             {                 stop_btn = false,                 start_btn = true,                 stop_time = "",                 pause_time = "",                 resume_btn = false,                 resume_time = "",                 project_id = project_id,                 start_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),                 timesheet_id = timesheetobj.id,                 // error_attachment_list = attach_list,             } ;             App._connection.Insert(sample);              App._connection.CreateTable<TimesheetModelDB>();             try             {                 var details = (from y in App._connection.Table<TimesheetModelDB>() select y).ToList();                 App.timesheet_listDb = details;             }             catch (Exception ex)             {                 int i = 0;             }                           // rel_time.Text = elapsedTime;         }          public void pausebtn__clicked(object sender, EventArgs args)         {             start_layout.IsVisible = false;             pause_stop_layout.IsVisible = true;             resume_btn.IsVisible = true;             pause_btn.IsVisible = false;              pause_time.IsEnabled = true;              //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);             //rel_time.Text = elapsedTime;              App._connection.Query<TimesheetModelDB>("UPDATE TimesheetModelDB set pause_time=? Where project_id=? AND timesheet_id=?", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), project_id, timesheetobj.id);             App._connection.Query<TimesheetModelDB>("UPDATE TimesheetModelDB set pause_btn=? Where project_id=? AND timesheet_id=?", true, project_id,timesheetobj.id);             //App._connection.Query<TimesheetModelDB>("UPDATE TimesheetModelDB Where timesheet_id=?", timesheetobj.id);                                try             {                 var details = (from y in App._connection.Table<TimesheetModelDB>() select y).ToList();                 App.timesheet_listDb = details;             }              catch             {                 int k = 0;             }               resume_btn.IsVisible = true;             pause_btn.IsVisible = false;                 // Format and display the TimeSpan value.                           }          public void resume__clicked(object sender, EventArgs args)         {             start_layout.IsVisible = false;             pause_stop_layout.IsVisible = true;             resume_btn.IsVisible = false;             pause_btn.IsVisible = true;              DateTime start_dt1 = DateTime.ParseExact(timesheet_details[0].start_time, "yyyy-MM-dd HH:mm", null);             DateTime start_dt2 = DateTime.ParseExact(timesheet_details[0].pause_time, "yyyy-MM-dd HH:mm", null);             TimeSpan start_ts = start_dt2.Subtract(start_dt1);               DateTime date_new = DateTime.Now;             int minutes = 0;              int hours = date_new.Hour - start_ts.Hours;              minutes = date_new.Minute - start_ts.Minutes;              if(minutes<0)             {                 hours = hours - 1;                 minutes = 60 - start_ts.Minutes;             }             int seconds =  start_ts.Seconds;              DateTime new_start_time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);              string new_start_time_string = new_start_time.ToString("yyyy-MM-dd HH:mm");              App._connection.Query<TimesheetModelDB>("UPDATE TimesheetModelDB set start_time=? Where project_id=?", new_start_time_string, project_id);                       App._connection.Query<TimesheetModelDB>("UPDATE TimesheetModelDB set pause_btn=? Where project_id=?", false, project_id);             App._connection.Query<TimesheetModelDB>("UPDATE TimesheetModelDB Where timesheet_id=?", timesheetobj.id);              pause_time.Text = "00:00";             rel_time.IsEnabled = true;              try             {                 var details = (from y in App._connection.Table<TimesheetModelDB>() select y).ToList();                 App.timesheet_listDb = details;             }              catch             {                 int k = 0;             }          }         public void stopbtn_clicked(object sender, EventArgs args)         {            // timer.Stop();            // timer.Reset();              Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();                          vals["sheet_id"] = timesheetobj.id;             vals["date"] = DateTime.Now.ToString("yyyy-MM-dd");             vals["user_id"] = App.userid;             vals["name"] = desc_text.Text;             vals["project_id"] = project_id;             vals["unit_amount"] = rel_time.Text;           bool res =   Controller.InstanceCreation().submittimesheetdetails("account.analytic.line","add_timesheet_activity",vals);              if(res)             {                 DependencyService.Get<Toast>().Show("Timesheet Successfully updated");                  timesheet_details = App._connection.Query<TimesheetModelDB>("DELETE FROM TimesheetModelDB where timesheet_id = ? AND project_id =?", timesheetobj.id, project_id);                  App.Current.MainPage = new MasterPage(new TimeSheetActivitiesPage());              }           }          protected override void OnAppearing()         {              base.OnAppearing();              Device.StartTimer(TimeSpan.FromSeconds(30), () =>             {                 timesheet_details = App._connection.Query<TimesheetModelDB>("SELECT * from TimesheetModelDB where timesheet_id = ? AND project_id =?", timesheetobj.id, project_id);                              try                  {              if (timesheet_details.Count != 0)                 // if( timesheet_details != null)                 {

                    if (timesheet_details[0].start_btn && !timesheet_details[0].pause_btn)
                    {
                        start_layout.IsVisible = false;
                        pause_stop_layout.IsVisible = true;
                        DateTime dt1 = DateTime.ParseExact(timesheet_details[0].start_time, "yyyy-MM-dd HH:mm", null);
                        DateTime dt2 = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", null);
                        TimeSpan ts = dt2.Subtract(dt1);
                        string elapsedTime = String.Format("{0:00}:{1:00}", ts.Hours, ts.Minutes);
                         rel_time.Text = "";
                        rel_time.Text = elapsedTime;
                        pause_time.Text = "00:00";                         // resume_btn.IsVisible = true;                         pause_btn.IsVisible = true;                         stop_btn.IsVisible = true;

                    }

                    else if (timesheet_details[0].start_btn && timesheet_details[0].pause_btn)
                    {
                        DateTime start_dt1 = DateTime.ParseExact(timesheet_details[0].start_time, "yyyy-MM-dd HH:mm", null);
                        DateTime start_dt2 = DateTime.ParseExact(timesheet_details[0].pause_time, "yyyy-MM-dd HH:mm", null);
                        TimeSpan start_ts = start_dt2.Subtract(start_dt1);
                        string elapsedTime = String.Format("{0:00}:{1:00}", start_ts.Hours, start_ts.Minutes);
                        rel_time.Text = elapsedTime;


                        start_layout.IsVisible = false;
                        pause_stop_layout.IsVisible = true;
                        DateTime dt1 = DateTime.ParseExact(timesheet_details[0].pause_time, "yyyy-MM-dd HH:mm", null);
                        DateTime dt2 = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", null);
                        TimeSpan ts = dt2.Subtract(dt1);
                        string elapsedTime1 = String.Format("{0:00}:{1:00}", ts.Hours, ts.Minutes);
                        pause_time.Text = elapsedTime1;

                        resume_btn.IsVisible = true;
                        pause_btn.IsVisible = false;                         stop_btn.IsVisible = true;
                    }                 }

                                    }                 catch(Exception e)                 {                     int k = 0;                 }                  return true;             }                                 );           }           void Handle_SelectedIndexChanged(object sender, System.EventArgs e)         {
            //  project_picker.ItemsSource = tsresult.Select(n => n.display_name).ToList();
            // project_picker.SelectedIndex = 0;
            timesheet_details = null;               project_id = (tsresult.AsEnumerable().Where(p => p.display_name == project_picker.SelectedItem.ToString()).Select(p => p.id)).FirstOrDefault();              if (App.timesheet_listDb.Count != 0)             {                 timesheet_details = App._connection.Query<TimesheetModelDB>("SELECT * from TimesheetModelDB where timesheet_id = ? AND project_id =?", timesheetobj.id, project_id);             }


            try             {
                if (timesheet_details.Count != 0)

                {

                    if (timesheet_details[0].start_btn && !timesheet_details[0].pause_btn)
                    {
                        start_layout.IsVisible = false;
                        pause_stop_layout.IsVisible = true;
                        DateTime dt1 = DateTime.ParseExact(timesheet_details[0].start_time, "yyyy-MM-dd HH:mm", null);
                        DateTime dt2 = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", null);
                        TimeSpan ts = dt2.Subtract(dt1);
                        string elapsedTime = String.Format("{0:00}:{1:00}", ts.Hours, ts.Minutes);

                        rel_time.Text = elapsedTime;
                        pause_time.Text = "00:00";

                        resume_btn.IsVisible = false;
                        pause_btn.IsVisible = true;
                        stop_btn.IsVisible = true;

                    }

                    else if (timesheet_details[0].start_btn && timesheet_details[0].pause_btn)
                    {
                        DateTime start_dt1 = DateTime.ParseExact(timesheet_details[0].start_time, "yyyy-MM-dd HH:mm", null);
                        DateTime start_dt2 = DateTime.ParseExact(timesheet_details[0].pause_time, "yyyy-MM-dd HH:mm", null);
                        TimeSpan start_ts = start_dt2.Subtract(start_dt1);
                        string elapsedTime = String.Format("{0:00}:{1:00}", start_ts.Hours, start_ts.Minutes);
                        rel_time.Text = elapsedTime;


                        start_layout.IsVisible = false;
                        pause_stop_layout.IsVisible = true;
                        DateTime dt1 = DateTime.ParseExact(timesheet_details[0].pause_time, "yyyy-MM-dd HH:mm", null);
                        DateTime dt2 = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "yyyy-MM-dd HH:mm", null);
                        TimeSpan ts = dt2.Subtract(dt1);
                        string elapsedTime1 = String.Format("{0:00}:{1:00}", ts.Hours, ts.Minutes);
                        pause_time.Text = elapsedTime1;

                        resume_btn.IsVisible = true;
                        pause_btn.IsVisible = false;
                        stop_btn.IsVisible = true;
                    }
                }

                else
                {
                    start_layout.IsVisible = true;
                    pause_stop_layout.IsVisible = false;

                    rel_time.Text = "00:00";
                    pause_time.Text = "00:00";
                }              }              catch(Exception exc)             {                 int k = 0;             }          }         } }



