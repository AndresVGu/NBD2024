using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Position
    {
        public int ID { get; set; }

        [Display(Name = "Position")]
        [Required(ErrorMessage = "Position name cannot be blank.")]
        [StringLength(100, ErrorMessage = "Position name cannot be more than 100 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Project")]
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        [Display(Name = "Worker")]
        public int StaffID { get; set; }
        public Staff Staff { get; set; }
    }
}
