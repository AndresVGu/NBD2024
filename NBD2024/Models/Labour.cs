using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Labour
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave first name blank.")]
        public double LabourHours { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave first name blank.")]
        public string LabourDescription { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave first name blank.")]
        public double LabourUnitPrice { get; set; }

        [Display(Name = "Extended Price")]
        public double LabourExtendedPrice
        {
            get { return LabourHours * LabourUnitPrice; }
        }

        //Forgien keys

        [Display(Name = "Project")]
        [Required(ErrorMessage = "You cannot leave project blank.")]
        public int ProjectID { get; set; }

        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Display(Name = "Project")]
        [Required(ErrorMessage = "You cannot leave project blank.")]
        public int LabourTypeID { get; set; }

        [Display(Name = "Labour Type")]
        public LabourType LabourType { get; set; }

    }
}
