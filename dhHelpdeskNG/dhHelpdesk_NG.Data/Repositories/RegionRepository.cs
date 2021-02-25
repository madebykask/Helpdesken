namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region REGION

    public interface IRegionRepository : IRepository<Region>
    {
        List<ItemOverview> FindByCustomerId(int customerId);
        int? GetDefaultRegion(int customerId);
        IList<Region> GetRegionsWithDepartments(int customerId);
        void ResetDefault(int excludeId, int customerId);
        int GetRegionLanguage(int regiondid);
    }

    public class RegionRepository : RepositoryBase<Region>, IRegionRepository
    {
        public RegionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public int GetRegionLanguage(int regiondid)
        {
            return this.DataContext.Regions.Where(d => d.Id == regiondid).Select(d => d.LanguageId).FirstOrDefault();
        }

        public IList<Region> GetRegionsWithDepartments(int customerId)
        {
            //TODO fixa distinct
            return (from r in this.DataContext.Regions
                    join d in this.DataContext.Departments on r.Id equals d.Region_Id
                    where r.Customer_Id == customerId
                    select r).ToList();  
        }

        public List<ItemOverview> FindByCustomerId(int customerId)
        {
            var regionOverview =
                this.DataContext.Regions.Where(r => r.Customer_Id == customerId && r.IsActive != 0)
                    .Select(r => new { r.Name, r.Id })
                    .ToList();

            return
                regionOverview.Select(r => new ItemOverview(r.Name, r.Id.ToString(CultureInfo.InvariantCulture)))
                    .OrderBy(x => x.Name).ToList();
        }
        
        public int? GetDefaultRegion(int customerId)
        {
            int? regionId =
                this.DataContext.Regions.Where(r => r.Customer_Id == customerId && r.IsActive != 0 && r.IsDefault == 1)
                    .Select(r => r.Id).SingleOrDefault();

            return regionId <= 0 ? null : regionId;
        }

        public void ResetDefault(int excludeId, int customerId)
        {
            foreach (var obj in this.GetMany(s => s.IsDefault == 1 && s.Id != excludeId && s.Customer_Id == customerId))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }
    }

    #endregion

    #region REGIONLANGUAGE

    public interface IRegionLanguageRepository : IRepository<RegionLanguage>
    {
    }

    public class RegionLanguageRepository : RepositoryBase<RegionLanguage>, IRegionLanguageRepository
    {
        public RegionLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }

    #endregion
}
