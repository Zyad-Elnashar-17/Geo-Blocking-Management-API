using BlockedCountries.Interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.Repositories
{
    public class BlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, DateTime> _permanentBlocks = new();

        public bool AddBlockedCountry(string countryCode)
        
           => _permanentBlocks.TryAdd(countryCode.ToUpper(), DateTime.Now);
        

        public IEnumerable<string> GetBlockedCountries()
         => _permanentBlocks.Keys;

        public bool IsCountryBlocked(string countryCode)
        => _permanentBlocks.ContainsKey(countryCode.ToUpperInvariant());

        public bool RemoveBlockedCountry(string countryCode)
        => _permanentBlocks.TryRemove(countryCode.ToUpperInvariant(), out _);
    }
}
