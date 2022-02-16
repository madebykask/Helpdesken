
namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.CaseSolution;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICaseSolutionCategoryLanguageRepository : IRepository<CaseSolutionCategoryLanguageEntity>
    {
        CaseSolutionCategoryLanguageEntity GetCaseSolutionCategoryLanguage(int categoryId, int languageId);
        void UpdateOtherLanguageCaseSolutionCategory(CaseSolutionCategoryLanguageEntity caseSolutionCategoryLang);
    }

    public class CaseSolutionCategoryLanguageRepository : RepositoryBase<CaseSolutionCategoryLanguageEntity>, ICaseSolutionCategoryLanguageRepository
    {

        public CaseSolutionCategoryLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public CaseSolutionCategoryLanguageEntity GetCaseSolutionCategoryLanguage(int categoryId, int languageId)
        {
            var lang = this.DataContext.CaseSolutionCategoryLanguages.Where(c => c.Category_Id == categoryId && c.Language_Id == languageId).FirstOrDefault();
            return lang;
        }

        public void UpdateOtherLanguageCaseSolutionCategory(CaseSolutionCategoryLanguageEntity caseSolutionCategoryLang)
        {
            var catLang = DataContext.CaseSolutionCategoryLanguages.SingleOrDefault(l => l.Category_Id == caseSolutionCategoryLang.Category_Id && l.Language_Id == caseSolutionCategoryLang.Language_Id);
            if (catLang != null)
            {
                catLang.CaseSolutionCategoryName = caseSolutionCategoryLang.CaseSolutionCategoryName;
            }
            else
            {
                var newCaseSolutionCategoryLanguage = new CaseSolutionCategoryLanguageEntity
                {
                    CaseSolutionCategoryName = caseSolutionCategoryLang.CaseSolutionCategoryName,
                    Category_Id = caseSolutionCategoryLang.Category_Id,
                    Language_Id = caseSolutionCategoryLang.Language_Id
                };

                DataContext.CaseSolutionCategoryLanguages.Add(newCaseSolutionCategoryLanguage);
            }
        }
    }
}

