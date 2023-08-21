using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Interfaces
{
    public interface IProductoService
    {

        Task<BaseResponseGeneric<ICollection<ProductoDTO>>> FindByDescriptionAsync(string description);
        Task<BaseResponseGeneric<ICollection<ProductoDTO>>> FindByIdAsync(int id);
        Task<BaseResponse> AddAsync(ProductoDTO identity);
        Task<BaseResponse> DeleteAsync(int id);
        Task<BaseResponse> UpdateAsync(int id, ProductoDTO request);
        Task<BaseResponseGeneric<ICollection<ProductoDTO>>> ListAsync();
    }
}
