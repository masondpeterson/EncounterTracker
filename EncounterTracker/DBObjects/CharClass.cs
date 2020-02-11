using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterTracker.DBObjects
{
    [Table("CharClass")]
    public class CharClass
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ClassId { get; set; }

        [NotNull, Column("_name")]
        public string ClassName { get; set; }
    }
}
