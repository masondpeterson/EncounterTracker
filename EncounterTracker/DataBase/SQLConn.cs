using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EncounterTracker.DataBase
{
    public class SQLConn
    {
        private SQLiteConnection _conn;

        public SQLConn()
        {
            _conn = new SQLiteConnection(Constants.DatabasePath);
        }
    }
}
