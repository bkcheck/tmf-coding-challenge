using System.Collections.Generic;
using TMFDailyEmailer.DTO;

namespace TMFDailyEmailer.TemplateGeneration
{
    public class ArticleTemplateGenerationResult
    {
        public string TemplateContent { get; set; }
        public Dictionary<int, ArticleInstrument> UniqueArticleInstruments { get; set; }
    }
}
