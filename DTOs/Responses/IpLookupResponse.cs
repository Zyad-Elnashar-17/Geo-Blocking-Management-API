namespace BlockedCountries.DTOs.Responses
{
    public class IpLookupResponse
    {
        public string IpAddress { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string Isp { get; set; } = string.Empty;
    }
}
