namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseTypeRepository : IRepository<CaseType>
    {
        void ResetDefault(int exclude, int customerId);

        void ResetEmailDefault(int exclude, int customerId);

        IEnumerable<ItemOverview> GetOverviews(int customerId);

        IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds);

        IEnumerable<int> GetChildren(int caseTypeId);

        IEnumerable<CaseTypeOverview> GetCaseTypeOverviews(int customerId);
    }

    public class CaseTypeRepository : RepositoryBase<CaseType>, ICaseTypeRepository
    {
        public CaseTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void ResetDefault(int exclude, int customerId)
        {
            foreach (CaseType obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }

        public void ResetEmailDefault(int exclude, int customerId)
        {
            foreach (CaseType obj in this.GetMany(s => s.IsEMailDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsEMailDefault = 0;
                this.Update(obj);
            }
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId)
        {
            var entities = this.Table
                    .Where(g => g.Customer_Id == customerId && g.IsActive == 1)
                    .Select(g => new { Value = g.Id, g.Name })
                    .OrderBy(g => g.Name)
                    .ToList();

            return entities.Select(g => new ItemOverview(g.Name, g.Value.ToString(CultureInfo.InvariantCulture)));            
        }

        public IEnumerable<ItemOverview> GetOverviews(int customerId, IEnumerable<int> caseTypesIds)
        {
            var all = caseTypesIds == null || !caseTypesIds.Any();
            var entities = this.Table
                    .Where(g => g.Customer_Id == customerId && 
                            g.IsActive == 1 &&
                            (all || caseTypesIds.Contains(g.Id)))
                    .Select(g => new { Value = g.Id, g.Name })
                    .OrderBy(g => g.Name)
                    .ToList();

            return entities.Select(g => new ItemOverview(g.Name, g.Value.ToString(CultureInfo.InvariantCulture)));                        
        }

        public IEnumerable<int> GetChildren(int caseTypeId)
        {
            var children = new List<int>();
            this.GetChildrenProcess(caseTypeId, children);
            return children.ToArray();
        }

        public IEnumerable<CaseTypeOverview> GetCaseTypeOverviews(int customerId)
        {
            var entities =
                this.DataContext.CaseTypes.Where(c => c.Customer_Id == customerId && c.IsActive != 0)
                    .Select(c => new { c.Id, ParentId = c.Parent_CaseType_Id, c.Name, c.ShowOnExternalPage })
                    .OrderBy(c => c.Name)
                    .ToList();
            //For next release #57742
            //var entities =
            //    this.DataContext.CaseTypes.Where(c => c.Customer_Id == customerId && c.IsActive != 0)
            //        .Select(c => new { c.Id, ParentId = c.Parent_CaseType_Id, c.Name, c.ShowOnExternalPage, c.ShowOnExtPageCases })
            //        .OrderBy(c => c.Name)
            //        .ToList();
            return entities
                    .Select(c => new CaseTypeOverview
                                     {
                                         Id = c.Id,
                                         ParentId = c.ParentId,
                                         Name = c.Name,
                                         ShowOnExternalPage = c.ShowOnExternalPage
                                         //ShowOnExtPageCases = c.ShowOnExtPageCases
                                     });
        }

        private void GetChildrenProcess(int caseTypeId, List<int> children)
        {
            var caseType = this.GetById(caseTypeId);
            if (caseType != null && caseType.SubCaseTypes != null)
            {
                foreach (var child in caseType.SubCaseTypes)
                {
                    children.Add(child.Id);
                    this.GetChildrenProcess(child.Id, children);
                }
            }            
        }
    }
}
