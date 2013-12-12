using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;
using System.Linq;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Data.Repositories
{
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;

    #region REGION

    public interface IRegionRepository : IRepository<Region>
    {
        List<ItemOverviewDto> FindByCustomerId(int customerId);

        void ResetDefault(int exclude);
        //IList<Region> GetRegionsBySelection(int customerId, string[] reg);
        //IList<Region> GetRegionsSelected(int customerId, string[] reg);
    }

    public class RegionRepository : RepositoryBase<Region>, IRegionRepository
    {
        public RegionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }



        //public IList<Region> GetRegionsSelected(int customerId, string[] reg)
        //{
        //    //return _regionRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();

        //    List<Region> rlist = new List<Region>();// = new IList<CaseResults>();

        //    IList<Region> rr = (from w in this.DataContext.Set<Region>()
        //                        where w.Customer_Id == customerId
        //                        select w).ToList();

        //    bool add = false;
        //    if (reg != null)
        //    {
        //        foreach (Region fg in rr)
        //        {
        //            add = false;
        //            foreach (string i in reg)
        //            {
        //                if (i == fg.Id.ToString())
        //                {
        //                    add = true;
        //                }

        //            }

        //            if (add == true)
        //            {
        //                rlist.Add(fg);
        //            }
        //        }
        //    }
        //    return rlist;
        //    //if (rlist.Count == 0)
        //    //{
        //    //    return rr.ToList();
        //    //}
        //    //else
        //    //{
        //    //    return rlist.ToList();
        //    //}
        //}

        //public IList<Region> GetRegionsBySelection(int customerId, string[] reg)
        //{
        //    List<Region> rlist = new List<Region>();// = new IList<CaseResults>();

        //    IList<Region> rr = (from w in this.DataContext.Set<Region>()
        //                        where w.Customer_Id == customerId
        //                        select w).ToList();

        //    bool add = false;
        //    if (reg != null)
        //    {
        //        foreach (Region fg in rr)
        //        {
        //            add = true;
        //            foreach (string i in reg)
        //            {
        //                if (i == fg.Id.ToString())
        //                {
        //                    add = false;
        //                }

        //            }

        //            if (add == true)
        //            {
        //                rlist.Add(fg);
        //            }
        //        }
        //    }
        //    if (rlist.Count == 0)
        //    {
        //        return rr.ToList();
        //    }
        //    else
        //    {
        //        return rlist.ToList();
        //    }

        //}

        public List<ItemOverviewDto> FindByCustomerId(int customerId)
        {
            var regionOverview =
                this.DataContext.Regions.Where(r => r.Customer_Id == customerId)
                    .Select(r => new { r.Name, r.Id })
                    .ToList();

            return
                regionOverview.Select(
                    r => new ItemOverviewDto { Name = r.Name, Value = r.Id.ToString(CultureInfo.InvariantCulture) })
                              .ToList();
        }

        public void ResetDefault(int exclude)
        {
            foreach (Region obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
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
