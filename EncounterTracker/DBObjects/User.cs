using SQLite;

namespace EncounterTracker.DBObjects
{
    [Table("User")]
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int UserId { get; set; }

        [NotNull, Column("_username")]
        public string Username { get; set; }

        [NotNull, Column("_password")]
        public string Password { get; set; }
    }
}
