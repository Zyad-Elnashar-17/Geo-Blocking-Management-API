namespace BlockedCountries.DTOs.Responses
{
    public class BlockedCountryResponse
    {
        public string CountryCode { get; set; } = string.Empty;
        public DateTime BlockedAt { get; set; }
    }
}
