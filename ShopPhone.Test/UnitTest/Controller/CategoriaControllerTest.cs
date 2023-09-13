using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Caching.Memory;
using ShopPhone.Services.Interfaces;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using ShopPhone.Server.Controllers;
using ShopPhone.Shared.Response;
using Serilog;
using Serilog.Events;
using k8s.Models;
using ShopPhone.Services.Implementations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.OidcClient;
using Microsoft.CodeAnalysis.CodeActions;
using ShopPhone.Server.Validators;
using Microsoft.AspNetCore.Http;

namespace ShopPhone.Test.UnitTest.Controller;

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
        var response = A.Fake<BaseResponseGeneric<ICollection<CategoriaDTO>>>();
        response.Data = GetListCategoria();

        //Act – perform the actual work of the test.
        A.CallTo(() => _categoriaService.ListAsync()).Returns(response);
        IActionResult actionResult = await _categoriaController.ListAsync();
        var returnedValues = (BaseResponseGeneric<ICollection<CategoriaDTO>>)((ObjectResult)actionResult).Value!;

        //Assert – verify the result.
        returnedValues.Should().NotBeNull();
        returnedValues.Data.Should().BeOfType(typeof(List<CategoriaDTO>), "because is a List of CategoriaDTO");
        returnedValues.Data.Should().HaveCountGreaterThan(1);
    }

    [Fact]
    public void GetCategoriaListAsync_Execution_Time()
    {
        // Arrange – setup the testing objects and prepare the prerequisites for your test.
        var response = A.Fake<BaseResponseGeneric<ICollection<CategoriaDTO>>>();
        response.Data = GetListCategoria();

        //Act – perform the actual work of the test.        
        Func<Task> action = async () => await _categoriaController.ListAsync();

        //Assert – verify the result.
        action.ExecutionTime().Should().BeLessThanOrEqualTo(200.Milliseconds(),"Because is a must < 200 Ms response");
    }



    /*
    [Fact]
    public async Task AddCategoria()
    {
        // Arrange – setup the testing objects and prepare the prerequisites for your test.
        var response = A.Fake<BaseResponseGeneric<ICollection<CategoriaDTO>>>();
        var fakeResponse = A.Fake<BaseResponseGeneric<int>>();
        response.Data = GetListCategoria();

        CategoriaDTO categoria = A.Fake<CategoriaDTO>();

        categoria.IdCategoria = 1;
        categoria.NombreCategoria = "Categoria 1";
        categoria.Estado = true;

        A.CallTo(() => _categoriaService.AddAsync(categoria));
        //Act – perform the actual work of the test.
        IActionResult actionResult = await _categoriaController.Post(categoria);

        //A.CallTo(() => _categoriaService.ListAsync()).Returns(response);
        //IActionResult actionResult = await _categoriaController.ListAsync();
        //var returnedValues = (BaseResponseGeneric<ICollection<CategoriaDTO>>)((ObjectResult)actionResult).Value!;

        //Assert – verify the result.
        actionResult.Should().NotBeNull();
        //returnedValues.Should().NotBeNull();
    }
    */
    private List<CategoriaDTO> GetListCategoria()
    {
        return new List<CategoriaDTO>() {
                        new CategoriaDTO() { IdCategoria = 1, NombreCategoria = "test1" },
                        new CategoriaDTO() { IdCategoria = 2, NombreCategoria = "test2" },
                        new CategoriaDTO() { IdCategoria = 3, NombreCategoria = "test3" }
        };
    }
}
