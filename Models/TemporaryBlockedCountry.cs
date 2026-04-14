namespace BlockedCountries.Models
{
    public class TemporaryBlockedCountry
    {
        public string CountryCode { get; set; } = string.Empty;
        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public int DurationMinutes { get; set; }
    }
}
