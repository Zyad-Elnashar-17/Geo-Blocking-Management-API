using BlockedCountries.DTOs.Responses;
using BlockedCountries.Interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.Repositories
{
    public class BlockAttemptLogRepository : IBlockAttemptLogRepository
    {
        private readonly ConcurrentBag<BlockAttemptLogResponse> _logs = new();
        public void AddLog(BlockAttemptLogResponse log)
        => _logs.Add(log);

        public IEnumerable<BlockAttemptLogResponse> GetLogs()
        => _logs.OrderByDescending(l => l.Timestamp);
    }
}
