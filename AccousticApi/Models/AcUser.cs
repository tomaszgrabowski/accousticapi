namespace AccousticApi.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class AcUser
    {
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
