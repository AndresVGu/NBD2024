using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class BidLabour
    {     
        public int ID { get; set; }

        [Display(Name = "Hours")]
        [Required(ErrorMessage = "You cannot Leave Hours blank.")]

        public double HoursQuantity { get; set; } = 1;

        [Display(Name ="Bid")]
        public int BidID { get; set; }
        public Bid Bid { get; set; }

        [Display(Name ="Labour")]
        public int LabourID { get; set; }
        public Labour Labours { get; set; }


    }
}
