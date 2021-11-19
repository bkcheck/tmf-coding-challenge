using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMFDailyEmailer.DTO;

namespace TMFDailyEmailer.TemplateGeneration
{
    public class ArticleTemplateGenerator : IArticleTemplateGenerator
    {
        public ArticleTemplateGenerationResult GenerateTemplate(IEnumerable<Article> articles)
        {
            var sb = new StringBuilder();

            var uniqueInstruments = GenerateArticlesSection(sb, articles);
            GenerateInstrumentsSection(sb, uniqueInstruments);

            var content = sb.ToString();

            return new ArticleTemplateGenerationResult
            {
                TemplateContent = content,
                UniqueArticleInstruments = uniqueInstruments,
            };
        }

        /// <summary>
        /// Appends the Articles XML chunk to the template content. Returns a Dictionary of unique
        /// instruments across all articles.
        /// </summary>
        private Dictionary<int, ArticleInstrument> GenerateArticlesSection(StringBuilder sb, IEnumerable<Article> articles)
        {
            var uniqueInstruments = new Dictionary<int, ArticleInstrument>();

            sb.Append("<Articles>");

            foreach (var article in articles)
            {
                sb.Append("<Article><Headline>");
                sb.Append(article.Headline);
                sb.Append("</Headline><Byline>");
                sb.Append(article.Byline);
                sb.Append("</Byline><PermaLink>");
                sb.Append(article.Permalink);
                sb.Append("</PermaLink><PublishDate>");
                sb.Append(article.DatePublished.ToString("o"));
                sb.Append("</PublishDate><Authors>");

                foreach (var author in article.Authors)
                {
                    sb.Append("<Author><FirstName>");
                    sb.Append(author.FirstName);
                    sb.Append("</FirstName><LastName>");
                    sb.Append(author.LastName);
                    sb.Append("</LastName></Author>");
                }

                sb.Append("</Authors><InstrumentIds>");

                foreach (var instrument in article.Instruments)
                {
                    sb.Append("<InstrumentId>");
                    sb.Append(instrument.InstrumentId);
                    sb.Append("</InstrumentId>");

                    uniqueInstruments[instrument.InstrumentId] = instrument;
                }

                sb.Append("</InstrumentIds></Article>");
            }

            sb.Append("</Articles>");

            return uniqueInstruments;
        }

        /// <summary>
        /// Appends the Instruments XML chunk to the template content.
        /// </summary>
        private void GenerateInstrumentsSection(StringBuilder sb, Dictionary<int, ArticleInstrument> uniqueInstruments)
        {
            sb.Append("<Instruments>");

            foreach (var instrument in uniqueInstruments.Values)
            {
                sb.Append("<Instrument><InstrumentId>");
                sb.Append(instrument.InstrumentId);
                sb.Append("</InstrumentId><Symbol>");
                sb.Append(instrument.Symbol);
                sb.Append("</Symbol><CompanyName>");
                sb.Append(instrument.CompanyName);
                sb.Append("</CompanyName></Instrument>");
            }

            sb.Append("</Instruments>");
        }
    }
}
