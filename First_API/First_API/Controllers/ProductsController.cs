using First_API.DAL;
using First_API.Dtos.ProductDtos;
using First_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace First_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProductsController(ApiDbContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public ActionResult<List<ProductListItemDto>> GetAll()
        {
            var data = _context.Products.Select(x => new ProductListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                BrandName = x.Brand.Name,
            }).ToList();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public ActionResult<ProductGetDto> Get(int id)
        {
            var entity = _context.Products.Include(x => x.Brand).FirstOrDefault(x => x.Id == id);
            if(entity == null) return NotFound();
            var data = new ProductGetDto
            {
                Name = entity.Name,
                SalePrice = entity.SalePrice,
                CostPrice = entity.CostPrice,
                Brand = new BrandInProductGetDto
                {
                    Id = entity.Brand.Id,
                    Name = entity.Brand.Name,
                }
            };
            return Ok(data);
        }
        [HttpPost("")]
        public IActionResult Create(ProductPostDto dto)
        {
            if (!_context.Brands.Any(x => x.Id == dto.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand not found");
                return BadRequest(ModelState);
            }

            Product entity = new Product
            {
                Name = dto.Name,
                SalePrice = dto.SalePrice,
                CostPrice = dto.CostPrice,
                BrandId = dto.BrandId,
            };
            _context.Products.Add(entity);
            _context.SaveChanges();

            return StatusCode(201, new { id = entity.Id });
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductPutDto dto)
        {
            var entity = _context.Products.Find(id);
            if(entity == null) return NotFound();

            if (!_context.Brands.Any(x => x.Id == dto.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand not found");
                return BadRequest(ModelState);
            }
                
            entity.Name = dto.Name;
            entity.SalePrice = dto.SalePrice;
            entity.CostPrice = dto.CostPrice;
            entity.BrandId = dto.BrandId;

            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _context.Products.Find(id);
            if (entity == null) return NotFound();

            _context.Products.Remove(entity);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
