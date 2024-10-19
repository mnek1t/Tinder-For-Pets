using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using SharedKernel;
using TinderForPets.Core;

namespace TinderForPets.Application.Services
{
    public class GeocodingService
    {
        private readonly string? geocodingApiUrl;
        private readonly HttpClient _httpClient;
        public GeocodingService(IConfiguration configuration, HttpClient httpClient)
        {
            geocodingApiUrl = configuration["GeocodingService:Url"];
            _httpClient = httpClient;
        }

        public async Task<Result<(decimal latitude, decimal longitude)>> GetLocationCoordinates(string city, string country) 
        {
            if (geocodingApiUrl.IsNullOrEmpty())
            {
                return Result.Failure<(decimal latitude, decimal longitude)>(ApiErrors.InvalidRequestUrl);
            }

            _httpClient.BaseAddress = new Uri(geocodingApiUrl);
            var requestUri = $"{geocodingApiUrl}?city={city}&country={country}&format=json";
            _httpClient.DefaultRequestHeaders.Add(
            HeaderNames.UserAgent, "GeocodingApiApp (mednikita2004@gmail.com)");

            HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JArray json = JArray.Parse(jsonResponse);
                if (json.Count > 0)
                {
                    decimal latitude = (decimal)json[0]["lat"];
                    decimal longitude = (decimal)json[0]["lon"];
                    return Result.Success<(decimal latitude, decimal longitude)>((latitude, longitude));
                }
                else
                {
                    return Result.Failure<(decimal latitude, decimal longitude)>(ApiErrors.ResponseWereNotReadCorrect);
                }
            }
            else 
            {
                return Result.Failure<(decimal latitude, decimal longitude)>(ApiErrors.FailedResponse(response.StatusCode.ToString(), response.Content.ToString()));
            }
        }
    }
}
