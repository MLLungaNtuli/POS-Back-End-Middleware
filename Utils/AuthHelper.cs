using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utils
{
    public static class AuthHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // Method to get an OAuth 2.0 access token using client credentials
        public static async Task<string> GetOAuthTokenAsync()
        {
            var tokenUrl = "https://xxxx.keonn.com/advancloud/oauth/token";
            var clientId = "your-client-id";
            var clientSecret = "your-client-secret";
            var scope = "api.read";

            string accessToken = await AuthHelper.GetOAuthTokenAsync(tokenUrl, clientId, clientSecret, scope);
            //MY LOGS
            Console.WriteLine($"Access Token: {accessToken}");
            
            var requestBody = new StringContent(
                $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}&scope={scope}",
                Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(tokenUrl, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve token: {response.ReasonPhrase}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(jsonResponse);

            return tokenResponse?.AccessToken ?? throw new Exception("Access token not found in response.");
        }

        // Method to refresh an OAuth 2.0 token using a refresh token
        public static async Task<string> RefreshOAuthTokenAsync()
        {
            var refreshToken = "your-refresh-token";
            var tokenUrl = "https://xxxx.keonn.com/advancloud/oauth/token";
            var clientId = "your-client-id";
            var clientSecret = "your-client-secret";

            string newAccessToken = await AuthHelper.RefreshOAuthTokenAsync(tokenUrl, clientId, clientSecret, refreshToken);
            //MY LOGS
            Console.WriteLine($"New Access Token: {newAccessToken}");
            
            var requestBody = new StringContent(
                $"grant_type=refresh_token&client_id={clientId}&client_secret={clientSecret}&refresh_token={refreshToken}",
                Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(tokenUrl, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to refresh token: {response.ReasonPhrase}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(jsonResponse);

            return tokenResponse?.AccessToken ?? throw new Exception("Access token not found in response.");
        }

        // Method to attach the access token as a Bearer token to an HTTP request
        public static void AddBearerToken(HttpRequestMessage request)
        {
            var tokenUrl = "https://xxxx.keonn.com/advancloud/oauth/token";
            var request = new HttpRequestMessage(HttpMethod.Get, "https://xxxx.keonn.com/advancloud/resource");
            AuthHelper.AddBearerToken(request, accessToken);

            var response = await new HttpClient().SendAsync(request);
            //MY LOGS
            Console.WriteLine($"Response: {await response.Content.ReadAsStringAsync()}");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    // Model for OAuth token response
    public class OAuthTokenResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
