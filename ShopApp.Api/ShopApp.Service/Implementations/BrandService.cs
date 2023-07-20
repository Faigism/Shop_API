using AutoMapper;
using ShopApp.Core.Entities;
using ShopApp.Core.Repositories;
using ShopApp.Service.Dtos.BrandDtos;
using ShopApp.Service.Dtos.Common;
using ShopApp.Service.Exceptions;
using ShopApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository,IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        public PostResultDto Create(BrandPostDto postDto)
        {
            if(_brandRepository.IsExist(x=>x.Name == postDto.Name))
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "Name already taken");
            }
            var entity = _mapper.Map<Brand>(postDto);
            _brandRepository.Add(entity);
            _brandRepository.Commit();

            return new PostResultDto { Id = entity.Id };
        }

        public void Edit(int id, BrandPutDto putDto)
        {
            var entity = _brandRepository.Get(x => x.Id == id);

            if(entity == null)
            {
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Brand not found by {id}");
            }
            if (entity.Name != putDto.Name && _brandRepository.IsExist(x => x.Name == putDto.Name))
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "Name already taken");
            }
            entity.Name = putDto.Name;
            _brandRepository.Commit();
        }

        public List<BrandGetAllItemDto> GetAll()
        {
            var dtos = _brandRepository.GetQueryable(x => true).ToList();

            return _mapper.Map<List<BrandGetAllItemDto>>(dtos);
        }

        public BrandGetDto GetById(int id)
        {
            var entity = _brandRepository.Get(x => x.Id == id, "Products");

            if(entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Brand not found by {id}");

            return _mapper.Map<BrandGetDto>(entity);
        }

        public void Remove(int id)
        {
            var entity = _brandRepository.Get(x => x.Id == id, "Products");

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Brand not found by {id}");

            _brandRepository.Remove(entity);
            _brandRepository.Commit();
        }
    }
}
