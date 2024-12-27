using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Track.Models
{
    public class BillClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Customer_id { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(Customer_id))]
        public CustomerClass? Customer { get; set; }
        public string? Billno { get; set; }
        [Required]
        public decimal total { get; set; }

        public DateTime? Date {  get; set; } 

        public decimal? payment { get; set; }

        public string? Remark { get; set; }

        public string? hasChalani { get; set; }
    }
}
