using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Labour
    {
        public int ID { get; set; }

        [Display(Name ="Hours")]
        public string HoursSummary
        {
            get
            {
                return $"{LabourHours.ToString("F2")}";
            }
        }

        [Display(Name = "Price/Hour")]
        public string PriceHours
        {
            get
            {
                return $"{LabourUnitPrice.ToString("C2")}";
            }
        }

        [Display(Name = "Extended Price")]
        public string Total
        {
            get
            {
                return $"{LabourExtendedPrice.ToString("C2")}";
            }
        }

       
        [Display(Name = "Hours")]
        [Required(ErrorMessage = "You cannot leave first name blank.")]
        public double LabourHours { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "You cannot leave first name blank.")]
        public string LabourDescription { get; set; }

        [Display(Name = "Price/Hour")]
        [Required(ErrorMessage = "You cannot leave first name blank.")]
        public double LabourUnitPrice { get; set; }

        [Display(Name = "Extended Price")]
        public double LabourExtendedPrice
        {
            get { return LabourHours * LabourUnitPrice; }
        }

        //Forgien keys

        [Display(Name ="Labour Type")]
       public int LabourTypeID { get; set; }
        public LabourType LabourType { get; set; }
        
    }
}
