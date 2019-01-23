using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
    public class CaseSectionsRepository : RepositoryBase<CaseSection>, ICaseSectionsRepository
    {
        public CaseSectionsRepository(IDatabaseFactory databaseFactory, IWorkContext workContext = null) : base(databaseFactory, workContext)
        {
        }

        public List<CaseSection> GetCaseSections(int customerId)
        {
            return GetCaseSectionsQuery(customerId).ToList();
        }

        public Task<List<CaseSection>> GetCaseSectionsAsync(int customerId)
        {
            return GetCaseSectionsQuery(customerId).ToListAsync();
        }

        public CaseSection GetCaseSectionByType(int caseSectionType, int customerId)
        {
            return DataContext.CaseSections
                .Include(x => x.CaseSectionLanguages)
                .SingleOrDefault(x => x.Customer_Id == customerId && x.SectionType == caseSectionType);
        }

        public CaseSection GetCaseSection(int sectionId, int customerId)
        {
            return DataContext.CaseSections
                .Include(x => x.CaseSectionLanguages)
                .SingleOrDefault(x => x.Customer_Id == customerId && x.Id == sectionId);
        }

        public int UpdateCaseSection(CaseSection caseSection)
        {
            DataContext.Entry(caseSection).State = EntityState.Modified;
            DataContext.SaveChanges();
            return caseSection.Id;
        }

        public int AddCaseSection(CaseSection caseSection)
        {
            DataContext.CaseSections.Add(caseSection);
            DataContext.SaveChanges();
            return caseSection.Id;
        }

        public void DeleteCaseSectionFields(List<int> ids)
        {
            var fields = DataContext.CaseSectionFields.Where(x => ids.Contains(x.CaseFieldSetting_Id)).ToList();
            DataContext.CaseSectionFields.RemoveRange(fields);
            DataContext.SaveChanges();
        }

        public void ApplyChanges()
        {
            DataContext.SaveChanges();
        }
        private IOrderedQueryable<CaseSection> GetCaseSectionsQuery(int customerId)
        {
            return DataContext.CaseSections
                .Include(x => x.CaseSectionFields)
                .Include(x =>x.CaseSectionLanguages)   
                .Where(x => x.Customer_Id == customerId).OrderBy(x => x.SectionType);
        }
    }

    public interface ICaseSectionsRepository
    {
        List<CaseSection> GetCaseSections(int customerId);
        Task<List<CaseSection>> GetCaseSectionsAsync(int customerId);
        CaseSection GetCaseSection(int sectionId, int customerId);
        CaseSection GetCaseSectionByType(int caseSectionType, int customerId);
        int UpdateCaseSection(CaseSection caseSection);
        int AddCaseSection(CaseSection caseSection);
        void DeleteCaseSectionFields(List<int> ids);
        void ApplyChanges();
    }
}
