using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Interfaces
{
    public interface IVentaService
    {

        Task<BaseResponseGeneric<ICollection<FacturaDTO>>> FindByIdAsync(int id);

        Task<BaseResponseGeneric<ICollection<FacturaDTO>>> ListAsync();
        Task<BaseResponseGeneric<int>> AddAsync(FacturaDTO identity);
        Task<BaseResponse> UpdateAsync(int id, FacturaDTO request);
    }
}
