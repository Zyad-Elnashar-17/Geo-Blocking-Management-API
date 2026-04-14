using System.ComponentModel.DataAnnotations;

namespace BlockedCountries.DTOs.Requests
{
    public class TemporalBlockRequest
    {
        [Required(ErrorMessage = "CountryCode is required.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "CountryCode must be exactly 2 characters (ISO 3166-1 alpha-2).")]
        [RegularExpression(@"^[A-Za-z]{2}$", ErrorMessage = "CountryCode must contain only letters.")]
        public string CountryCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "DurationMinutes is required.")]
        [Range(1, 1440, ErrorMessage = "DurationMinutes must be between 1 and 1440 (24 hours).")]
        public int DurationMinutes { get; set; }
    }
}
