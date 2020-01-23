using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMAPP.Persistance
{
    public interface ISQLiteDb
    {
        SQLiteConnection GetConnection();
    }
}
