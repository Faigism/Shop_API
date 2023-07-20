using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ShopApp.Core.Entities;
using ShopApp.Core.Repositories;
using ShopApp.Service.Dtos.Common;
using ShopApp.Service.Dtos.ProductDtos;
using ShopApp.Service.Exceptions;
using ShopApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service.Implementations
{
    public class ProductService:IProductService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IBrandRepository brandRepository,IProductRepository productRepository,IMapper mapper)
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public PostResultDto Create(ProductPostDto postDto)
        {
            if (_brandRepository.IsExist(x => x.Id == postDto.BrandId))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "BrandId", $"Brand not found by id:{postDto.BrandId}");

            if (_productRepository.IsExist(x => x.Name == postDto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", $"Name already take");

            var entity = _mapper.Map<Product>(postDto);

            _productRepository.Add(entity);
            _productRepository.Commit();

            return new PostResultDto { Id = entity.Id };
        }

        public void Delete(int id)
        {
            var entity = _productRepository.Get(x => x.Id == id);

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Product not found by id: {id}");

            _productRepository.Remove(entity);
            _productRepository.Commit();
        }

        public void Edit(int id, ProductPutDto putDto)
        {
            var entity = _productRepository.Get(x => x.Id == id);

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Product not found by id: {id}");
            if (entity.BrandId != putDto.BrandId && !_brandRepository.IsExist(x => x.Id == putDto.BrandId))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "BrandId", "BrandId not found");
            if (entity.Name != putDto.Name && _productRepository.IsExist(x => x.Name == putDto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "name already taken");

            entity.Name = putDto.Name;
            entity.CostPrice = putDto.CostPrice;
            entity.SalePrice = putDto.SalePrice;
            entity.BrandId = putDto.BrandId;
            entity.ModifiedAt = DateTime.UtcNow.AddHours(4);

            _productRepository.Add(entity);
        }

        public List<ProductGetAllItemDto> GetAll()
        {
            var entities = _productRepository.GetQueryable(x => true, "Brand").ToList();

            return _mapper.Map<List<ProductGetAllItemDto>>(entities);
        }

        public ProductGetDto GetById(int id)
        {
            var entity = _productRepository.Get(x => x.Id == id, "Brand");
            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Product not found by id:{id}");

            /*return new ProductGetDto
            {
                Name = entity.Name,
                CostPrice = entity.CostPrice,
                SalePrice = entity.SalePrice,
                Brand = new BrandInProductGetDto
                {
                    Id = entity.Brand.Id,
                    Name = entity.Brand.Name,
                }
            }; bele bir obyekt duzeltmek evezine mapp -den ist. etmek daha sade prosesdir:*/
            return _mapper.Map<ProductGetDto>(entity);
        }
    }
}
