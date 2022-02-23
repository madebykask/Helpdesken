namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.CaseSolution;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICaseSolutionLanguageRepository : IRepository<CaseSolutionLanguageEntity>
    {
        CaseSolutionLanguageEntity GetCaseSolutionTranslation(int casSolutionId, int languageId);
        void UpdateOtherLanguageCaseSolution(CaseSolutionLanguageEntity caseSolutionLang);
    }

    public class CaseSolutionLanguageRepository : RepositoryBase<CaseSolutionLanguageEntity>, ICaseSolutionLanguageRepository
    {

        public CaseSolutionLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public CaseSolutionLanguageEntity GetCaseSolutionTranslation(int casSolutionId, int languageId)
        {
            var lang = this.DataContext.CaseSolutionLanguages.Where(c => c.CaseSolution_Id == casSolutionId && c.Language_Id == languageId).FirstOrDefault();
            return lang;
        }
        public void UpdateOtherLanguageCaseSolution(CaseSolutionLanguageEntity caseSolutionLang)
        {
            var caseLang = DataContext.CaseSolutionLanguages.SingleOrDefault(l => l.CaseSolution_Id == caseSolutionLang.CaseSolution_Id && l.Language_Id == caseSolutionLang.Language_Id);
            if (caseLang != null)
            {
                caseLang.CaseSolutionName = caseSolutionLang.CaseSolutionName;
                caseLang.ShortDescription = caseSolutionLang.ShortDescription;
                caseLang.Information = string.IsNullOrEmpty(caseSolutionLang.Information) ? string.Empty : caseSolutionLang.Information;
            }
            else
            {
                var newCaseSolutionLanguage = new CaseSolutionLanguageEntity
                {
                    CaseSolutionName = caseSolutionLang.CaseSolutionName,
                    CaseSolution_Id = caseSolutionLang.CaseSolution_Id,
                    Language_Id = caseSolutionLang.Language_Id,
                    ShortDescription = caseSolutionLang.ShortDescription,
                    Information = string.IsNullOrEmpty(caseSolutionLang.Information) ? string.Empty : caseSolutionLang.Information,
                };

                DataContext.CaseSolutionLanguages.Add(newCaseSolutionLanguage);
            }
        }
    }
}
