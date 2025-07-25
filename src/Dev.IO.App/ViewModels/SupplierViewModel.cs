using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevIO.App.ViewModels
{
    public class SupplierViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(100, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(14, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 11)]
        public string Document { get; set; }

        [DisplayName("Type")]
        public int Type { get; set; }
        
        [DisplayName("Is Active?")]
        public bool Active { get; set; }

        public AddressViewModel Address { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
