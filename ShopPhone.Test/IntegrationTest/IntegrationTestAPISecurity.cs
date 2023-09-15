using FluentAssertions;
using FluentAssertions.Extensions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using ShopPhone.Shared.Request;
using ShopPhone.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ShopPhone.Test.IntegrationTest;




public class IntegrationTestAPISecurity : IClassFixture<WebApplicationFactory<Program>>
{

    private HttpClient _httpClient;
    private WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public IntegrationTestAPISecurity(ITestOutputHelper output)
    {
        _factory = new WebApplicationFactory<Program>();
        _httpClient = _factory.CreateDefaultClient();
        _output = output;
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("    ", "    ")]
    [InlineData("XXXXXXX", "")]
    [InlineData("", "YYYYYYYYYYYY")]
    [InlineData("NoExiste1", "NoExiste2")]
    [InlineData("admin", "123456*")]
    private async Task Security_Login_Valid_User(string usuario, string password)
    {
        string url = $"/api/Security/login";
        LoginRequestDTO request = new LoginRequestDTO()
        {
            UserName = usuario,
            Password = password
        };

        var httpResponseMessage = await _httpClient.PostAsJsonAsync(url, request);
        _output.WriteLine(httpResponseMessage.StatusCode.ToString());
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDTO>();


        switch (usuario, password)
        {
            case (string user, string pwd) when string.IsNullOrWhiteSpace(user) &&
                                             string.IsNullOrWhiteSpace(pwd):
                response!.Should().NotBeNull("Because response shouldn't be null");
                response!.ErrorMessage.Should().NotBeEmpty("Because there is a error");
                response.Token.Should().BeNullOrEmpty("There is a error, Token should be returned");
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
                break;

            case (string user, string pwd) when string.IsNullOrWhiteSpace(user) &&
                                             !string.IsNullOrWhiteSpace(pwd):
                response!.Should().NotBeNull("Because response shouldn't be null");
                response!.ErrorMessage.Should().NotBeEmpty("Because there is a error");
                response.Token.Should().BeNullOrEmpty("There is a error, Token should be returned");
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
                break;


            case (string user, string pwd) when !string.IsNullOrWhiteSpace(user) &&
                                             string.IsNullOrWhiteSpace(pwd):
                response!.Should().NotBeNull("Because response shouldn't be null");
                response!.ErrorMessage.Should().NotBeEmpty("Because there is a error");
                response.Token.Should().BeNullOrEmpty("\"Token must be empty");
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
                break;

            case (string user, string pwd) when user.Equals("noexiste1", StringComparison.CurrentCultureIgnoreCase) &&
                                             pwd.Equals("noexiste2", StringComparison.CurrentCultureIgnoreCase):
                response!.Should().NotBeNull("Because response shouldn't be null");
                response!.ErrorMessage.Should().NotBeEmpty("Because there is a error");
                response.Token.Should().BeNullOrEmpty("Token must be empty");
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
                break;

            case (string user, string pwd) when user.Contains("admin", StringComparison.CurrentCultureIgnoreCase) &&
                                             pwd.Equals("123456*", StringComparison.CurrentCultureIgnoreCase):
                response!.Should().NotBeNull("Because response shouldn't be null");
                response!.ErrorMessage.Should().BeNullOrEmpty("Because  there isnt a error");
                response.Token.Should().NotBeEmpty("There is a error, Token should be returned");
                response.Token.Length.Should().BeGreaterThan(100, "There is a error, Token should be returned");
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
                break;
            default:
                break;
        }

    }


    private async Task Login_With_Valid_User()
    {
        string url = $"/api/Security/login";
        LoginRequestDTO request = new LoginRequestDTO()
        {
            UserName = "admin",
            Password = "123456*"
        };

        var httpResponseMessage = await _httpClient.PostAsJsonAsync(url, request);
        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<LoginResponseDTO>();
         
        response!.Should().NotBeNull("Because response shouldn't be null");
        response!.ErrorMessage.Should().BeNullOrEmpty("Because ErrorMessage must be empty");
        response.Token.Should().BeOfType<string>("because token is a string ");
        response.Token.Length.Should().BeGreaterThan(100);
    }
     
    [Fact]
    public async Task Security_Login_With_Valid_User_LastLessThan5Seconds()
    {
        await Task.FromResult(1);
        Func <Task> action = () => Login_With_Valid_User();       
        action.ExecutionTime().Should().BeLessThan(5000.Milliseconds(),"Because must last less than 5 seconds");
    }
}


