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
                return Quantity + Name + Price.ToString();
            }
        }
        #endregion
        [Display(Name = "Material")]
        [Required(ErrorMessage = "You cannot leave the material name blank.")]
        [StringLength(80, ErrorMessage = "Material name cannot be more than 80 characters long")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You must enter the Quantity.")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; } = 1;
 
        [Display(Name = "Type")]
        public string? type { get; set; }

        [Required(ErrorMessage = "You must enter the Price.")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "You must enter the Purchase Price.")]
        [Display(Name = "Purchase Price")]
        [DataType(DataType.Currency)]
        public double PurchasePrice{ get; set; }
ICollection<Material> Materials { get; set; } = new HashSet<Material>();
    }
}
