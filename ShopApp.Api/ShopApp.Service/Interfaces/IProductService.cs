using ShopApp.Service.Dtos.Common;
using ShopApp.Service.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service.Interfaces
{
    public interface IProductService
    {
        PostResultDto Create(ProductPostDto postDto);
        ProductGetDto GetById(int id);
        List<ProductGetAllItemDto> GetAll();
        void Edit(int id, ProductPutDto putDto);
        void Delete(int id);
    }
}
