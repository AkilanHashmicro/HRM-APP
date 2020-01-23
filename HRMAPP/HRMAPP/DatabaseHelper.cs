using HRMAPP.Persistance;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HRMAPP
{
    public class DatabaseHelper
    {
        static SQLiteConnection sqliteconnection;
        public const string DbFileName = "HRM.db";

        public DatabaseHelper()
        {
            sqliteconnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            //sqliteconnection.CreateTable<ProductsList>();
        }

    }
}
