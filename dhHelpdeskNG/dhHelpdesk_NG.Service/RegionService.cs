using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IRegionService
    {
        IList<Region> GetAllRegions();
        IList<Region> GetRegions(int customerId);
        int? GetDefaultId(int customerId); 
        Region GetRegion(int id);

        void DeleteRegion(int id);

        void SaveRegion(Region region, out IDictionary<string, string> errors);
        void Commit();
    }

    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegionService(
            IRegionRepository regionRepository,
            IUnitOfWork unitOfWork)
        {
            _regionRepository = regionRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Region> GetAllRegions()
        {
            return _regionRepository.GetAll().Where(x => x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public IList<Region> GetRegions(int customerId)
        {
            return _regionRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public int? GetDefaultId(int customerId)
        {
            var r = _regionRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }

        public Region GetRegion(int id)
        {
            return _regionRepository.Get(x => x.Id == id);
        }

        public void DeleteRegion(int id)
        {
            var region = _regionRepository.GetById(id);

            if (region != null)
            {
                _regionRepository.Delete(region);
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
                _regionRepository.Add(region);
            else
                //region.ChangedDate = DateTime.UtcNow;
                _regionRepository.Update(region);

            if (region.IsDefault == 1)
                _regionRepository.ResetDefault(region.Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
