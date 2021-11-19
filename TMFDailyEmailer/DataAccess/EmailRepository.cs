using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TMFDailyEmailer.DataAccess
{
    public class EmailRepository : IEmailRepository
    {
        private readonly string _mailChampUri;
        private readonly HttpClient _httpClient;

        public EmailRepository(string mailChampUri, HttpClient httpClient)
        {
            _mailChampUri = mailChampUri;
            _httpClient = httpClient;
        }


        public async Task SendEmails(int mailingId, IEnumerable<Dictionary<string, string>> emailSendRecords)
        {
            var uri = $"{_mailChampUri}mailings/{mailingId}/send";
            var payload = new
            {
                records = emailSendRecords.ToList()
            };

            var response = await _httpClient.PostAsJsonAsync(uri, payload);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var stringContent = await response.Content.ReadAsStringAsync();
            var errMsg = $"Couldn't send email. Raw payload from API: {stringContent}";

            throw new HttpRequestException(errMsg, null, response.StatusCode);
        }

        public async Task UpdateTemplate(int templateId, string content)
        {
            var uri = $"{_mailChampUri}templates/{templateId}/content";
            var response = await _httpClient.PutAsJsonAsync(uri, new { content });

            if (response.IsSuccessStatusCode)
            {
                return; 
            }

            var stringContent = await response.Content.ReadAsStringAsync();
            var errMsg = $"Couldn't update template. Raw payload from API: {stringContent}";

            throw new HttpRequestException(errMsg, null, response.StatusCode);
        }
    }
}
