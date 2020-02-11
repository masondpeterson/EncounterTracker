using System;
using System.Collections.Generic;
using System.Text;
using EncounterTracker.DBObjects;
using SQLite;

namespace EncounterTracker.DataBase
{
    public class SQLConn
    {
        private SQLiteConnection _conn;

        public SQLConn()
        {
            _conn = new SQLiteConnection(Constants.DatabasePath);
            _conn.CreateTable<User>();
        }

        #region User Methods

        public int InsertUser(User user)
        {
            var result = _conn.Insert(user);
            return result;
        }

        public List<string> GetUserNames()
        {
            var usernames = new List<string>();
            var users = _conn.Table<User>();
            foreach (var user in users)
            {
                usernames.Add(user.Username);
            }
            return usernames;
        }

        public User GetUserByName(string username)
        {
            var user = from u in _conn.Table<User>()
                       where u.Username == username
                       select u;
            return user.First();
        }

        #endregion
    }
}
