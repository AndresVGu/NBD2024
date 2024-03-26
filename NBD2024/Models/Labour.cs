using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Labour
    {
        public int ID { get; set; }
        public string Summary
        {
            get
            {
                return Name + " - " + Price.ToString("C2");
            }
        }

        public string UnitPrice
        {
            get
            {
                return Price.ToString("C2");
            }
        }

        [Display(Name = "Labour Name")]
        [Required(ErrorMessage = "Labour names is required")]
        [StringLength(300, ErrorMessage = "Labour name cannot be more 300 characters Long")]
        public string Name { get; set; }


        [Display(Name = "Labour Description")]
        [StringLength(3000, ErrorMessage = "Description cannot be more than 3000 characters long")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Price per Hour")]
        [Required(ErrorMessage = "You must a price.")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public ICollection<BidLabour> BidLabours { get; set; } = new HashSet<BidLabour>();
    }
}
