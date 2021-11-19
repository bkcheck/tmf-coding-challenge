using System;
using System.Collections.Generic;
using TMFDailyEmailer.DTO;

namespace TMFDailyEmailer.DataAccess
{
    /// <summary>
    /// Provides methods for accessing users subscribed to email lists.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a list of users subscribed to the given email subscription.
        /// </summary>
        /// <param name="subscriptionId">The email subcription ID</param>
        /// <param name="includeInstrumentIds">If specified, the result set will only include users watching the given instruments.</param>
        /// <param name="offset">The number of users to skip (during a batching operation).</param>
        /// <param name="rows">The number of users to return.</param>
        /// <returns></returns>
        IEnumerable<UserEmailSubscription> GetUserBatch(int subscriptionId, IEnumerable<int> includeInstrumentIds = null, int offset = 0, int rows = 200);
    }
}
