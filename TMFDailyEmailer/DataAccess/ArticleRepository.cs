using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMFDailyEmailer.DTO;

namespace TMFDailyEmailer.DataAccess
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly string _articleApiUri;
        private readonly HttpClient _httpClient;


        /// <param name="articleApiUri">The base uri of the Article API.</param>
        /// <param name="httpClient">An http client that will be used to call the API.</param>
        public ArticleRepository(string articleApiUri, HttpClient httpClient)
        {
            if (string.IsNullOrWhiteSpace(articleApiUri))
            {
                throw new ArgumentNullException(nameof(articleApiUri));
            }

            _articleApiUri = articleApiUri;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<IEnumerable<Article>> GetArticles(DateTime? from = null, DateTime? to = null)
        {
            var today = DateTime.Today;
            var endDate = to ?? new DateTime(today.Year, today.Month, today.Day, 17, 0, 0);
            var startDate = from ?? endDate.AddDays(-1);

            var pathAndQuery = $"{_articleApiUri}articles?publish_date_from={startDate:o}&publish_date_to={endDate:o}";
            var response = await _httpClient.GetAsync(pathAndQuery);
            var stringContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var articles = JsonConvert.DeserializeObject<List<Article>>(stringContent);
                return articles;
            }

            var errMsg = $"Couldn't retrieve articles. Raw payload from API: {stringContent}";
            throw new HttpRequestException(errMsg, null, response.StatusCode);
        }
    }
}
