using System.ComponentModel.DataAnnotations;

namespace First_API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public Brand Brand { get; set; }
    }
}
