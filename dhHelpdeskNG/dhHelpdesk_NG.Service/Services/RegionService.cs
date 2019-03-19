using System.Data.Entity;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Common.Extensions.Integer;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IRegionService
    {
        IList<Region> GetAllRegions();
        IList<Region> GetRegions(int customerId);
        Task<List<Region>> GetRegionsAsync(int customerId);
        IList<RegionOverview> GetRegionsOverview(int customerId);
        IList<Region> GetActiveRegions(int customerId);
        Task<List<Region>> GetActiveRegionsAsync(int customerId);
        IList<Region> GetRegionsWithDepartments(int customerId);
        int? GetDefaultId(int customerId); 
        Region GetRegion(int id);

        void DeleteRegion(int id);

        void SaveRegion(Region region, out IDictionary<string, string> errors);
        void Commit();

        List<ItemOverview> FindByCustomerId(int customerId);
    }

    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegionService(
            IRegionRepository regionRepository,
            IUnitOfWork unitOfWork)
        {
            this._regionRepository = regionRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<Region> GetAllRegions()
        {
            return this._regionRepository.GetMany(x => x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public IList<RegionOverview> GetRegionsOverview(int customerId)
        {
            return this._regionRepository.GetMany(x => x.Customer_Id == customerId)
                .Select(r => new RegionOverview
                    {
                        Id = r.Id,
                        Name = r.Name,
                        IsActive = r.IsActive > 0
                })
                .OrderBy(x => x.Name).ToList();
        }

        public IList<Region> GetRegions(int customerId)
        {
            return this._regionRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Task<List<Region>> GetRegionsAsync(int customerId)
        {
            return _regionRepository.GetMany(x => x.Customer_Id == customerId).AsQueryable()
                    .OrderBy(x => x.Name)
                    .ToListAsync();
        }

        public IList<Region> GetActiveRegions(int customerId)
        {
            return this._regionRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public Task<List<Region>> GetActiveRegionsAsync(int customerId)
        {
            return this._regionRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1).AsQueryable()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public IList<Region> GetRegionsWithDepartments(int customerId)
        {
            return this._regionRepository.GetRegionsWithDepartments(customerId);  
        }

        public int? GetDefaultId(int customerId)
        {
            var r = this._regionRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }

        public Region GetRegion(int id)
        {
            return this._regionRepository.Get(x => x.Id == id);
        }

        public void DeleteRegion(int id)
        {
            var region = this._regionRepository.GetById(id);

            if (region != null)
            {
                this._regionRepository.Delete(region);
                this.Commit();
            }
        }

        public void SaveRegion(Region region, out IDictionary<string, string> errors)
        {
            if (region == null)
                throw new ArgumentNullException("region");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(region.Name))
                errors.Add("region.Name", "Du måste ange ett område");

            region.ChangedDate = DateTime.UtcNow;

            if (region.Id == 0)
                this._regionRepository.Add(region);
            else
                //region.ChangedDate = DateTime.UtcNow;
                this._regionRepository.Update(region);

            if (region.IsDefault == 1)
                this._regionRepository.ResetDefault(region.Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public List<ItemOverview> FindByCustomerId(int customerId)
        {
            return this._regionRepository.FindByCustomerId(customerId);
        }
    }
}
