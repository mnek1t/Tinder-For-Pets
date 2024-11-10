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

        public async Task<Result<(double latitude, double longitude)>> GetLocationCoordinates(string city, string country, CancellationToken cancellationToken) 
        {
            try
            {
                if (geocodingApiUrl.IsNullOrEmpty())
                {
                    return Result.Failure<(double latitude, double longitude)>(ApiErrors.InvalidRequestUrl);
                }

                _httpClient.BaseAddress = new Uri(geocodingApiUrl);
                var requestUri = $"{geocodingApiUrl}?city={city}&country={country}&format=json";
                _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "GeocodingApiApp (mednikita2004@gmail.com)");

                HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JArray json = JArray.Parse(jsonResponse);
                    if (json.Count > 0)
                    {
                        double latitude = (double)json[0]["lat"];
                        double longitude = (double)json[0]["lon"];
                        return Result.Success<(double latitude, double longitude)>((latitude, longitude));
                    }
                    else
                    {
                        return Result.Failure<(double latitude, double longitude)>(ApiErrors.ResponseWereNotReadCorrect);
                    }
                }
                else
                {
                    return Result.Failure<(double latitude, double longitude)>(ApiErrors.FailedResponse(response.StatusCode.ToString(), response.Content.ToString()));
                }
            }
            catch (OperationCanceledException)
            {
                return Result.Failure<(double latitude, double longitude)>(new Error("400", "Operation canceled"));
            }
        }
    }
}
