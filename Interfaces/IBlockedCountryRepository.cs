using BlockedCountries.DTOs.Responses;

namespace BlockedCountries.Interfaces
{
    public interface IBlockedCountryRepository
    {
        bool AddBlockedCountry(string countryCode);
        bool RemoveBlockedCountry(string countryCode);
        IEnumerable<string> GetBlockedCountries();
        bool IsCountryBlocked(string countryCode);

    }
}
