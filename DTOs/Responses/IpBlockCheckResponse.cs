namespace BlockedCountries.DTOs.Responses
{
    public class IpBlockCheckResponse
    {
        public string IpAddress { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public string BlockType { get; set; } = string.Empty;
    }
}
