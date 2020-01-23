using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using HRMAPP.Persistance;
using SQLite;
using UIKit;

namespace HRMAPP.iOS.Persistance
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteConnection GetConnection()
        {
            var filename = "MySQLite.db";
            var documentspath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var librarypath = Path.Combine(documentspath, " ", "Library");
            var path = Path.Combine(librarypath, filename);

            return new SQLiteConnection(path);
        }
    }
}