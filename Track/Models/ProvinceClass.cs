using System.ComponentModel.DataAnnotations;

namespace Track.Models
{
    public class ProvinceClass
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(400)]
        public string? Name { get; set; }

        [StringLength(400)]
        public string? NameNp { get; set; }

        public int? IMUCode { get; set; }

    }
}
