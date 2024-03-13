﻿using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class BidMaterial
    {
        public int ID { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "You cannot Leave Quantity blank.")]

        public int MaterialQuantity { get; set; } = 1;

        [Display(Name ="Bid")]
  
        public int BidID { get; set; }
        public Bid Bid {  get; set; }

      

        [Display(Name ="Material")]
        public int MaterialID { get; set; }
        public Material Materials { get; set; }
   

    }
}
