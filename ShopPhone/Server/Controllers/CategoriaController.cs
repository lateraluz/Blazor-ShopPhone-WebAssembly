using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;
using log4net;
using System.Reflection;
using System.Text;
using ShopPhone.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using ShopPhone.Shared.Response;
using ShopPhone.DataAccess;

namespace ShopPhone.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        private ICategoriaService _CategoriaService;
        private ILog _Logger;
        public CategoriaController(ICategoriaService CategoriaService, ILog Logger)
        {
            _CategoriaService = CategoriaService;
            _Logger = Logger;
        }

        [HttpGet("FindByDescription")]
        public async Task<IActionResult> FindByDescriptionAsync(string description)
        {
            try
            {
                var response = await _CategoriaService.FindByDescriptionAsync(description);

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                string msg = UtilLog.Error(ex, MethodBase.GetCurrentMethod()!);
                _Logger.Error(msg, ex);
                throw;
            }
        }


        [HttpPut("{id:int}")]

        public async Task<IActionResult> Put(int id, CategoriaDTO request)
        {
            await _CategoriaService.UpdateAsync(id, request);

            return Ok();
        }

        [HttpGet("FindById")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var response = await _CategoriaService.FindByIdAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                string msg = UtilLog.Error(ex, MethodBase.GetCurrentMethod()!);
                _Logger.Error(msg, ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoriaDTO request)
        {
            try
            {
                var response = await _CategoriaService.AddAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string msg = UtilLog.Error(ex, MethodBase.GetCurrentMethod()!);
                _Logger.Error(msg, ex);
                throw;
            }
        }



        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _CategoriaService.DeleteAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                string msg = UtilLog.Error(ex, MethodBase.GetCurrentMethod()!);
                _Logger.Error(msg, ex);
                throw;
            }
        }



    }
}
