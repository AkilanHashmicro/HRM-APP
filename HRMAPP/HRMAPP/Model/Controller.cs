using HRMAPP.DBModel;
using HRMAPP.OdooRpc;
using HRMAPP.Persistance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using static HRMAPP.Model.HRMModel;

namespace HRMAPP.Model
{
    class Controller
    {

        private static Controller instance = null;
        private static readonly object padlock = new object();
        private OdooRPC odooConnector;

        private Controller()
        {
        }

        public string[] getDatabases(string url)
        {
            try
            {
                OdooRPC con = OdooRPC.InstanceCreation(url);
                return con.getDatabases();
            }
            catch (Exception ex)
            {
                throw ex;
            }

         }

        public String login(String url, String database, String username, String password)
        {
            try
            {
                odooConnector = OdooRPC.InstanceCreation(url);
                App.userid = odooConnector.login(database, username, password);
                return "true";
            }
            catch (Exception ea)
            {
                return "false";
            }
        }

        public List<LeavesModel> GetMasterData()
        {
            try
            {
                JObject res = odooConnector.odooFilterDataCall("res.users", "get_master_data");

                App.user_name = res["user_name"].ToString();
                App.user_image = res["user_image_medium"].ToString();
                App.user_mobile = res["user_mobile"].ToString();
                App.user_email = res["user_email"].ToString();
                App.partner_id = res["partner_id"].ToObject<int>();
                App.employee_id = res["employee_id"].ToObject<int>();

                App.productList = res["Products"].ToObject<List<ProductsList>>();
                App.expense_productList = res["expense_products"].ToObject<List<ExpenseProductsList>>();

                App.product_Uom = res["product_uom"].ToObject<List<ProductUOM>>();
                App.taxList = res["taxes"].ToObject<List<taxes>>();
                App.hr_yearList = res["hr_years"].ToObject<List<HRYear>>();
                App.currencyList = res["available_currency"].ToObject<List<Currency>>();
                App.leaveTypeList = res["holiday_status"].ToObject<List<LeaveType>>();

                App.employee_list = res["employees"].ToObject<List<EmployeesModel>>();

                App.access_dict = res["access_right"].ToObject<Dictionary<string, dynamic>>();

                App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                App._connection.CreateTable<UserModelDB>();

                if (App.UserListDb.Count == 0)
                {
                    var jso_tags_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["taxes"]);
                    var jso_products_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["Products"]);
                    var jso_expenseproducts_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["expense_products"]);
                    var jso_product_Uomlist = Newtonsoft.Json.JsonConvert.SerializeObject(res["product_uom"]);
                    var jso_hr_year_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["hr_years"]);
                    var jso_currency_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["available_currency"]);
                    var jso_leavetype_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["holiday_status"]);
                    var jso_employee_list = Newtonsoft.Json.JsonConvert.SerializeObject(res["employees"]);

                    var sample = new UserModelDB
                    {
                        userid = App.userid,
                        partnerid = App.partner_id,
                        employeeid = App.employee_id,
                        user_name = App.user_name,
                        user_email = App.user_email,
                        user_mobile = App.user_mobile,
                        user_image_medium = App.user_image,
                        products = jso_products_list,
                        expense_products = jso_expenseproducts_list,
                        tax_list = jso_tags_list,
                        product_Uom = jso_product_Uomlist,
                        hr_year_list = jso_hr_year_list,
                        currency_list = jso_currency_list,
                        leavetype_list = jso_leavetype_list,
                        employee_list = jso_employee_list,
                    };
                    App._connection.Insert(sample);

                    try
                    {
                        var details = (from y in App._connection.Table<UserModelDB>() select y).ToList();
                        App.UserListDb = details;

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
                            App.userid_db = item.userid;
                            App.ProductListDb = productslistdb;
                            App.expense_productListDb = expense_productslistdb;
                            App.taxListDb = taxListDb;
                            App.product_UomDb = product_UomDb;
                            App.hr_yearListDb = hr_yearListDb;
                            App.currencyListDb = currencyListDb;
                            App.leaveTypeListDb = leaveTypeListDb;
                            App.employee_idDb = item.employeeid;
                            App.employee_listDb = employeeListDb;
                        }

                    }
                    catch (Exception ex)
                    {
                        int i = 0;
                    }

                }

                /////// For Leaves

                App.leaves_list = res["holidays"].ToObject<List<LeavesModel>>();

                App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                App._connection.CreateTable<LeavesModelDB>();

              //  List<AttachmentFile> attlistDb = new List<AttachmentFile>();

               

                var leaves_details = App._connection.Query<LeavesModelDB>("SELECT * from LeavesModelDB where sync_string = ?", "sync.png");
                if (leaves_details.Count() == 0)
                {
                    App._connection.Query<LeavesModelDB>("DELETE from LeavesModelDB");

                    foreach (var item in App.leaves_list)
                    {
                        var dbattachment_list = Newtonsoft.Json.JsonConvert.SerializeObject(item.attachment);

                        var sample = new LeavesModelDB
                        {
                            id = item.id,
                            name = item.name,
                            display_name = item.display_name,
                            state = item.state,
                            state_name = item.state_name,
                            date_from = item.date_from,
                            date_to = item.date_to,
                            no_of_days = item.no_of_days,
                            stage_colour = item.stage_colour,
                            is_half_day = item.is_half_day,
                            am_or_pm = item.am_or_pm,
                            leave_structure = item.leave_structure,
                            year = item.year,
                            employee = item.employee,
                            report_note = item.report_note,
                            holidays_status = item.holidays_status,
                            datefrom = item.datefrom,
                            dateto = item.dateto,
                            fromDate = item.fromDate,
                            endDate = item.endDate,
                            check_tt = item.check_tt,
                            department = item.department,
                            attachment_list = dbattachment_list,
                            date = item.date,
                           
                        };
                        App._connection.Insert(sample);

                    }

                    try
                    {
                        var details = (from y in App._connection.Table<LeavesModelDB>() select y).ToList();
                        App.leaves_listDb = details;
                    }
                    catch
                    {
                        int i = 0;
                    }

                }

                var rpc_condition = App._connection.Query<LeavesModelDB>("SELECT * from LeavesModelDB where check_rpc_condition = ?", "false");
                if(rpc_condition.Count != 0)
                {
                  //  List<AttachmentFile> productslistdb = new List<AttachmentFile>();

                    foreach(var item in rpc_condition)
                    {
                        var sample = new LeavesModel
                        {
                            name = item.name,
                            holidays_status = item.holidays_status,
                            year = item.year,
                            date_from = item.date_from,
                            date_to = item.date_to,
                            state = item.state,
                            state_name = item.state_name,
                            stage_colour = item.stage_colour,
                            no_of_days = item.no_of_days,
                            is_half_day = item.is_half_day,
                            sync_string = item.sync_string,
                            am_or_pm = item.check_tt,
                            employee = item.employee,
                            error_rpcTxt = item.txt_rpc_error,
                            dbid_error_txt = item.Dbid,
                            check_rpc_condition = item.check_rpc_condition,
                          //  attachlistfield = item.error_attachment_list,

                            attachment =  JsonConvert.DeserializeObject<List<AttachmentFile>>(item.attachment_list),
                           // attachment = item.error_attachment_list,
                        };
                        App.leaves_list.Add(sample);
                    }
                }
                var data = App.leaves_list;
            
                /////// For Expense               
                App.expense_list = res["expense"].ToObject<List<ExpenseModel>>();

                App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
                App._connection.CreateTable<ExpenseModelDB>();

                var expense_details = App._connection.Query<ExpenseModelDB>("SELECT * from ExpenseModelDB where sync_string = ? OR sync_string = ?", "sync.png","editexp.png");
                if(expense_details.Count() == 0)
                {
                    
                    App._connection.Query<ExpenseModelDB>("DELETE from ExpenseModelDB");

                    foreach (var item in App.expense_list)
                    {
                        var expattach_list = Newtonsoft.Json.JsonConvert.SerializeObject(item.attachment);

                        var sample = new ExpenseModelDB
                        {
                            id = item.id,
                            name = item.name,
                            product = item.product,
                            product_uom = item.product_uom,
                            state = item.state,
                            unit_price = item.unit_price,
                            quantity = item.quantity,
                            reference = item.reference,
                            notes = item.notes,
                            account = item.account,
                            employee_id = item.employee_id,
                            currency = item.currency,
                            date = item.date,
                            stage_name = item.stage_name,
                            stage_colour = item.stage_colour,
                            half_date = item.half_date,
                            full_date = item.full_date,

                            attachment = expattach_list,
                        };
                        App._connection.Insert(sample);

                    }

                    try
                    {
                        var details = (from y in App._connection.Table<ExpenseModelDB>() select y).ToList();
                        App.expense_listDb = details;
                    }
                    catch
                    {
                        int i = 0;
                    }

                }

                return App.leaves_list;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("::::: HRM Warning Message ::::  " + ex.Message);

                if (ex.Message.Contains("(Network is unreachable)") || ex.Message.Contains("NameResolutionFailure"))
                {
                    App.NetAvailable = false;
                }
                else if (ex.Message.Contains("(503) Service Unavailable"))
                {
                    App.responseState = false;
                }
                return App.leaves_list;
            }

        }

        public List<TimesheetModel> GetTimesheetData()
        {
            List<TimesheetModel> tsdata = new List<TimesheetModel>();
            JArray result = odooConnector.odooMethodCall_gettimesheet<JArray>("res.users", "get_timesheet_data");
            tsdata = result.ToObject<List<TimesheetModel>>();
            return tsdata;
        }

        public List<ExpenseModel> GetExpenseData()
        {
            List<ExpenseModel> tsdata = new List<ExpenseModel>();
            JArray result = odooConnector.odooMethodCall_gettimesheet<JArray>("hr.expense", "get_expense");
            tsdata = result.ToObject<List<ExpenseModel>>();

            App.expense_list = tsdata;

            App._connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            App._connection.CreateTable<ExpenseModelDB>();

            var expense_details = App._connection.Query<ExpenseModelDB>("SELECT * from ExpenseModelDB where sync_string = ? OR sync_string = ?", "sync.png", "editexp.png");
            if (expense_details.Count() == 0)
            {

                App._connection.Query<ExpenseModelDB>("DELETE from ExpenseModelDB");

                foreach (var item in App.expense_list)
                {
                    var expattach_list = Newtonsoft.Json.JsonConvert.SerializeObject(item.attachment);

                    var sample = new ExpenseModelDB
                    {
                        id = item.id,
                        name = item.name,
                        product = item.product,
                        product_uom = item.product_uom,
                        state = item.state,
                        unit_price = item.unit_price,
                        quantity = item.quantity,
                        reference = item.reference,
                        notes = item.notes,
                        account = item.account,
                        employee_id = item.employee_id,
                        currency = item.currency,
                        date = item.date,
                        stage_name = item.stage_name,
                        stage_colour = item.stage_colour,
                        half_date = item.half_date,
                        full_date = item.full_date,

                        attachment = expattach_list,
                    };
                    App._connection.Insert(sample);

                }

                try
                {
                    var details = (from y in App._connection.Table<ExpenseModelDB>() select y).ToList();
                    App.expense_listDb = details;
                }
                catch
                {
                    int i = 0;
                }

            }

            return tsdata;

        }

        public List<ProjectsModel> GetProjectsData()
        {
            List<ProjectsModel> tsdata = new List<ProjectsModel>();
            JArray result = odooConnector.odooMethodCall_gettimesheet<JArray>("project.project", "search_read");
            tsdata = result.ToObject<List<ProjectsModel>>();
            return tsdata;
        }

        public List<TimesheetDetailsModel> GetTimesheetDetailsData()
        {
            List<TimesheetDetailsModel> tsdata = new List<TimesheetDetailsModel>();
            JArray result = odooConnector.odooMethodCall_gettimesheet_details<JArray>("res.users", "get_timesheet_details");
            tsdata = result.ToObject<List<TimesheetDetailsModel>>();
            return tsdata;
        }

        public List<AttendanceModel> GetAttendanceDetailsData()
        {
            List<AttendanceModel> tsdata = new List<AttendanceModel>();
            JArray result = odooConnector.odooMethodCall_getattendance_details<JArray>("hr.attendance", "get_attendance_data");
            tsdata = result.ToObject<List<AttendanceModel>>();
            return tsdata;
        }


        //public List<LeavesModel> Getleaves_data()
        //{
        //    List<LeavesModel> data = new List<LeavesModel>();
        //    try
        //    {
        //        object[] domain = new object[] { };
        //        JArray jdata = odooConnector.odooSearchReadCall<dynamic>("hr.holidays", domain, new string[] { "name", "state", "employee_id", "date_from", "date_to", "holiday_status_id", "hr_year_id", "holiday_type", "leave_config_id", "department_id", "half_day", "display_name", "number_of_days_temp", "am_or_pm" }, false);
        //        data = jdata.ToObject<List<LeavesModel>>();
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("::::: Leaves Warning Message ::::  " + ex.Message);
        //        return data;
        //    }
        //}

        //public List<ExpenseModel> GetExpense_data()
        //{
        //    List<ExpenseModel> data = new List<ExpenseModel>();
        //    try
        //    {
        //        object[] domain = new object[] { };
        //        JArray jdata = odooConnector.odooSearchReadCall<dynamic>("hr.expense", domain, new string[] { "name", "state", "product_id", "unit_amount", "reference", "date", "quantity", "account_id", "employee_id", "currency_id", "product_uom_id" }, false);
        //        data = jdata.ToObject<List<ExpenseModel>>();
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("::::: Expense Warning Message ::::  " + ex.Message);
        //        return data;
        //    }
        //}

        public bool CreateCheckIn(string modelName, string methodName, Dictionary<string, dynamic> vals)
        {
            bool flag = odooConnector.odoocreateAttendances(modelName, methodName, vals);
            return flag;
        }


        public bool UpdateCheckout(string modelName, string methodName, int checkIn_Id, Dictionary<string, dynamic> vals)
        {
            bool flag = odooConnector.odooUpdateAttendances(modelName, methodName, checkIn_Id, vals);
            return flag;
        }

        public string CreateLeave(string modelName, string methodName, Dictionary<string, dynamic> vals)
        {
            string flag = odooConnector.odoocreateLeaves(modelName, methodName, vals);
            return flag;
        }

        public string UpdateLeave(string modelName, string methodName,int leave_id ,Dictionary<string, dynamic> vals)
        {
            string flag = odooConnector.odooupdateLeaves(modelName, methodName,leave_id ,vals);
            return flag;
        }

        public bool deleteattachment(string modelName, string methodName, int att_id)
        {
            bool flag = odooConnector.odoodeleteattachment(modelName, methodName,  att_id);
            return flag;
        }

        public bool submittomanager(string modelName, string methodName, int att_id)
        {
            bool flag = odooConnector.odoosubmittomanager(modelName, methodName, att_id);
            return flag;
        }


        public bool submittimesheetdetails(string modelName, string methodName, Dictionary<string, dynamic> vals)
        {
            bool flag = odooConnector.odootimesheetsubmit(modelName, methodName, vals);
            return flag;
        }

        public JObject GetAttendanceData()
        {
            JObject dt = odooConnector.odooAttendanceCall<dynamic>("res.users", "get_attendance_data");
            return dt;
        }
            
        public static Controller InstanceCreation()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Controller();
                }
                return instance;
            }
        }

    }
}
