namespace BlockedCountries.DTOs.Responses
{
    public class TemporaryBlockedCountryResponse
    {
        public string CountryCode { get; set; } = string.Empty;
        public DateTime BlockedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int DurationMinutes { get; set; }
        public int MinutesRemaining { get; set; }
    }
}
