using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System.Net;
using System.Reflection;
using ILogger = Serilog.ILogger;
using FluentValidation;
using ShopPhone.Server.Extensions;

namespace ShopPhone.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [EnableRateLimiting("concurrency")]
    public class ProductoController : ControllerBase
    {
        private IProductoService _productoService;
        private ILogger<IProductoService> _logger;
        private IValidator<ProductoDTO> _validator;
        public ProductoController(IProductoService service, 
                                  ILogger<IProductoService> logger,
                                  IValidator<ProductoDTO> validator)
        {
            _productoService = service;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet("FindByDescription")]
        public async Task<IActionResult> FindByDescriptionAsync(string description)
        {
            try
            {
                if (string.IsNullOrEmpty(description))
                {
                    var validResponse = new BaseResponse()
                    {
                        Success = false,
                        ErrorMessage = "La descripción es un dato requerido"
                    };

                    return Ok(validResponse);
                }

                var response = await _productoService.FindByDescriptionAsync(description);

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpGet("List")]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                var response = await _productoService.ListAsync();

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }


        [HttpPut("{id:int}")]

        public async Task<IActionResult> Put(int id, ProductoDTO request)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    BaseResponseGeneric<int> validResponse = new();
                    validResponse.Success = false;
                    validResponse.ErrorMessage = validationResult.ToListErrorsString();
                    return Ok(validResponse);
                }


                var response = await _productoService.UpdateAsync(id, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpGet("FindById")]
        public async Task<IActionResult> FindByIdAsync(int id)
        {
            try
            {
                var response = await _productoService.FindByIdAsync(id);
                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductoDTO request)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    BaseResponseGeneric<int> validResponse = new();
                    validResponse.Success = false;
                    validResponse.ErrorMessage = validationResult.ToListErrorsString();
                    return Ok(validResponse);
                }

                var response = await _productoService.AddAsync(request);

                return response.Success ? Ok(response) : NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }
         

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _productoService.DeleteAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}", ex);
                throw;
            }
        }
    }
}
