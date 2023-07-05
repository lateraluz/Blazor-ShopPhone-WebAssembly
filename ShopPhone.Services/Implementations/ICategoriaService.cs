using ShopPhone.DataAccess;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Services.Implementations;

public interface ICategoriaService
{
    Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByDescriptionAsync(string description);
    Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> ListAsync();
    Task<BaseResponseGeneric<ICollection<CategoriaDTO>>> FindByIdAsync(int id);
    Task<BaseResponseGeneric<int>> AddAsync(CategoriaDTO identity);
    Task<BaseResponse> DeleteAsync(int  id);
    Task<BaseResponse> UpdateAsync(int id, CategoriaDTO request);

}
