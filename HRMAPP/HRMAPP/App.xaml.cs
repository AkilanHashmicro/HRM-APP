using HRMAPP.DBModel;
using HRMAPP.Model;
using HRMAPP.Persistance;
using HRMAPP.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static HRMAPP.Model.HRMModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HRMAPP
{
    public partial class App : Application
    {
        public static SQLiteConnection _connection;

        public static int userid = 0;

        public static int partner_id = 0;
        public static int employee_id = 0;
        public static string user_name = "";
        public static string user_image = "";
        public static string user_email = "";
        public static string user_mobile = "";


        public static int userid_db = 0;
        public static int employee_idDb = 0;


        public static bool NetAvailable = false;
        public static bool responseState = false;

        public static int atten_checkIn_id = 0;
        public static bool check_in_out = false;

        public static bool masterpage_check = false;

        public static int leave_id = 0;

        public static Dictionary<string, dynamic> filterdict = new Dictionary<string, dynamic>();

        public static List<LeavesModel> leaves_list = new List<LeavesModel>();
        public static List<ExpenseModel> expense_list = new List<ExpenseModel>();


        public static List<LeavesModelDB> leaves_listDb = new List<LeavesModelDB>();
        public static List<ExpenseModelDB> expense_listDb = new List<ExpenseModelDB>();


        public static List<ProductsList> productList = new List<ProductsList>();
        public static List<ExpenseProductsList> expense_productList = new List<ExpenseProductsList>();
        public static List<ProductUOM> product_Uom = new List<ProductUOM>();
        public static List<taxes> taxList = new List<taxes>();
        public static List<Currency> currencyList = new List<Currency>();
        public static List<HRYear> hr_yearList = new List<HRYear>();
        public static List<LeaveType> leaveTypeList = new List<LeaveType>();

        public static List<UserModelDB> UserListDb = new List<UserModelDB>();
        public static List<ProductsList> ProductListDb = new List<ProductsList>();
        public static List<ExpenseProductsList> expense_productListDb = new List<ExpenseProductsList>();
        public static List<ProductUOM> product_UomDb = new List<ProductUOM>();
        public static List<taxes> taxListDb = new List<taxes>();
        public static List<Currency> currencyListDb = new List<Currency>();
        public static List<HRYear> hr_yearListDb = new List<HRYear>();
        public static List<LeaveType> leaveTypeListDb = new List<LeaveType>();

        public static List<EmployeesModel> employee_list = new List<EmployeesModel>();
        public static List<EmployeesModel> employee_listDb = new List<EmployeesModel>();

        public static Dictionary<string, dynamic> access_dict = new Dictionary<string, dynamic>();
        public static List<TimesheetModelDB> timesheet_listDb = new List<TimesheetModelDB>();

        public App()
        {
            InitializeComponent();

            App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();

            App._connection.CreateTable<TimesheetModelDB>();
            try
            {
                var details = (from y in App._connection.Table<TimesheetModelDB>() select y).ToList();
                App.timesheet_listDb = details;
            }
            catch (Exception ex)
            {
                int i = 0;
            }

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

            App._connection.CreateTable<UserModelDB>();
            try
            {
                var details = (from y in App._connection.Table<UserModelDB>() select y).ToList();
                App.UserListDb = details;
            }
            catch (Exception ex)
            {
                int i = 0;
            }
            List<ProductsList> productslistdb = new List<ProductsList>();
            List<ExpenseProductsList> expense_productslistdb = new List<ExpenseProductsList>();
            List<taxes> taxListDb = new List<taxes>();
            List<ProductUOM> product_UomDb = new List<ProductUOM>();
            List<Currency> currencyListDb = new List<Currency>();
            List<HRYear> hr_yearListDb = new List<HRYear>();
            List<LeaveType> leaveTypeListDb = new List<LeaveType>();
            List<EmployeesModel> employeeListDb = new List<EmployeesModel>();

            foreach (var item in App.UserListDb)
            {
                productslistdb = JsonConvert.DeserializeObject<List<ProductsList>>(item.products);
                expense_productslistdb = JsonConvert.DeserializeObject<List<ExpenseProductsList>>(item.expense_products);
                taxListDb = JsonConvert.DeserializeObject<List<taxes>>(item.tax_list);
                product_UomDb = JsonConvert.DeserializeObject<List<ProductUOM>>(item.product_Uom);
                currencyListDb = JsonConvert.DeserializeObject<List<Currency>>(item.currency_list);
                hr_yearListDb = JsonConvert.DeserializeObject<List<HRYear>>(item.hr_year_list);
                leaveTypeListDb = JsonConvert.DeserializeObject<List<LeaveType>>(item.leavetype_list);
                employeeListDb = JsonConvert.DeserializeObject<List<EmployeesModel>>(item.employee_list);

                App.ProductListDb = productslistdb;
                App.expense_productListDb = expense_productslistdb;
                App.userid_db = item.userid;
                App.taxListDb = taxListDb;
                App.product_UomDb = product_UomDb;
                App.hr_yearListDb = hr_yearListDb;
                App.currencyListDb = currencyListDb;
                App.leaveTypeListDb = leaveTypeListDb;
                App.employee_idDb = item.employeeid;
                App.employee_listDb = employeeListDb;

            }


            if (Settings.UserName.Length > 0 && Settings.UserPassword.Length > 0)
            {

                String res = "";
                try
                {
                    string url = Settings.UserUrlName;
                    string database = Settings.UserDbName;
                    string username = Settings.UserName;
                    string password = Settings.UserPassword;

                    res = Controller.InstanceCreation().login(url, database, username, password);

                   List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();

                  App.Current.MainPage = new MasterPage(new DashboardPage());

                  //  App.Current.MainPage = new MasterPage(new TimeSheetDetailPage());

                 //   App.atten_checkIn_id = Convert.ToInt32(Settings.CheckIn_ID);

                    if (Settings.CheckIn_Out.Equals("false"))
                    {
                        App.check_in_out = false;
                    }
                    else
                    {
                        App.check_in_out = true;
                    }

                }
                catch (Exception ea)
                {
                    if (ea.Message.Contains("(Network is unreachable)") || ea.Message.Contains("NameResolutionFailure"))
                    {
                        App.NetAvailable = false;
                    }
                    else if (ea.Message.Contains("(503) Service Unavailable"))
                    {
                        App.responseState = false;
                    }
                }
                if (App.NetAvailable == false)
                {
                    App.Current.MainPage = new MasterPage(new DashboardPage());
                }

            }

            else
            {
                MainPage = new Views.LoginPage();

               // MainPage = new Views.TimeSheetDetailPage();
              //  MainPage = new Views.AIPage();
            }

        }

        async Task MainRefreshData()
        {
            try
            {
                Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                {
                    if (App.NetAvailable == false)
                    {
                        List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();
                    }
                    //var leaves_details = App._connection.Query<LeavesModelDB>("SELECT * from LeavesModelDB where sync_string = ?", "sync.png");
                    var leaves_details = App._connection.Query<LeavesModelDB>("SELECT * from LeavesModelDB where check_rpc_condition = ?", "true");

                    var rpc_condition = App._connection.Query<LeavesModelDB>("SELECT * from LeavesModelDB where check_rpc_condition = ?", "false");

                    if (leaves_details.Count() != 0 && App.NetAvailable == true )
                    {
                        foreach (var dbobj in leaves_details)
                        {
                            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

                            vals["name"] = dbobj.name;
                            vals["holiday_status_id"] = dbobj.leave_type_id;
                            vals["hr_year_id"] = dbobj.year_id;
                            vals["employee_id"] = dbobj.employee_id;
                            vals["date_from"] = dbobj.date_from;
                            vals["date_to"] = dbobj.date_to;
                            vals["half_day"] = dbobj.is_half_day;
                            vals["half_day_related"] = dbobj.is_half_day;
                            vals["am_or_pm"] = dbobj.check_tt;
                            vals["number_of_days_temp"] = dbobj.no_of_days;

                            var json_attachment = JsonConvert.DeserializeObject(dbobj.attachment_list);

                            vals["attachment"] = json_attachment;

                            var created = "";

                            created = Controller.InstanceCreation().CreateLeave("hr.holidays", "app_create_leave", vals);
                            if (created == "True")
                            {
                                //App._connection.Query<LeavesModelDB>("UPDATE LeavesModelDB set sync_string=? Where Dbid=?", "", dbobj.Dbid);
                                
                                App._connection.Query<LeavesModelDB>("DELETE FROM LeavesModelDB WHERE Dbid=?", dbobj.Dbid);

                                App._connection.CreateTable<LeavesModelDB>();
                                var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();
                                App.leaves_listDb = details;

                                List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();
                                MessagingCenter.Send<string, String>("MyApp", "LeavesListUpdated", "true");
                            }
                            else
                            {
                                App._connection.Query<LeavesModelDB>("UPDATE LeavesModelDB set check_rpc_condition=? Where Dbid=?", "false", dbobj.Dbid);                                
                                App._connection.Query<LeavesModelDB>("UPDATE LeavesModelDB set txt_rpc_error=? Where Dbid=?", created, dbobj.Dbid);

                                App._connection.CreateTable<LeavesModelDB>();
                                var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();

                                App.leaves_listDb = details;

                                LeavesModel data = new LeavesModel();
                                data.name = dbobj.name;
                                data.holidays_status = dbobj.holidays_status;
                                data.year = dbobj.year;
                                data.date_from = dbobj.date_from;
                                data.date_to = dbobj.date_to;
                                data.state = dbobj.state;
                                data.state_name = dbobj.state_name;
                                data.stage_colour = dbobj.stage_colour;
                                data.no_of_days = dbobj.no_of_days;
                                data.is_half_day = dbobj.is_half_day;
                                data.sync_string = dbobj.sync_string;   ///"sync.png";
                                data.am_or_pm = dbobj.check_tt;
                                data.employee = dbobj.employee;
                                data.error_rpcTxt = created;
                                data.dbid_error_txt = dbobj.Dbid;
                                data.check_rpc_condition = "false";

                                data.attachment = JsonConvert.DeserializeObject<List<AttachmentFile>>(dbobj.error_attachment_list);
                               // data.attachment = dbobj.error_attachment_list;
                                int i = 0;

                                App.leaves_list.Add(data);

                                var lea = App.leaves_list;

                                List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();
                                MessagingCenter.Send<string, String>("MyApp", "LeavesListUpdated", "true");

                            }

                        }
                    }


                    var leave_edits = App._connection.Query<LeavesModelDB>("SELECT * from LeavesModelDB where check_rpc_condition = ?", "edit");

                    if (leave_edits.Count() != 0 && CrossConnectivity.Current.IsConnected == true)
                    {
                        foreach (var dbobj in leave_edits)
                        {
                            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();
                            List<dynamic> attfile = new List<dynamic>();

                            List<AttachmentFile> addattachlistDb = JsonConvert.DeserializeObject<List<AttachmentFile>>(dbobj.attachment_list);
                            List<int> removeListDb = JsonConvert.DeserializeObject<List<int>>(dbobj.remove_attachment);

                            foreach (var data in addattachlistDb)
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

                            vals["name"] = dbobj.name;
                            vals["holiday_status_id"] = dbobj.leave_type_id;
                            vals["hr_year_id"] = dbobj.year_id;
                            vals["employee_id"] = dbobj.employee_id;

                            vals["date_from"] = dbobj.date_from;
                            vals["date_to"] = dbobj.date_to;
                            vals["number_of_days_temp"] = dbobj.no_of_days;
                            vals["half_day_related"] = dbobj.is_half_day;
                            vals["half_day"] = dbobj.is_half_day;
                            vals["am_or_pm"] = dbobj.check_tt;
                            vals["add_attachment"] = addattachlistDb;
                            vals["remove_attachment"] = removeListDb;
                   
                      //      var created = "";

                            var updated = Controller.InstanceCreation().UpdateLeave("hr.holidays", "app_update_leave", dbobj.id, vals);

                            if (updated == "True")
                            {
                                //App._connection.Query<LeavesModelDB>("UPDATE LeavesModelDB set sync_string=? Where Dbid=?", "", dbobj.Dbid);

                                App._connection.Query<LeavesModelDB>("DELETE FROM LeavesModelDB WHERE Dbid=?", dbobj.Dbid);

                                App._connection.CreateTable<LeavesModelDB>();
                                var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();
                                App.leaves_listDb = details;

                                Task.Run(() => Controller.InstanceCreation().GetMasterData()); 
                              //  List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();
                                MessagingCenter.Send<string, String>("MyApp", "LeavesListUpdated", "true");

                            }
                            else
                            {
                                
                                Task.Run(() => Controller.InstanceCreation().GetMasterData()); 
                                MessagingCenter.Send<string, String>("MyApp", "ErrorListUpdated", updated);

                                App._connection.Query<LeavesModelDB>("DELETE FROM LeavesModelDB WHERE Dbid=?", dbobj.Dbid);

                                App._connection.CreateTable<LeavesModelDB>();
                                var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();
                                App.leaves_listDb = details;
                                                                                           
                            }
                        }

                    }


                    var expense_details = App._connection.Query<ExpenseModelDB>("SELECT * from ExpenseModelDB where check_rpc_condition = ?", "true");

                    if (expense_details.Count() != 0 && App.NetAvailable == true)
                    {
                        foreach (var dbobj in expense_details)
                        {

                            Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

                            vals["name"] = dbobj.name;
                            vals["product_id"] = dbobj.product_id;
                            vals["unit_amount"] = dbobj.unit_price;
                            vals["quantity"] = dbobj.quantity;
                            vals["product_uom_id"] = dbobj.uom_id;
                            vals["reference"] = dbobj.reference;
                            vals["date"] = dbobj.date;
                            vals["employee_id"] = dbobj.emp_id;
                            vals["currency_id"] = dbobj.currency_id;
                            vals["total_amount"] = dbobj.total;

                            var json_attachment = JsonConvert.DeserializeObject(dbobj.attachment);
                            vals["attachment"] = json_attachment;

                            var created = Controller.InstanceCreation().CreateLeave("hr.expense", "app_create_expense", vals);


                            if (created == "True")
                            {
                                //App._connection.Query<LeavesModelDB>("UPDATE LeavesModelDB set sync_string=? Where Dbid=?", "", dbobj.Dbid);

                                App._connection.Query<ExpenseModelDB>("DELETE FROM ExpenseModelDB WHERE Dbid=?", dbobj.Dbid);

                                App._connection.CreateTable<ExpenseModelDB>();
                                var details = (from y in App._connection.Table<ExpenseModelDB>() select y).ToList();
                                App.expense_listDb = details;

                                List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();
                                MessagingCenter.Send<string, String>("MyApp", "ExpenseListUpdated", "true");
                            }

                        }

                    }


                    var expense_edits = App._connection.Query<ExpenseModelDB>("SELECT * from ExpenseModelDB where check_rpc_condition = ?", "edit" );

                    if (expense_edits.Count() != 0 && App.NetAvailable == true)
                    {
                        foreach (var dbobj in expense_edits)
                        {
                             Dictionary<string, dynamic> vals = new Dictionary<string, dynamic>();

                            vals["name"] = dbobj.name;
                            vals["product_id"] = dbobj.product_id;
                            vals["unit_amount"] = dbobj.unit_price;
                            vals["quantity"] = dbobj.quantity;
                            vals["product_uom_id"] = dbobj.uom_id;
                            vals["reference"] = dbobj.reference;
                            vals["date"] = dbobj.date;
        
                           List<AttachmentFile> addattachlistDb = JsonConvert.DeserializeObject<List<AttachmentFile>>(dbobj.attachment);
                           List<int> removeListDb = JsonConvert.DeserializeObject<List<int>>(dbobj.remove_attachment);

                            vals["add_attachment"] = addattachlistDb;
                            vals["remove_attachment"] = removeListDb;
                            vals["employee_id"] = dbobj.emp_id;
                            vals["currency_id"] = dbobj.currency_id;

                            var created = Controller.InstanceCreation().UpdateLeave("hr.expense", "app_update_expense", dbobj.id, vals);

                            if (created == "True")
                            {

                                App._connection.Query<ExpenseModelDB>("DELETE FROM ExpenseModelDB WHERE Dbid=?", dbobj.Dbid);

                                App._connection.CreateTable<ExpenseModelDB>();
                                var details = (from y in App._connection.Table<ExpenseModelDB>() select y).ToList();
                                App.expense_listDb = details;

                                // List<LeavesModel> Data = Controller.InstanceCreation().GetMasterData();

                               Task.Run(() => Controller.InstanceCreation().GetMasterData());
                                MessagingCenter.Send<string, String>("MyApp", "ExpenseListUpdated", "true");
                            }

                            else
                            {
                             //   App._connection.Query<ExpenseModelDB>("DELETE FROM ExpenseModelDB WHERE Dbid=?", dbobj.Dbid);

                             //   App._connection.CreateTable<ExpenseModelDB>();
                             //   var details = (from y in App._connection.Table<ExpenseModelDB>() select y).ToList();
                             //   App.expense_listDb = details;

                             //Task.Run(() => Controller.InstanceCreation().GetMasterData());
                                //MessagingCenter.Send<string, String>("MyApp", "ExpenseListUpdated", "true");
                            }
                        }

                    }

                   //List< TimesheetModelDB> timesheet_edits = App._connection.Query<TimesheetModelDB>("SELECT * from TimesheetModelDB ");

                    //if(timesheet_edits.Count !=0)
                    //{
                    //    foreach (var dbobj in timesheet_edits)
                    //    {

                    //        MessagingCenter.Send<string, TimesheetModelDB>("MyApp", "timesheetUpdate", dbobj);
                    //    }

                    //    System.Diagnostics.Debug.Write("AKILLLLLLLLLLLLLLL" + timesheet_edits);
                    //}

                    return true;
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            await MainRefreshData();

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
