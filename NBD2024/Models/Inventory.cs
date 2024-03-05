using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Inventory
    {
        public int ID { get; set; }

        #region Summary Properties
        public string Summary
        {
            get
            {
                return Name + " (STd. " + StandardCharge.ToString("c") + ")";
            }
        }
        #endregion
        [Display(Name = "Material")]
        [Required(ErrorMessage = "You cannot leave the material name blank.")]
        [StringLength(80, ErrorMessage = "Material name cannot be more than 80 characters long")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You must enter the Standard Charge Per Unit.")]
        [Display(Name = "Standard Charge")]
        [DataType(DataType.Currency)]
        public double StandardCharge { get; set; }
        public ICollection<Material> Materials { get; set; } = new HashSet<Material>();
    }
}
