using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Material
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "The Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Material Quantity must be greater than zero.")]
        public int Quantity { get; set; } = 1;

        [Display(Name = "Description")]
         public string? Description { get; set; }


        [Display(Name = "Price per yard")]
        [Required(ErrorMessage = "You must enter the charhge per Yard.")]
        [DataType(DataType.Currency)]
        public double PerYardCharge { get; set; }

        [Display(Name ="Inventory")]
        public int InventoryID { get; set; }
        public Inventory Inventory { get; set; }
       

    }
}
