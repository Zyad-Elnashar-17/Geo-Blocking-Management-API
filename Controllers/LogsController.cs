using BlockedCountries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : Controller
    {
        private readonly IBlockAttemptLogRepository _logRepo;

        public LogsController(IBlockAttemptLogRepository logRepo)
        {
            _logRepo = logRepo;
        }
        [HttpGet]
        public IActionResult GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var allLogs = _logRepo.GetLogs();
            var totalCount = allLogs.Count();

            var data = allLogs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Data = data,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }
    }
}
