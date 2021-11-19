using System.Collections.Generic;
using TMFDailyEmailer.DTO;

namespace TMFDailyEmailer.TemplateGeneration
{
    /// <summary>
    /// Generates a template for today's watchlist email.
    /// </summary>
    public interface IArticleTemplateGenerator
    {
        /// <summary>
        /// Generates the template for today's watchlist email using the provided list of today's articles.
        /// </summary>
        /// <returns>Updated template content and a list of distinct instruments in today's articles.</returns>
        ArticleTemplateGenerationResult GenerateTemplate(IEnumerable<Article> articles);
    }
}
