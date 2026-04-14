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
        public async Task<IActionResult> CheckBlock()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip == "::1" || string.IsNullOrEmpty(ip) || ip == "127.0.0.1")
            {
                return BadRequest(new { Message = "Cannot lookup Localhost IP. Please deploy or provide an external IP." });
            }

            var info = await _geoService.GetCountryInfoAsync(ip);
            if (info == null) return StatusCode(500, "Could not determine location.");

            bool isPermanent = _permanentRepo.IsCountryBlocked(info.CountryCode);
            bool isTemp = _tempRepo.IsCountryBlocked(info.CountryCode);

            bool isBlocked = isPermanent || isTemp;

            _logRepo.AddLog(new BlockAttemptLogResponse
            {
                IpAddress = ip,
                CountryCode = info.CountryCode,
                IsBlocked = isBlocked,
                Timestamp = DateTime.UtcNow,
                UserAgent = Request.Headers["User-Agent"].ToString()
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
