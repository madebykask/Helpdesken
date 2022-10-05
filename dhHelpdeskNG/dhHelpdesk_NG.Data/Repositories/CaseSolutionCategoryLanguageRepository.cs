
namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.CaseSolution;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICaseSolutionCategoryLanguageRepository : IRepository<CaseSolutionCategoryLanguageEntity>
    {
        CaseSolutionCategoryLanguageEntity GetCaseSolutionCategoryTranslation(int categoryId, int languageId);
        void UpdateOtherLanguageCaseSolutionCategory(CaseSolutionCategoryLanguageEntity caseSolutionCategoryLang);
        IEnumerable<CaseSolutionCategory> GetTranslatedCategoryList(int languageId, int customerId);
        void DeleteCategoryTranslation(int categoryId);
    }

    public class CaseSolutionCategoryLanguageRepository : RepositoryBase<CaseSolutionCategoryLanguageEntity>, ICaseSolutionCategoryLanguageRepository
    {

        public CaseSolutionCategoryLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        public void DeleteCategoryTranslation(int categoryId)
        {
            var categoryTranslation = this.DataContext.CaseSolutionCategoryLanguages.Where(x => x.Category_Id == categoryId).ToList();
            foreach (var item in categoryTranslation)
            {
                this.Delete(item);
            }
        }
        public CaseSolutionCategoryLanguageEntity GetCaseSolutionCategoryTranslation(int categoryId, int languageId)
        {
            var lang = this.DataContext.CaseSolutionCategoryLanguages.Where(c => c.Category_Id == categoryId && c.Language_Id == languageId).FirstOrDefault();
            return lang;
        }

        public IEnumerable<CaseSolutionCategory> GetTranslatedCategoryList(int languageId, int customerId)
        {
            var cats = DataContext.CaseSolutionCategories.AsEnumerable().Where(c => c.Customer_Id == customerId)
               .Select(q => new CaseSolutionCategory
               {
                   Id = q.Id,
                   Name = q.Name
               }).OrderBy(c => c.Name).ToList();
         
            foreach (var cat in cats)
            {
                var catLang = DataContext.CaseSolutionCategoryLanguages.AsEnumerable().Where(c => c.Category_Id == cat.Id && c.Language_Id == languageId).Select(x => x.CaseSolutionCategoryName).FirstOrDefault();
                if(catLang != null)
                {
                    cat.Name = catLang;
                }
                   
            }

            return cats.ToList();

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

