using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Material
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "The Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Material Quantity must be greater than zero.")]
        public int Quantity { get; set; } = 1;

        [Display(Name = "Area")]
        [Required(ErrorMessage = "You must enter the Square Area.")]
        public double Area { get; set; } = 1;


        [Display(Name = "Price per yard")]
        [Required(ErrorMessage = "You must enter the charhge per Yard.")]
        [DataType(DataType.Currency)]
        public double PerYardCharge { get; set; }

        [Required(ErrorMessage ="You must Select a Client")]
        [Display(Name ="Client")]
        public int ClientID { get; set; }
        [Display(Name ="Client")]
        public Client Client { get; set; }  

        [Display(Name = "Proyect")]
        public int ProyectID { get; set; }
        public Project Project { get; set; }

        [Display(Name = "Inventory")]
        public int InventoryID { get; set; }
        public Inventory Inventory { get; set; }

    }
}
