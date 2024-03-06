using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Bid
    {
        public int ID { get; set; }

        [Display(Name = "Bid Date")]
        [Required(ErrorMessage = "You cannot leave bid date blank.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MMM-dd}", ApplyFormatInEditMode = true)]

        public DateTime BidDate { get; set; }
        //Forgien keys
        [Display(Name ="Project")]
        public int ProjectID { get; set; }
        public Project Project { get; set; }

        [Display(Name = "Material")]
        public int MaterialID { get; set; }
        public Material Material { get; set; }

        [Display(Name = "Labour")]
        public int LabourID { get; set; }
        public Labour Labour { get; set; }
        ICollection<Material> Materials { get; set; } = new HashSet<Material>();
        ICollection<Labour> Labours { get; set; } = new HashSet<Labour>();

    }
}
