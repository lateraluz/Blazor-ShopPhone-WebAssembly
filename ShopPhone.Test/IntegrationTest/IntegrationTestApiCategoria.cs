using Microsoft.AspNetCore.Mvc.Testing;
using ShopPhone.Shared.Response;
using System.Net.Http.Json;
using FluentAssertions;
using IdentityModel.Client;
using ShopPhone.Shared.Request;

namespace ShopPhone.Test.IntegrationTest;

public class IntegrationTestApiCategoria : IClassFixture<WebApplicationFactory<Program>>
{

    private string path = "api/Categoria/";
    private HttpClient _httpClient;
    private WebApplicationFactory<Program> _factory;
    private string _token;

    public  IntegrationTestApiCategoria()
    {
        _factory = new WebApplicationFactory<Program>();
        _httpClient = _factory.CreateDefaultClient();
        _token = GetToken().Result;        
    }

    
    private async Task<string> GetToken()
    {
        string url = $"/api/Security/login";
        LoginRequestDTO request = new LoginRequestDTO()
        {
            UserName = "admin",
            Password = "123456*"
        };

        var httpResponseMessage = await _httpClient.PostAsJsonAsync(url, request);
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDTO>();

        response!.Should().NotBeNull("Because response shouldn't be null");
        response!.ErrorMessage.Should().BeNullOrEmpty("Because ErrorMessage must be empty");
        response.Token.Should().BeOfType<string>("because token is a string ");
        response.Token.Length.Should().BeGreaterThan(600);
        return response!.Token;
    }


    [Fact]
    public async Task Categoria_Get_listAsync_GreaterthanZero()
    {
        var client = _factory.CreateClient();
        client.SetBearerToken(_token);
        var response = await client.GetFromJsonAsync<BaseResponseGeneric<ICollection<CategoriaDTO>>>(path + "list");

        response!.Data.Should().HaveCountGreaterThan(1,"Must > 1");
        response!.ErrorMessage.Should().BeNullOrEmpty("Because Error message must be null or empty");
        response!.Success.Should().BeTrue();
    }


    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(-1)]
    [InlineData(-50)]
    [InlineData(500)]
    public async Task Categoria_FindById_Must_Exist(int id)
    {
        var client = _factory.CreateClient();
        client.SetBearerToken(_token);
        var response = await client.GetFromJsonAsync<BaseResponseGeneric<ICollection<CategoriaDTO>>>(path + $"findbyid?id={id}");

        
        switch (id)
        {
            case int alias when alias >= 1 && alias <= 5:
                response!.Data.Should().HaveCount(1, $"Because {id} there is only 1 record");
                response!.ErrorMessage.Should().BeNullOrEmpty("Because Error message must be null or empty");
                response!.Success.Should().BeTrue($"{id} must be success");
                break;
            case int alias when alias <= -1 && alias <= 1000:
                response!.Data.Should().BeNull();
                response!.ErrorMessage.Should().BeNullOrEmpty("");
                response!.Success.Should().BeTrue();
                break;

            default:
                break;
        }

        
    }


}
