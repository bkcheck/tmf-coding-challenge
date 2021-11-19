using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMFDailyEmailer.DTO;

namespace TMFDailyEmailer.DataAccess
{
    /// <summary>
    /// Provides methods for retrieving data from the Article API.
    /// </summary>
    public interface IArticleRepository
    {
        /// <summary>
        /// Retrieves a list of published articles within the given time frame.
        /// </summary>
        /// <param name="from">The beginning of the time frame. Defaults to 5 PM yesterday.</param>
        /// <param name="to">The end of the time frame. Defaults to 5 PM today.</param>
        Task<IEnumerable<Article>> GetArticles(DateTime? from = null, DateTime? to = null);
    }
}
