using System;
using Newtonsoft.Json;

namespace TMFDailyEmailer.DTO
{
    public class Article
    {
        [JsonProperty("article_id")]
        public int ArticleId { get; set; }

        [JsonProperty("date_published")]
        public DateTime DatePublished { get; set; }

        [JsonProperty("perma_link")]
        public string Permalink { get; set; }

        public string Headline { get; set; }

        public string Byline { get; set; }

        public ArticleInstrument[] Instruments { get; set; }

        [JsonProperty("date_created")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("date_modified")]
        public DateTime DateModified { get; set; }

        public string Slug { get; set; }

        public Author[] Authors { get; set; }
    }
}
