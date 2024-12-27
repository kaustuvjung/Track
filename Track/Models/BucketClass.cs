using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Track.Models
{
    public class BucketClass
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? Product_id { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(Product_id))]
        public ProductClass? Product { get; set; }
        public int? Order_id { get; set; }

        [Required]
        public int? Quantity{ get; set;}

        [Required]
        [MaxLength(450)]
        [ValidateNever]
        public string? User_id { get; set; }
    }
}
