namespace BlockedCountries.DTOs.Responses
{
    public class IpApiResponse
    {
        public string? Ip { get; set; }
        public string? Country_Code { get; set; }
        public string? Country_Name { get; set; }
        public string? Org { get; set; }
        public string? Error { get; set; }
        public bool Reserved { get; set; }
    }
}
