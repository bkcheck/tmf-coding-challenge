using System.Collections.Generic;
using System.Threading.Tasks;

namespace TMFDailyEmailer
{
    public interface IDailyEmailCoordinator
    {
        Task<IEnumerable<string>> BatchSendWatchlistEmails(int batchSize = 200);
    }
}