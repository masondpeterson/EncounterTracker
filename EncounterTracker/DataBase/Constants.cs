using System;
using System.IO;

namespace EncounterTracker.DataBase
{
    public static class Constants
    {
        //Built from the example found in Microsoft Docs
        //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/databases

        public const string DatabaseFilename = "sqlData.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
