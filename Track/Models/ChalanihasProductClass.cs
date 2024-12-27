using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Track.Models
{
    public class ChalanihasProductClass
    {
        public int Id { get; set; }
        public int? Chalani_id { get; set; }

        [ForeignKey(nameof(Chalani_id))]
        [ValidateNever]
        public ChalaniClass? Chalani{ get; set; }

        [Required]
        public int Quantity { get; set; }

        public int? product_id { get; set; }
        [ForeignKey("product_id ")]
        [ValidateNever]
        public ProductClass? Product { get; set; }

        [StringLength(450)]
        public string? User_id { get; set; }
    }
}
