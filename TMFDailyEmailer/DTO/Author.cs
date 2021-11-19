using Newtonsoft.Json;

namespace TMFDailyEmailer.DTO
{
    public class Author
    {
        [JsonProperty("author_id")]
        public int AuthorId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }
    }
}
