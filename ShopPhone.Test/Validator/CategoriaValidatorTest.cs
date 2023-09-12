using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ShopPhone.Server.Controllers;
using ShopPhone.Server.Validators;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPhone.Test.Validator;

public class CategoriaValidatorTest
{

    private ICategoriaService _categoriaService;
    private ILogger<CategoriaController> _logger;
    private IMemoryCache _cache;
    private IValidator<CategoriaDTO> _validator;
    private CategoriaController _categoriaController;

    public CategoriaValidatorTest()
    {
        _categoriaService = A.Fake<ICategoriaService>();
        _cache = A.Fake<IMemoryCache>();
        _validator = A.Fake<IValidator<CategoriaDTO>>();
        _logger = A.Fake<ILogger<CategoriaController>>();

        // SUT
        _categoriaController = new CategoriaController(_categoriaService,
                                                        _logger,
                                                        _cache,
                                                        _validator);
    }


    [Theory]
    [InlineData(1,1)]
    [InlineData(2,99)]
    public void CategoriaValidator_AddCategoria_Exist_or_Not(int id1, int id2)
    {
        // Arrange – setup the testing objects and prepare the prerequisites for your test.
        var dummyResponse = new BaseResponseGeneric<ICollection<CategoriaDTO>>();
        List<CategoriaDTO> list = new List<CategoriaDTO>()
                                                        {
                                                        new CategoriaDTO() { IdCategoria = id1,
                                                                            NombreCategoria = $"Categoria {id1}",
                                                                                Estado=false
                                                                            }
                                                        };
        dummyResponse.Data = list;

        if (id1 >= 2)
            dummyResponse.Data = null;

        // Dummy HttpContextAccessor
        HttpContextAccessor httpContext = (HttpContextAccessor)GetHttpContext("/dummy.com", "dummy.com");
        httpContext.HttpContext!.Request.Method = "POST";
        CategoriaDTO categoria = new CategoriaDTO();
        categoria.IdCategoria = id2;
        categoria.NombreCategoria = $"Categoria {id2}";
        categoria.Estado = true;

       
        A.CallTo(() => _categoriaService.FindByIdAsync(categoria.IdCategoria)).Returns(dummyResponse);

        //Act – perform the actual work of the test.        
        CategoriaValidator categoriaValidator = new CategoriaValidator(_categoriaService, httpContext);
        var response = categoriaValidator.TestValidateAsync(categoria);

        //Assert – verify the result.
        switch (id1)
        {
            case 1:
                response.Result.Errors.Count.Should().Be(1, $"Debe ser 1 porque el {id1} ya existe");
                break;

            case 2:
                response.Result.Errors.Count.Should().Be(0, $"Debe ser CERO");
                break;

            default:
                break;
        }

    }

    [Theory]    
    [InlineData(1, "")]
    [InlineData(-1111, "Categoria Prueba con código negativo")]
    [InlineData(1, "Categoria Prueba XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX")]
    public void CategoriaValidator_Categoria_MandatoryFields(int id, string nombreCategoria)
    {
        // Arrange – setup the testing objects and prepare the prerequisites for your test.
        var dummyResponse = new BaseResponseGeneric<ICollection<CategoriaDTO>>();
        List<CategoriaDTO> list = new List<CategoriaDTO>()
                                                        {
                                                        new CategoriaDTO() { IdCategoria = id,
                                                                             NombreCategoria = nombreCategoria,
                                                                             Estado=false
                                                                            }
                                                        };
        dummyResponse.Data = null; 

        // Dummy HttpContextAccessor
        HttpContextAccessor httpContext = (HttpContextAccessor)GetHttpContext("/dummy.com", "dummy.com");
        httpContext.HttpContext!.Request.Method = "POST";
        CategoriaDTO categoria = new CategoriaDTO();
        categoria.IdCategoria = id;
        categoria.NombreCategoria = nombreCategoria;
        categoria.Estado = true;
        A.CallTo(() => _categoriaService.FindByIdAsync(categoria.IdCategoria)).Returns(dummyResponse);

        //Act – perform the actual work of the test.        
        CategoriaValidator categoriaValidator = new CategoriaValidator(_categoriaService, httpContext);
        var response = categoriaValidator.TestValidateAsync(categoria);

        //Assert – verify the result.
        response.Result.Errors.Count.Should().BeGreaterThan(0,$"Id = {id} Descripcion ={nombreCategoria}"); 

    }

    public IHttpContextAccessor GetHttpContext(string incomingRequestUrl, string host)
    {
        var context = new DefaultHttpContext();
        context.Request.Path = incomingRequestUrl;
        context.Request.Host = new HostString(host);

        //Do your thing here...

        var obj = new HttpContextAccessor();
        obj.HttpContext = context;
        return obj;
    }
}
