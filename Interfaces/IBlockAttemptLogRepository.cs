using BlockedCountries.DTOs.Responses;
using BlockedCountries.Models;

namespace BlockedCountries.Interfaces
{
    public interface IBlockAttemptLogRepository
    {
        void AddLog(BlockAttemptLogResponse log);
        IEnumerable<BlockAttemptLogResponse> GetLogs();
    }
}
