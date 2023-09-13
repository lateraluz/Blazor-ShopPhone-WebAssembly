using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using FluentValidation;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using ShopPhone.DataAccess;
using ShopPhone.Repositories.Implementations;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Server.Controllers;
using ShopPhone.Services.Implementations;
using ShopPhone.Services.Interfaces;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ShopPhone.Test.UnitTest.Repository;

public class CategoriaRepositoryTest
{
    private ICategoriaRepository _categoriaRepository;
    private readonly IMapper _mapper;
    private ILogger<CategoriaRepository> _logger;
    private readonly ITestOutputHelper _output;

    public CategoriaRepositoryTest(ITestOutputHelper output)
    {
        _categoriaRepository = A.Fake<ICategoriaRepository>();
        _mapper = A.Fake<IMapper>();
        _logger = A.Fake<ILogger<CategoriaRepository>>();
        _output = output;
    }

    public async Task<ShopPhoneContext> GetDataBaseContext()
    {

        var options = new DbContextOptionsBuilder<ShopPhoneContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .EnableSensitiveDataLogging()
                            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                            .Options;
        var databaseContext = new ShopPhoneContext(options);
        databaseContext.Database.EnsureCreated();

        // Add temp data
        databaseContext.Categoria.Add(new Categorium() { IdCategoria = 1, Estado = true, NombreCategoria = "Categoria 1" });
        databaseContext.Categoria.Add(new Categorium() { IdCategoria = 2, Estado = true, NombreCategoria = "Categoria 2" });
        databaseContext.Categoria.Add(new Categorium() { IdCategoria = 3, Estado = true, NombreCategoria = "Categoria 3" });

        await databaseContext.SaveChangesAsync();
        return databaseContext;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(-1)]
    public async void CategoriaRepository_FindAsync(int id)
    {

        //Arrange
        var context = await GetDataBaseContext();
        var categoriaRepository = new CategoriaRepository(context, _logger);

        // Act
        var result = await categoriaRepository.FindAsync(id);

        // Acert
        if (id == -1)
        {
            result.Should().BeNull($"{id} must  be null");
        }
        else
        {
            result.Should().NotBeNull($"{id} must be MANDATORY");
        }
    }

    [Fact]
    public async void CategoriaRepository_AddAsync()
    {
        //Arrange
        var context = await GetDataBaseContext();
        var categoriaRepository = new CategoriaRepository(context, _logger);
        Random rnd = new Random();
        int id = rnd.Next();
        Categorium categorium = new Categorium()
        {
            IdCategoria = id,
            Estado = true,
            NombreCategoria = "Categoria Duplicada"
        };

        // Act
        var result = await categoriaRepository.AddAsync(categorium);

        // Acert
        result.Should().Be(id,$"Because it is a new Category id = {id}");
    }



    [Fact]
    public async void CategoriaRepository_Return_ListAsync()
    {

        //Arrange
        var context = await GetDataBaseContext();
        var categoriaRepository = new CategoriaRepository(context, _logger);

        // Act
        var result = await categoriaRepository.ListAsync();

        // Acert
        result.Should().HaveCountGreaterThan(1,$"Because Must be > 1");
        result.Should().NotBeNullOrEmpty("Because must be mandatory");

        _output.WriteLine("Categoria Rows Count {0}", result.Count);
    }


    [Fact]
    public async void CategoriaRepository_Return_LessThan500ms()
    {

        //Arrange
        var context = await GetDataBaseContext();
        var categoriaRepository = new CategoriaRepository(context, _logger);

        // Act
        var result = await categoriaRepository.ListAsync();

        // Acert
        categoriaRepository.ExecutionTimeOf(s => s.ListAsync().Wait()).Should().BeLessThanOrEqualTo(500.Milliseconds(),"Because must last <= 500 ms");

    }


}
