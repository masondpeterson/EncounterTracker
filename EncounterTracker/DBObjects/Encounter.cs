using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterTracker.DBObjects
{
    [Table("Encounter")]
    public class Encounter
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int EncounterId { get; set; }

        [NotNull, Column("_name")]
        public string EncounterName { get; set; }

        [NotNull, Column("_userId")]
        public int UserId { get; set; }

        [NotNull, Column("_charId")]
        public int CharId { get; set; }

        [Column("_kill")]
        public int Kills { get; set; }

        [Column("_assist")]
        public int Assist { get; set; }

        [Column("_dmg")]
        public int DmgDealt { get; set; }

        [Column("_taken")]
        public int DmgTaken { get; set; }

        [Column("_heal")]
        public int Healing { get; set; }

        [Column("_dropped")]
        public int Dropped { get; set; }

        [Column("_dateTime")]
        public DateTime Session { get; set; }
    }
}
