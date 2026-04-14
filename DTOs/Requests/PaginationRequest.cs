using System.ComponentModel.DataAnnotations;

namespace BlockedCountries.DTOs.Requests
{
    public class PaginationRequest
    {

        [Range(1, int.MaxValue, ErrorMessage = "Page must be at least 1.")]
        public int Page { get; set; } = 1;

        [Range(1, 20, ErrorMessage = "PageSize must be between 1 and 20.")]
        public int PageSize { get; set; } = 20;
    }
}
