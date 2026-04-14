using BlockedCountries.DTOs.Responses;
using BlockedCountries.Interfaces;

namespace BlockedCountries.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;

        public GeolocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IpLookupResponse?> GetCountryInfoAsync(string ipAddress)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IpApiResponse>($"{ipAddress}/json/");

                if (response == null || !string.IsNullOrEmpty(response.Error))
                    return null;

                return new IpLookupResponse
                {
                    IpAddress = response.Ip ?? ipAddress,
                    CountryCode = response.Country_Code ?? "Unknown",
                    CountryName = response.Country_Name ?? "Unknown",
                    Isp = response.Org ?? "Unknown"
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
