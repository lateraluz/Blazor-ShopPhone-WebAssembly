using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopPhone.Services.Implementations;

namespace ShopPhone.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

       private ICategoriaService _CategoriaService;

        public CategoriaController(ICategoriaService CategoriaService)
        {
            _CategoriaService  = CategoriaService;
        }

        [HttpGet("FindByDescription")]
        public async Task<IActionResult> FindByDescriptionAsync(string description)
        {
            var response = await _CategoriaService.FindByDescriptionAsync(description);
            return response.Success ? Ok(response) : NotFound(response);
        }


    }
}
