using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Data;
using ShopApp.Service.Dtos.ProductDtos;
using ShopApp.Core.Entities;
using ShopApp.Core.Repositories;
using ShopApp.Service.Interfaces;

namespace ShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("")]
        public IActionResult Create(ProductPostDto postDto)
        {
            /*if (!_brandRepository.IsExist(x => x.Id == postDto.BrandId))
            {
                ModelState.AddModelError("BrandId", $"Brand not found by Id {postDto.BrandId}");
                return BadRequest(ModelState);
            }
            Product product = new Product
            {
                BrandId = postDto.BrandId,
                Name = postDto.Name,
                CostPrice = postDto.CostPrice,
                SalePrice = postDto.SalePrice,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                ModifiedAt = DateTime.UtcNow.AddHours(4),
            };
            _productRepository.Add(product);
            _productRepository.Commit();

            return StatusCode(201, new { Id = product.Id });*/
            return StatusCode(201, _productService.Create(postDto));
        }
        [HttpGet("{id}")]
        public ActionResult<ProductGetDto> Get(int id)
        {
            /*Product product = _productRepository.Get(x => x.Id == id, "Brand");
            if(product == null) return NotFound();

            ProductGetDto productDto = new ProductGetDto
            {
                Name = product.Name,
                CostPrice = product.CostPrice,
                SalePrice = product.SalePrice,
                Brand = new BrandInProductGetDto
                {
                    Id = product.Id,
                    Name = product.Brand.Name,
                }
            };
            return Ok(productDto);*/
            return Ok(_productService.GetById(id));
        }
        [HttpGet("all")]
        public ActionResult<List<ProductGetAllItemDto>> GetAll()
        {
            /*var productDtos = _productRepository.GetQueryable(x => true, "Brand").Select(x => new ProductGetAllItemDto { Id = x.Id, Name = x.Name, BrandName = x.Brand.Name }).ToList();
            return Ok(productDtos);*/
            return Ok(_productService.GetAll());
        }
        /*[HttpPut("{id}")]
        public IActionResult Edit(int id, ProductPutDto putDto)
        {
            Product product = _productRepository.Get(x => x.Id == id);
            if (product == null) return NotFound();

            if (product.Name != putDto.Name && _productRepository.IsExist(x => x.Name == putDto.Name))
            {
                ModelState.AddModelError("Name", "Name is already exit");
                return BadRequest(ModelState);
            }
            product.Name = putDto.Name;
            _productRepository.Commit();

            return NoContent();
        }*/
        /*[HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _productRepository.Get(x => x.Id == id);
            if (product == null) return NotFound();

            _productRepository.Remove(product);
            _productRepository.Commit();

            return NoContent();
        }*/
        [HttpPut("{id}")]
        public IActionResult Edit(int id, ProductPutDto putDto)
        {
            _productService.Edit(id, putDto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return NoContent();
        }
    }
}
