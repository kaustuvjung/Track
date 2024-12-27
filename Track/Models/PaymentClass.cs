using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Track.Models
{
    public class PaymentClass
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Method { get; set; }
        [Required]
        public int Amount { get; set; }

        [Range(0, 100)]
        public float? commissionper { get; set; }

        public float? commission { get; set; }
        [StringLength(450)]

       
        [Required]
        //[JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? PDate {  get; set; }

        [Required]
        public int Bill_id { get; set; }
        [ValidateNever]
        [ForeignKey(nameof(Bill_id))]
        public BillClass? Bill { get; set; }

        public string? Commissino_to { get; set;}
    }
}
