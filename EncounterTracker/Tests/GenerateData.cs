using EncounterTracker.DataBase;
using EncounterTracker.DBObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterTracker.Tests
{
    public class GenerateData
    {
        private SQLConn _conn = new SQLConn();
        private Simple3Des _secure = new Simple3Des();
        private int _id;
        private List<Character> _clist = new List<Character>();

        public GenerateData()
        {
            
        }

        public void GenerateTestData()
        {
            //Clear Database to ensure a clean start for the demo
            _conn.ClearDatabase();

            //Create and insert demo user
            CreateUser();

            //Create 12 Characters for demo user
            CreateCharacters();

            //Create Encounters for Each Character
            CreateEncounters();
        }

        #region Support Methods

        private void CreateUser()
        {
            var user = new User();
            user.Username = "demo";
            user.Password = _secure.EncryptData("test");
            _conn.InsertUser(user);
            _id = _conn.GetUserByName(user.Username).UserId;
        }

        private void CreateCharacters()
        {
            _conn.PopulateCharClassTable();
            var clist = _conn.GetCharClasses();
            int i = 0;
            foreach (var cClass in clist)
            {
                //Create a new Character of the specified class and insert it into the DB
                var pc = new Character();
                pc.CharName = "Nash " + i;
                pc.ClassId = cClass.ClassId;
                pc.UserId = _id;
                _conn.InsertCharacter(pc);
                i++;

                //Get the Character back so it has an id and add it to the character list
                var npc = _conn.GetCharacterByName(pc.CharName);
                _clist.Add(npc);
            }
        }

        private void CreateEncounters()
        {
            foreach (var c in _clist)
            {
                //Loop to Create 3 random encounter stat sets
                for(int i=0; i<5; i++)
                {
                    //Create Encounter and fill with random stats for given Character and User
                    var e = new Encounter();
                    e.EncounterName = "Fight" + c.CharacterId + _id + i;
                    e.UserId = _id;
                    e.CharId = c.CharacterId;
                    e.Kills = Rand();
                    e.Assist = Rand();
                    e.DmgDealt = Rand();
                    e.DmgTaken = Rand();
                    e.Healing = Rand();
                    e.Dropped = SmRand();
                    e.Session = DateTime.Now.AddDays(Rand());
                    _conn.InsertEncounter(e);
                }
            }
        }

        private int Rand()
        {
            var random = new Random();
            return random.Next(0, 100);
        }

        private int SmRand()
        {
            var random = new Random();
            return random.Next(0, 1);
        }

        private DateTime DtRand()
        {
            var randomTest = new Random();

            TimeSpan timeSpan = DateTime.Now - DateTime.Now.AddDays(-365);
            TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
            DateTime newDate = DateTime.Now + newSpan;
            return newDate;
        }

        #endregion


    }
}
