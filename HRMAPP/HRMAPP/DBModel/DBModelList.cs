using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMAPP.DBModel
{
    public class DBModelList
    {

    }

    [Table("UserModelDB")]
    public class UserModelDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }

        public int userid { get; set; }
        public int partnerid { get; set; }
        public int employeeid { get; set; }
        public string user_name { get; set; }
        public string user_image_medium { get; set; }
        public string user_email { get; set; }
        public string user_mobile { get; set; }

        public string products { get; set; }
        public string expense_products { get; set; }
        public string product_Uom { get; set; }
        public string tax_list { get; set; }
        public string hr_year_list { get; set; }
        public string currency_list { get; set; }
        public string leavetype_list { get; set; }
        public string employee_list { get; set; }

    }

    [Table("LeavesModelDB")]
    public class LeavesModelDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }

        public int id { get; set; }
        public string display_name { get; set; }
        public string name { get; set; }
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

        public string check_tt { get; set; }

        // custom datetime
        public string datefrom { get; set; }
        public string dateto { get; set; }

        // full datetime
        public string fromDate { get; set; }
        public string endDate { get; set; }

        //creation field
        public string attachment_list { get; set; }
        public int leave_type_id { get; set; }
        public int year_id { get; set; }
        public int employee_id { get; set; }
        public string yellowimg_string { get; set; }
        public string sync_string { get; set; }

        //public string convert_startDatetime { get; set; }
        //public string convert_stopDatetime { get; set; }
        public string check_rpc_condition { get; set; }
        public string txt_rpc_error { get; set; }
        public string error_attachment_list { get; set; }

        public string remove_attachment { get; set; }
        public string date { get; set; }

    }

    [Table("ExpenseModelDB")]
    public class ExpenseModelDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }

        public int id { get; set; }
        public string name { get; set; }
        public string product { get; set; }
        public int product_id { get; set; }
        public string product_uom { get; set; }
        public string state { get; set; }
        public float unit_price { get; set; }
        public float quantity { get; set; }
        public string reference { get; set; }
        public string notes { get; set; }
        public string attachment { get; set; }
        public string account { get; set; }
        public string employee_id { get; set; }
        public int emp_id { get; set; }
        public string currency { get; set; }
        public int currency_id { get; set; }
        public string date { get; set; }
        public string stage_name { get; set; }
        public string stage_colour { get; set; }
        public string sync_string { get; set; }
        public double total { get; set; }
        public string check_rpc_condition { get; set; }
        public int uom_id { get; set; }

        // converted date format
        public string half_date { get; set; }
        public string full_date { get; set; }

        public string remove_attachment { get; set; }

    }

    [Table("TimesheetModelDB")]
    public class TimesheetModelDB
    {
        [PrimaryKey, AutoIncrement]
        public int Dbid { get; set; }
        public int project_id { get; set; }
        public string start_time { get; set; }
        public bool start_btn { get; set; }
        public string pause_time { get; set; }
        public bool pause_btn { get; set; }
        public string resume_time { get; set; }
        public bool resume_btn { get; set; }
        public string stop_time { get; set; }
        public bool stop_btn { get; set; }

        public int total_hours { get; set; }
        public int toral_minutes { get; set; }
        public int total_seconds { get; set; }

        public int timesheet_id { get; set; }
    }

}
