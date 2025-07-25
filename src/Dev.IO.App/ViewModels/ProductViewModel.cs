using DevIO.App.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevIO.App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [DisplayName("Supplier")]
        public Guid SupplierId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(200, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(100, ErrorMessage = "The field {0} must have between {2} and {1} characters", MinimumLength = 2)]
        public string Description { get; set; }

        [DisplayName("Product Image")]
        public IFormFile ImageUpload { get; set; }

        public string Image { get; set; }

        [Currency]
        [Required(ErrorMessage = "The field {0} is required.")]
        public decimal Price { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Is Active?")]
        public bool Active { get; set; }

        public SupplierViewModel Supplier { get; set; }

        public IEnumerable<SupplierViewModel> Suppliers { get; set; }
    }
}
