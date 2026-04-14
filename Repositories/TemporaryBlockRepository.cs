using BlockedCountries.Interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.Repositories
{
    public class TemporaryBlockRepository : ITemporaryBlockRepository
    {
        private readonly ConcurrentDictionary<string, DateTime> _temporalBlocks = new();
        public bool AddTemporalBlock(string countryCode, DateTime expiryTime)
        =>_temporalBlocks.TryAdd(countryCode.ToUpper(), expiryTime);

        public IEnumerable<KeyValuePair<string, DateTime>> GetTemporalBlocks()
        => _temporalBlocks.AsEnumerable();

        public bool IsCountryBlocked(string countryCode)
        => _temporalBlocks.ContainsKey(countryCode.ToUpper());

        public void RemoveExpiredBlocks()
        {
            var now = DateTime.UtcNow;
            var expiredCountryCodes = _temporalBlocks
                .Where(kvp => kvp.Value <= now)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var code in expiredCountryCodes)
            {
                _temporalBlocks.TryRemove(code, out _);
            }
        }

        public bool RemoveTemporalBlock(string countryCode)
        => _temporalBlocks.TryRemove(countryCode, out _);
    }
}
