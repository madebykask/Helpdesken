using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
            return DataContext.CaseSections
                .Include(x => x.CaseSectionFields)
                .Include(x =>x.CaseSectionLanguages)   
                .Where(x => x.Customer_Id == customerId).OrderBy(x => x.SectionType).ToList();
        }

        public List<CaseSection> AddCaseSections(List<CaseSection> caseSections)
        {
            DataContext.CaseSections.AddRange(caseSections);
            DataContext.SaveChanges();
            return caseSections;
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

        public CaseSectionLanguage GetCaseSectionLanguage(int id, int languageId)
        {
            return DataContext.CaseSectionLanguages.SingleOrDefault(x => x.CaseSection_Id == id && x.Language_Id == languageId);
        }

        public void ApplyChanges()
        {
            DataContext.SaveChanges();
        }
    }

    public interface ICaseSectionsRepository
    {
        List<CaseSection> GetCaseSections(int customerId);
        List<CaseSection> AddCaseSections(List<CaseSection> caseSections);
        CaseSection GetCaseSection(int sectionId, int customerId);
        int UpdateCaseSection(CaseSection caseSection);
        int AddCaseSection(CaseSection caseSection);
        void DeleteCaseSectionFields(List<int> ids);
        CaseSectionLanguage GetCaseSectionLanguage(int id, int languageId);
        void ApplyChanges();
    }
}
