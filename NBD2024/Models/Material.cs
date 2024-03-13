using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Material
    {
        public int ID { get; set; }

        public string Summary
        {
            get
            {
                return Name + Price.ToString("C2");
            }
        }

        public string UnitPrice
        {
            get
            {
                return Price.ToString("C2");
            }
        }

        [Display(Name = "Material Name")]
        [Required(ErrorMessage = "Material names is required")]
        [StringLength(300, ErrorMessage = "Material name cannot be more 300 characters Long")]
        public string Name { get; set; }

        [Display(Name = "Material Description")]
        [StringLength(3000, ErrorMessage = "Description cannot be more than 3000 characters long")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "You must a price.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public ICollection<BidMaterial> BidMaterials { get; set; } = new HashSet<BidMaterial>();


    }
}
