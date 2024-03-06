using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class LabourType
    {
        public int ID { get; set; }

        public string Summary => Name;

        [Display(Name = "Labour Type")]
        [Required(ErrorMessage = "You cannot leave the labour type blank.")]
        [StringLength(120, ErrorMessage = "Labour type cannot be more than 120 characters long.")]
        public string Name { get; set; }

  ICollection<Labour> labours { get; set; } = new HashSet<Labour>();
    }
}
