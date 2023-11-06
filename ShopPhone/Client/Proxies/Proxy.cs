using ShopPhone.Shared.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using Blazored.SessionStorage;
using System.Net.Http;
using ShopPhone.Client.Auth;

namespace ShopPhone.Client.Proxies
{
    public class Proxy
    {

        private readonly HttpClient _httpClient;
        private ISessionStorageService _sessionStorage;
        private readonly string _url = $"/api/Security/refresh";
        private readonly string _idStorage = "token";

        public Proxy(HttpClient httpClient, ISessionStorageService sessionStorage, string url, string idStorage)
        {
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
            _url = url;
            _idStorage = idStorage;
        }



        public async Task RefreshToken()
        {
            // string url = $"/api/Security/refresh";
            string url = _url;
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {

                var (isTokenOutofTime, token) = await GetTokenStorage(_idStorage);

                if (!isTokenOutofTime && string.IsNullOrEmpty(token))
                {
                    return;
                }
                RefreshTokenDTO refreshToken = new RefreshTokenDTO()
                {
                    Token = token
                };

                //request.UserName = null;
                httpResponseMessage = await _httpClient.PostAsJsonAsync(url, refreshToken);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = await httpResponseMessage.Content.ReadFromJsonAsync<RefreshTokenDTO>() ?? throw new Exception("Error de conexión en Login");

                    if (response.Success)
                    {
                        await _sessionStorage.SetItemAsync<string>(_idStorage, response.Token);
                        // Testing
                       
                        if (!response.Token.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                        {
                            await _sessionStorage.SetItemAsync<string>("OLDtoken" + DateTime.Now.ToString("hh_mm_ss").Trim(), "Diferentes "+ token);
                        }
                       
                        return;
                    }
                    else
                    {
                        throw new Exception("Error de Seguridad Refresh Token!");
                    }

                }
                else
                {
                    throw new Exception("Error de Seguridad Refresh Token!");
                }

            }
            catch (Exception e)
            {
                Exception ex = e;
                throw;
            }
        }


        private async Task<(bool, string)> GetTokenStorage(string tokenId)
        {

            DateTimeOffset dateTimeOffset = new DateTimeOffset();
            var token = await _sessionStorage.GetItemAsync<string>(tokenId);

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Error en Seguridad!");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var unixDateTime = jwtToken.Claims.ToList().Find(p => p.Type == "exp")!.Value!;

            if (string.IsNullOrEmpty(unixDateTime))
            {
                throw new Exception("Error en Seguridad!");
            }

            dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixDateTime));

            /* Test Refresh 
            if (dateTimeOffset.DateTime > DateTime.Now && dateTimeOffset.DateTime.Subtract(DateTime.Now).TotalMinutes > 5)
            {
                return (false, "");
            }
            */
            return (true, token);

        }

    }
}
