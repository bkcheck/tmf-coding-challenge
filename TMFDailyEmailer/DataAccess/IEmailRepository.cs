using System.Collections.Generic;
using System.Threading.Tasks;

namespace TMFDailyEmailer.DataAccess
{
    /// <summary>
    /// Provides methods for interfacing with the MailChamp API.
    /// </summary>
    public interface IEmailRepository
    {
        /// <summary>
        /// Sends an email batch to the provided mailing ID.
        /// </summary>
        /// <param name="mailingId">The MailChamp mailing list ID.</param>
        /// <param name="emailSendRecords">A list of Dictionaries. Each dictionary contains parameters for one email.</param>
        Task SendEmails(int mailingId, IEnumerable<Dictionary<string, string>> emailSendRecords);

        /// <summary>
        /// Updates a MailChimp email template.
        /// </summary>
        /// <param name="templateId">The ID of the template to update.</param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task UpdateTemplate(int templateId, string content);
    }
}
