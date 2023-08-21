using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Interfaces;

public interface IClienteService
{
    Task<BaseResponseGeneric<ICollection<ClienteDTO>>> FindByDescriptionAsync(string description);
    Task<BaseResponseGeneric<ICollection<ClienteDTO>>> ListAsync();
    Task<BaseResponseGeneric<ICollection<ClienteDTO>>> FindByIdAsync(int id);
    Task<BaseResponseGeneric<int>> AddAsync(ClienteDTO identity);
    Task<BaseResponse> DeleteAsync(int id);
    Task<BaseResponse> UpdateAsync(int id, ClienteDTO request);
}
