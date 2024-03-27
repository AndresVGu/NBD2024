using System.ComponentModel.DataAnnotations;

namespace NBD2024.Models
{
    public class Bid : Auditable, IValidatableObject
    {
        public int ID { get; set; }
        public string Total
        {
            get
            {
                return "Sumar MaterialsTotal + LabourTotal";
            }
        }

        public string MaterialTotal
        {
            get
            {
                return "total material";
            }
        }

        public string LabourTotal
        {
            get
            {
                return "Labour Total";
            }
        }

        [Display(Name = "Bid Date")]
        [Required(ErrorMessage = "You cannot leave bid date blank.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MMM-dd}", ApplyFormatInEditMode = false)]

        public DateTime BidDate { get; set; } = DateTime.Today;

        //Concurrency:
        [ScaffoldColumn(false)]
        [Timestamp]
        public Byte[] RowVersion { get; set; }

        //Forgien keys
        [Display(Name ="Project")]
        public int ProjectID { get; set; }
        public Project Project { get; set; }

        [Display(Name ="Labour")]
        public ICollection<BidLabour>  BidLabours { get; set; } = new HashSet<BidLabour>();
        [Display(Name ="Material")]
         public ICollection<BidMaterial> BidMaterials { get; set; } = new HashSet<BidMaterial>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Project Date cannot be in the past, yyyy because that is when NBD open.
            if (BidDate < DateTime.Today)
            {
                yield return new ValidationResult("Date cannot be in the past", new[] { "BidDate" });
            }
        }

    }
}
