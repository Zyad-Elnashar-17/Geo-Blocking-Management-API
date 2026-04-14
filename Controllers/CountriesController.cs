using BlockedCountries.DTOs.Requests;
using BlockedCountries.DTOs.Responses;
using BlockedCountries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : Controller
    {
        private readonly IBlockedCountryRepository _permanentRepo;
        private readonly ITemporaryBlockRepository _tempRepo;

        public CountriesController(IBlockedCountryRepository permanentRepo, ITemporaryBlockRepository tempRepo)
        {
            _permanentRepo = permanentRepo;
            _tempRepo = tempRepo;
        }
        [HttpPost("block")]
        public IActionResult BlockCountry([FromBody] BlockCountryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CountryCode))
                return BadRequest(new ApiErrorResponse { Message = "Country code is required." });

            var added = _permanentRepo.AddBlockedCountry(request.CountryCode);
            if (!added)
                return Conflict(new ApiErrorResponse { Message = "Country is already blocked." });

            return Ok(new { Message = $"Country {request.CountryCode.ToUpper()} blocked successfully." });
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            var removed = _permanentRepo.RemoveBlockedCountry(countryCode);
            if (!removed)
                return NotFound(new ApiErrorResponse { Message = "Country not found in blocked list." });

            return Ok(new { Message = $"Country {countryCode.ToUpper()} unblocked successfully." });
        }

        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;
            var allBlocked = _permanentRepo.GetBlockedCountries();

            if (!string.IsNullOrEmpty(search))
            {
                allBlocked = allBlocked.Where(c => c.Contains(search.ToUpper()));
            }

            var totalCount = allBlocked.Count();
            var data = allBlocked
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new BlockedCountryResponse { CountryCode = c, BlockedAt = DateTime.UtcNow }) 
                .ToList();

            return Ok(new PagedResponse<BlockedCountryResponse>
            {
                Data = data,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }

        [HttpPost("temporal-block")]
        public IActionResult TemporalBlock([FromBody] TemporalBlockRequest request)
        {
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest(new ApiErrorResponse { Message = "Duration must be between 1 and 1440 minutes." });

            var expiryTime = DateTime.UtcNow.AddMinutes(request.DurationMinutes);
            var added = _tempRepo.AddTemporalBlock(request.CountryCode, expiryTime);

            if (!added)
                return Conflict(new ApiErrorResponse { Message = "Country is already temporarily blocked." });

            return Ok(new { Message = $"Country {request.CountryCode.ToUpper()} blocked for {request.DurationMinutes} minutes." });
        }
    }
}
