﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterTracker.DBObjects
{
    [Table("Character")]
    public class CharacterBase
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int CharacterId { get; set; }

        [NotNull, Column("_userId")]
        public int UserId { get; set; }

        [NotNull, Column("_classId")]
        public int ClassId { get; set; }
    }
}
