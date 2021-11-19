using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMFDailyEmailer.DataAccess;
using TMFDailyEmailer.DTO;
using TMFDailyEmailer.TemplateGeneration;

namespace TMFDailyEmailer
{
    public class DailyEmailCoordinator : IDailyEmailCoordinator
    {
        private readonly IArticleRepository _articleRepo;
        private readonly IUserRepository _userRepo;
        private readonly IEmailRepository _emailRepo;
        private readonly IArticleTemplateGenerator _templateGenerator;

        private const int DAILY_WATCHLIST_SUBSCRIPTION_ID = 12;
        private const int MAILCHAMP_TEMPLATE_ID = 9131;
        private const int MAILCHAMP_MAILING_ID = 2934;

        public DailyEmailCoordinator(
            IArticleRepository articleRepo,
            IUserRepository userRepo,
            IEmailRepository emailRepo,
            IArticleTemplateGenerator templateGenerator)
        {
            _articleRepo = articleRepo;
            _userRepo = userRepo;
            _emailRepo = emailRepo;
            _templateGenerator = templateGenerator;
        }

        /// <summary>
        /// Handles sending emails to all subscribers of the daily watchlist email.
        /// </summary>
        /// <param name="batchSize">An optional batch size between 10 and 1024. Defaults to 200.</param>
        /// <returns>A list of errors encountered while sending email batches.</returns>
        public async Task<IEnumerable<string>> BatchSendWatchlistEmails(int batchSize = 200)
        {
            if (batchSize < 10 || batchSize > 1024)
            {
                throw new ArgumentOutOfRangeException(nameof(batchSize), "batchSize must be between 10 and 1024.");
            }

            var instrumentIds = new List<int>();

            try
            {
                // Pull down the daily articles
                Console.WriteLine("EmailCoordinator: Fetching articles.");
                var articles = await _articleRepo.GetArticles();

                // Process articles to get updated template and instrument info
                Console.WriteLine("EmailCoordinator: Generating new watchlist template.");
                var templateGenerationResult = _templateGenerator.GenerateTemplate(articles);

                // Update template in MailChamp
                Console.WriteLine("EmailCoordinator: Updating template in MailChamp");
                await _emailRepo.UpdateTemplate(MAILCHAMP_TEMPLATE_ID, templateGenerationResult.TemplateContent);

                // Get the next user batch
                instrumentIds = templateGenerationResult.UniqueArticleInstruments.Keys.ToList();
                Console.WriteLine("EmailCoordinator: Starting user batching.");
            }
            catch (Exception ex)
            {
                throw new Exception($"EmailCoordinator: Encountered a fatal error. No emails were sent. Inner exception: {ex.Message}");
            }

            var usersProcessed = 0;
            var errorMessages = new List<string>();
            IEnumerable<UserEmailSubscription> currentBatch = new List<UserEmailSubscription>();

            do
            {
                Console.WriteLine($"EmailCoordinator: Starting user batch {usersProcessed} to {usersProcessed + batchSize}.");
                currentBatch = _userRepo.GetUserBatch(DAILY_WATCHLIST_SUBSCRIPTION_ID, instrumentIds, usersProcessed, batchSize);

                if (currentBatch == null || !currentBatch.Any())
                {
                    Console.WriteLine("EmailCoordinator: No more users found in DB. Exiting.");
                    continue;
                }

                var records = currentBatch.Select(user => new Dictionary<string, string>
                {
                    { "email", user.Email },
                    { "firstName", user.FirstName },
                    { "lastName", user.LastName },
                    { "instruments", user.WatchedInstrumentIds }
                });

                try
                {
                    await _emailRepo.SendEmails(MAILCHAMP_MAILING_ID, records);
                }
                catch (Exception ex)
                {
                    var errMsg = $"An error occurred while processing batch {usersProcessed} - {usersProcessed + batchSize}: {ex.Message}";
                    errorMessages.Add(errMsg);
                }

                Console.WriteLine($"EmailCoordinator: Finished processing user batch {usersProcessed} - {usersProcessed + batchSize}.");

                usersProcessed += batchSize;
            }
            while (currentBatch != null && currentBatch.Any());

            // Return any errors encountered during batching.
            return errorMessages;
        }
    }
}
