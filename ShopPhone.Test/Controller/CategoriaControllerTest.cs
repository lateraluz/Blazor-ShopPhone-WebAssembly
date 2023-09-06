using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using ShopPhone.Services.Interfaces;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ShopPhone.Server.Controllers;
using ShopPhone.Shared.Response;
using Serilog;
using Serilog.Events;
using k8s.Models;

namespace ShopPhone.Test.Controller;

public class CategoriaControllerTest
{

    private ICategoriaService _categoriaService;
    private ILogger<CategoriaController> _logger;
    private IMemoryCache _cache;
    private IValidator<CategoriaDTO> _validator;
    private CategoriaController _categoriaController;
    


    public CategoriaControllerTest()
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

    [Fact]
    public async Task GetCategoriaListAsync_Test()
    {
        // Arrange – setup the testing objects and prepare the prerequisites for your test.
        // BaseResponseGeneric<ICollection<CategoriaDTO>>  response ;
        //Act – perform the actual work of the test.

        //A.CallTo(()=> _categoriaService.ListAsync());
        //response = (BaseResponseGeneric<ICollection<CategoriaDTO>>)(await _categoriaController.ListAsync());

        //Assert – verify the result.
        //response.Should().NotBeNull();

        // Error
        //response.Should().BeOfType(typeof(BaseResponseGeneric<ICollection<CategoriaDTO>>));

    }
}
