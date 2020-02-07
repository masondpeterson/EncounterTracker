using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterTracker.DBObjects
{
    [Table("Character")]
    public class CharClass
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ClassId { get; set; }

        [NotNull, Column("_name")]
        public int ClassName { get; set; }

    }
}
