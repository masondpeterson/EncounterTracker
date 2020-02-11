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
            _conn.CreateTable<Character>();
            _conn.CreateTable<CharClass>();
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
            return user.FirstOrDefault();
        }

        #endregion

        #region Character Methods

        public int InsertCharacter(Character character)
        {
            var result = _conn.Insert(character);
            return result;
        }

        public List<Character> GetUserCharacters(int userId)
        {
            var character = from c in _conn.Table<Character>()
                       where c.UserId == userId
                       select c;
            return character.ToList();
        }

        #endregion

        #region CharClass Methods

        private int InsertCharClass(CharClass charClass)
        {
            var result = _conn.Insert(charClass);
            return result;
        }

        public List<CharClass> GetCharClasses()
        {
            return _conn.Table<CharClass>().ToList();
        }

        public void PopulateCharClassTable()
        {
            var classNames = GenerateClassNames();

            foreach (var name in classNames)
            {
                if (ValidateClass(name))
                {
                    var charClass = new CharClass();
                    charClass.ClassName = name;
                    InsertCharClass(charClass);
                }
            }
        }

        #endregion

        #region Support Methods

        private bool ValidateClass(string name)
        {
            var check = true;
            var charClasses = GetCharClasses();
            foreach (var c in charClasses)
            {
                if (c.ClassName == name)
                {
                    check = false;
                }
            }
            return check;
        }

        private List<string> GenerateClassNames()
        {
            var classNames = new List<string>();
            classNames.Add("Barbarian");
            classNames.Add("Bard");
            classNames.Add("Cleric");
            classNames.Add("Druid");
            classNames.Add("Fighter");
            classNames.Add("Monk");
            classNames.Add("Paladin");
            classNames.Add("Ranger");
            classNames.Add("Rogue");
            classNames.Add("Sorcerer");
            classNames.Add("Warlock");
            classNames.Add("Wizard");
            return classNames;
        }

        #endregion
    }
}
