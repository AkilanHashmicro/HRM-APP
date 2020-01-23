using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HRMAPP.Droid.Persistance;
using HRMAPP.Persistance;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]

namespace HRMAPP.Droid.Persistance
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteConnection GetConnection()
        {
            var filename = "MySQLite.db";
            var documentspath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentspath, filename);

            return new SQLiteConnection(path);
        }
    }
}