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

        [Column("_charId")]
        public int CharacterId { get; set; }

        [Column("_kill")]
        public int Kills { get; set; }

        [Column("_assist")]
        public int Assist { get; set; }

        [Column("_dmg")]
        public int Damage { get; set; }

        [Column("_taken")]
        public int DamageTaken { get; set; }

        [Column("_exp")]
        public int Experience { get; set; }
    }
}
