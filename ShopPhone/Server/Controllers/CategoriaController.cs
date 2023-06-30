using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;
using log4net;

namespace ShopPhone.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase    {

        private ICategoriaService _CategoriaService;
        private ILog  _Logger;
        public CategoriaController(ICategoriaService CategoriaService, ILog  Logger)
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
                _Logger.Error(ex);
                throw;
            }

           

        }


    }
}
