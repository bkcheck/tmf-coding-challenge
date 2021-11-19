using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using TMFDailyEmailer.DataAccess;
using TMFDailyEmailer.TemplateGeneration;

namespace TMFDailyEmailer
{
    class Program
    {
        private const int DEFAULT_USER_BATCH_SIZE = 200;

        static void Main(string[] args)
        {
            #region Load configuration

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            if (config == null)
            {
                throw new Exception("Can't initialize application: no configuration file found.");
            }

            var connectionString = config.GetConnectionString("FoolDB");
            var articleApiUri = config["ArticleApiUri"];
            var mailChampUri = config["MailChampUri"];

            var batchSizeConfigPresent = int.TryParse(config["UserBatchSize"], out int batchSize);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Can't initialize application: no DB connection string found.");
            }

            if (string.IsNullOrWhiteSpace(articleApiUri))
            {
                throw new Exception("Can't initialize application: no ArticleApiUri found.");
            }

            if (string.IsNullOrWhiteSpace(mailChampUri))
            {
                throw new Exception("Can't initialize application: no MailChampUri found.");
            }

            if (!batchSizeConfigPresent)
            {
                batchSize = DEFAULT_USER_BATCH_SIZE;
            }

            #endregion

            #region Instantiate dependencies

            var httpClient = new HttpClient();

            var articleRepo = new ArticleRepository(articleApiUri, httpClient);
            var userRepo = new UserRepository(connectionString);
            var emailRepo = new EmailRepository(mailChampUri, httpClient);
            var templateGenerator = new ArticleTemplateGenerator();

            var emailCoordinator = new DailyEmailCoordinator(articleRepo, userRepo, emailRepo, templateGenerator);

            #endregion

            try
            {
                Console.WriteLine("Starting EmailCoordinator");

                var start = DateTime.Now;
                var errorMessages = emailCoordinator.BatchSendWatchlistEmails(batchSize)
                    .GetAwaiter()
                    .GetResult();

                var end = DateTime.Now;

                var dt = end - start;
                Console.WriteLine($"EmailCoordinator finished in {dt.TotalSeconds}s.");

                if (errorMessages.Any())
                {
                    Console.WriteLine($"EmailCoordinator finished running with errors:");

                    foreach (var errMsg in errorMessages)
                    {
                        Console.WriteLine($"  {errMsg}");
                    }
                }
                else
                {
                    Console.WriteLine("EmailCoordinator finished running with no errors.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
