using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using TMFDailyEmailer.DTO;

namespace TMFDailyEmailer.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        private const string _getUserBatchQuery = @"
SELECT
    U.UserId,
    TRIM(U.Email) AS Email,
    TRIM(U.FirstName) AS FirstName,
    TRIM(U.LastName) AS LastName,
    STRING_AGG(UWI.InstrumentId, ',') AS WatchedInstrumentIds
FROM dbo.[User] U
INNER JOIN dbo.UserEmailSubscription UES ON UES.UserId = U.UserId
LEFT JOIN dbo.UserWatchedInstrument UWI ON U.UserId = UWI.UserId
WHERE UES.EmailSubscriptionId = {1}
{0}
GROUP BY U.UserId, U.Email, U.FirstName, U.LastName
ORDER BY UserId ASC
OFFSET {2} ROWS
FETCH NEXT {3} ROWS ONLY";

        private const string _getUserBatchQueryLegacy = @"
SELECT
  U.UserId,
  TRIM(U.Email) AS Email,
  TRIM(U.FirstName) AS FirstName,
  TRIM(U.LastName) AS LastName,
  STUFF((
    SELECT ',' + CAST(InstrumentId AS varchar) FROM UserWatchedInstrument
    WHERE UserId = U.UserId
{0}
    FOR XML PATH('')
  ), 1, 1, '') AS WatchedInstrumentIds
FROM dbo.[User] U
  INNER JOIN dbo.UserEmailSubscription UES ON UES.UserId = U.UserId
  LEFT JOIN dbo.UserWatchedInstrument UWI ON U.UserId = UWI.UserId
WHERE UES.EmailSubscriptionId = {1}
{0}
GROUP BY U.UserId, U.Email, U.FirstName, U.LastName
ORDER BY UserId ASC
OFFSET {2} ROWS
FETCH NEXT {3} ROWS ONLY";

        /// <param name="connectionString">The connection string of the user email subscription DB.</param>
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public IEnumerable<UserEmailSubscription> GetUserBatch(int subscriptionId, IEnumerable<int> includeInstrumentIds = null, int offset = 0, int rows = 200)
        {
            IEnumerable<UserEmailSubscription> subscriptions = new List<UserEmailSubscription>();
            string instrumentFilterClause = null;

            if (includeInstrumentIds != null && includeInstrumentIds.Any())
            {
                var instrumentIdString = string.Join(',', includeInstrumentIds);
                instrumentFilterClause = $"AND InstrumentId IN ({instrumentIdString})";
            }

            var query = string.Format(_getUserBatchQueryLegacy, instrumentFilterClause, subscriptionId, offset, rows);

            using (var db = new SqlConnection(_connectionString))
            {
                subscriptions = db.Query<UserEmailSubscription>(query);
            }

            return subscriptions;
        }
    }
}
