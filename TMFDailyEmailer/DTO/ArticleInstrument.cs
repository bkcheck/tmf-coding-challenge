using System;
using Newtonsoft.Json;

namespace TMFDailyEmailer.DTO
{
    public class ArticleInstrument
    {
        [JsonProperty("instrument_id")]
        public int InstrumentId { get; set; }

        public string Symbol { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("date_created")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("date_modified")]
        public DateTime DateModified { get; set; }
    }
}
