using BlockedCountries.DTOs.Responses;
using BlockedCountries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpController : Controller
    {
        private readonly IGeolocationService _geoService;
        private readonly IBlockedCountryRepository _permanentRepo;
        private readonly ITemporaryBlockRepository _tempRepo;
        private readonly IBlockAttemptLogRepository _logRepo;

        public IpController(IGeolocationService geoService, IBlockedCountryRepository permanentRepo, ITemporaryBlockRepository tempRepo, IBlockAttemptLogRepository logRepo)
        {
            _geoService = geoService;
            _permanentRepo = permanentRepo;
            _tempRepo = tempRepo;
            _logRepo = logRepo;
        }
        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock([FromQuery] string? testIp = null)
        {
            var ip = testIp ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ip) || ip == "::1" || ip == "127.0.0.1")
            {
                return BadRequest(new
                {
                    Message = "Localhost detected. Please provide a test IP in the query string to verify blocking logic.",
                    Example = "/api/ip/check-block?testIp=8.8.8.8"
                });
            }

            var info = await _geoService.GetCountryInfoAsync(ip);
            if (info == null) return StatusCode(500, "Could not determine location.");

            bool isPermanent = _permanentRepo.IsCountryBlocked(info.CountryCode.ToUpper());
            bool isTemp = _tempRepo.IsCountryBlocked(info.CountryCode.ToUpper());
            bool isBlocked = isPermanent || isTemp;

            _logRepo.AddLog(new BlockAttemptLogResponse
            {
                IpAddress = ip,
                CountryCode = info.CountryCode,
                IsBlocked = isBlocked,
                Timestamp = DateTime.UtcNow,
                UserAgent = Request.Headers.ContainsKey("User-Agent") ? Request.Headers["User-Agent"].ToString() : "Unknown"
            });

            return Ok(new IpBlockCheckResponse
            {
                IpAddress = ip,
                CountryCode = info.CountryCode,
                IsBlocked = isBlocked,
                BlockType = isPermanent ? "Permanent" : (isTemp ? "Temporary" : "None")
            });
        }
        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            var targetIp = ipAddress ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(targetIp) || targetIp == "::1")
            {
                return BadRequest("Please provide a valid public IP address.");
            }

            var info = await _geoService.GetCountryInfoAsync(targetIp);
            return Ok(info);
        }
    }
}
