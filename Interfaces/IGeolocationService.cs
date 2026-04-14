using BlockedCountries.DTOs.Responses;

namespace BlockedCountries.Interfaces
{
    public interface IGeolocationService
    {
        Task<IpLookupResponse?> GetCountryInfoAsync(string ipAddress);
    }
}
