namespace BlockedCountries.Interfaces
{
    public interface ITemporaryBlockRepository
    {
        bool AddTemporalBlock(string countryCode, DateTime expiryTime);
        bool RemoveTemporalBlock(string countryCode);
        IEnumerable<KeyValuePair<string, DateTime>> GetTemporalBlocks();
        bool IsCountryBlocked(string countryCode);
        public void RemoveExpiredBlocks();
    }
}
