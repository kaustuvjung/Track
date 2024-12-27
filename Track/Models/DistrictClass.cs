using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Track.Models
{
    public class DistrictClass
    {
        [Key]
        public int Id { get; set; }

        public int ProvinceId { get; set; }
        [ValidateNever]
        [ForeignKey("ProvinceId")]
        public ProvinceClass? Province { get; set; }

        [Required]
        [StringLength(400)]
        public string? Name { get; set; }

        [Required]
        [StringLength(400)]
        public string? NameNp{ get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        public int? IMUCode { get; set; }
    }
}
