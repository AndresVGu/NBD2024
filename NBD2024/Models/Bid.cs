using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Bid
    {
        public int Id { get; set; }


        //Forgien keys

        [Display(Name = "Client")]
        public int ClientID { get; set; }

        [Display(Name = "Client")]
        [Required(ErrorMessage = "You cannot leave client blank.")]
        public Client Client { get; set; }

    }
}
