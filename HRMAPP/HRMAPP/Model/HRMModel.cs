using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HRMAPP.Model
{
    public class HRMModel
    {

        public class LeavesModel
        {
            public int id { get; set; }
            public string display_name { get; set; }
            public string name { get; set; }             // reason
            public string state { get; set; }
            public string state_name { get; set; }
            public string date_from { get; set; }
            public string date_to { get; set; }
            public string no_of_days { get; set; }
            public string stage_colour { get; set; }
            public Boolean is_half_day { get; set; }
            public string am_or_pm { get; set; }
            public string leave_structure { get; set; }
            public string year { get; set; }
            public string employee { get; set; }
            public string report_note { get; set; }
            public string holidays_status { get; set; }
            public string department { get; set; }

            public string sync_string { get; set; }
            public string error_rpcTxt { get; set; }
            public int dbid_error_txt { get; set; }
            public string check_rpc_condition { get; set; }
           // public string attachlistfield { get; set; }
         //   public string attachment { get; set; }

            public List<AttachmentFile> attachment { get; set; }
            public string date { get; set; }


            public string check_tt
            {
                get
                {
                    string data = "";
                    try
                    {
                        if(am_or_pm.Equals("False"))
                        {
                            data = "";
                        }
                        else if (am_or_pm == "")
                        {
                            data = "";
                        }
                        else if (am_or_pm == null)
                        {
                            data = "";
                        }
                        else
                        {
                            data = am_or_pm;
                        }
                    }
                    catch
                    {
                        data = "";
                    }
                    return data;

                }
                set
                {

                }
            }
         
            public string datefrom
            {
                get
                {
                    if (date_from == "")
                    {
                        return "";
                    }
                    else
                    {
                        DateTime dt = DateTime.ParseExact(date_from, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        string date = dt.ToLocalTime().ToString("MMM dd");
                        return date;
                    }
                }
                set
                {

                }
            }

            public string dateto
            {                
                get
                {
                    if (date_to == "")
                    {
                        return "";
                    }
                    else
                    {

                        DateTime dt = DateTime.ParseExact(date_to, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        string date = dt.ToLocalTime().ToString("MMM dd");
                        return date;
                    }
                   
                }
                set
                {

                }
            }

            public string fromDate
            {
                get
                {
                    if (date_from == "")
                    {
                        return "";
                    }
                    else
                    {
                        DateTime dt = DateTime.ParseExact(date_from, "yyyy-MM-dd HH:mm:ss", null);
                        string datetime = dt.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                        return datetime;
                    }
                }
                set
                {

                }
            }

            public string endDate
            {
                get
                {
                    if (date_to == "")
                    {
                        return "";
                    }
                    else
                    {

                        DateTime dt = DateTime.ParseExact(date_to, "yyyy-MM-dd HH:mm:ss", null);
                        string datetime = dt.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                        return datetime;
                    }
                }
                set
                {

                }
            }
          
        }

        public class ExpenseModel
        {
            public int id { get; set; }
            public string name { get; set; }
            public string product { get; set; }
            public string product_uom { get; set; }
            public string state { get; set; }
            public float unit_price { get; set; }
            public float quantity { get; set; }
            public string reference { get; set; }
            public string notes { get; set; }
            public string account { get; set; }
            public string employee_id { get; set; }
            public string currency { get; set; }
            public string date { get; set; }
            public string stage_name { get; set; }
            public string stage_colour { get; set; }

            public List<AttachmentFile> attachment { get; set; }


            //public string stateName { get; set; }
            //public string state_name
            //{
            //    get
            //    {
            //        if (state.Equals("draft"))
            //        {
            //            stateName = "To Submit";
            //        }
            //        else if (state.Equals("reported"))
            //        {
            //            stateName = "Reported";
            //        }
            //        else if (state.Equals("done"))
            //        {
            //            stateName = "Posted";
            //        }
            //        else
            //        {
            //            stateName = "Refused";    //state ==> refused
            //        }
                    
            //        return stateName;
            //    }
            //    set
            //    {

            //    }
            //}

            //public string color { get; set; }
            //public string colorcode
            //{
            //    get
            //    {
            //        if(state.Equals("draft"))
            //        {
            //            color = "#008FD3";
            //        }
            //        else if (state.Equals("reported"))
            //        {
            //            color = "#efb139";
            //        }
            //        else if (state.Equals("done"))
            //        {
            //            color = "#2fb25f";
            //        }
            //        else
            //        {
            //            color = "#ea1621";    
            //        }

            //        return color;
            //    }
            //    set
            //    {

            //    }
            //}


            public string half_date
            {
                get
                {
                    DateTime dt = DateTime.ParseExact(date, "yyyy-MM-dd", null);
                    string convert = dt.ToLocalTime().ToString("MMMM dd");
                    return convert;
                }
                set
                {

                }
            }

            public string full_date
            {
                get
                {
                    DateTime dt = DateTime.ParseExact(date, "yyyy-MM-dd", null);
                    string convert = dt.ToLocalTime().ToString("dd/MM/yyyy");
                    return convert;
                }
                set
                {

                }
            }

        }

        public class AttachmentFile
        {
            public string file_name { get; set; }
            public string filepath { get; set; }
            public string file_data { get; set; }
            public string filebase64 { get; set; }
            public int id { get; set; }

        }

        public class taxes
        {
            public int ID { get; set; }
            public string Name { get; set; }

            public taxes(string name)
            {
                Name = name;
            }

        }


        public class ProductsList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string list_price { get; set; }

            public ProductsList(string name)
            {
                Name = name;
            }
        }

        public class ExpenseProductsList
        {
            // [PrimaryKey, AutoIncrement]
            public int id { get; set; }
            public string Name { get; set; }
            public string list_price { get; set; }
            public string uom_name { get; set; }
            public int uom_id { get; set; }

            public ExpenseProductsList(string name)
            {
                // Id = id;
                Name = name;
            }
        }

        public class ProductUOM
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class HRYear
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Currency
        {
            public int id { get; set; }
            public string Name { get; set; }
        }



        public class LeaveType
        {
            public int Id { get; set; }
            public string Name2 { get; set; }
        }


        public class EmployeesModel
        {
            public string department { get; set; }
            public string email { get; set; }
            public int id { get; set; }
            public string mobile { get; set; }
            public string name { get; set; }

        }

        public class AttendanceModel
        {
            public string check_in { get; set; }
            public string check_out { get; set; }
            public int id { get; set; }
            public string sign_in_lat { get; set; }
            public string sign_in_lng { get; set; }
            public string sign_out_lat { get; set; }
            public string sign_out_lng { get; set; }
            public string check_in_location { get; set; }
            public string check_out_location { get; set; }
            public string employee_name { get; set; }
                      
        }
           
        public class TimesheetModel
        {
            public int id { get; set; }
            public string name { get; set; }
            public int total_difference { get; set; }
            public int total_attendance { get; set; }
            public string date_from { get; set; }
            public string date_to { get; set; }
            public string state { get; set; }
            public string state_colour { get; set; }
            public List<timesheet_attendances> attendances { get; set; }
            public List<timesheet_details> details { get; set; }


            public string convertfromDate
            {
                get
                {
                    DateTime dt = DateTime.ParseExact(date_from, "yyyy-MM-dd", null);
                    string convert = dt.ToLocalTime().ToString("MMMM dd");
                    return convert;
                }
                set
                {

                }
            }

            public string convertToDate
            {
                get
                {
                    DateTime dt = DateTime.ParseExact(date_to, "yyyy-MM-dd", null);
                    string convert = dt.ToLocalTime().ToString("MMMM dd");
                    return convert;
                }
                set
                {

                }
            }

        }

        public class TimesheetDetailsModel
        {
            public string project_name { get; set; }
            public int user_id { get; set; }
            public string name { get; set; }
            public string unit_amount { get; set; }
            public string date { get; set; }
            public int project_id { get; set; }
            public string user_name { get; set; }
            public int id { get; set; }
        }

        public class timesheet_attendances
        {
            public int total_attendance { get; set; }
            public string name { get; set; }
            public int total_timesheet { get; set; }
            public string sheet_name { get; set; }
            public int total_difference { get; set; }
            public int sheet_id { get; set; }

        }

        public class timesheet_details
        {
            public string date { get; set; }
            public string project_name { get; set; }
            public int project_id { get; set; }
            public string name { get; set; }
            public int unit_amount { get; set; }
        }

        public class ProjectsModel
        {
            public int id { get; set; }
            public string display_name { get; set; }
        }
       
      
    }
}
