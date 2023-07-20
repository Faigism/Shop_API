using System.ComponentModel.DataAnnotations;

namespace First_API.Dtos.ProductDtos
{
    public class ProductPutDto
    {
        [Required]
        [MaxLength(35)]
        public string Name { get; set; }
        [Range(0, int.MaxValue)]
        public int BrandId { get; set; }
        [Range(0, int.MaxValue)]
        public decimal SalePrice { get; set; }
        [Range(0, int.MaxValue)]
        public decimal CostPrice { get; set; }
    }
}
