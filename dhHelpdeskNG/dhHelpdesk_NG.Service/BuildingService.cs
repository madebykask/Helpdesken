using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IBuildingService
    {
        IList<Building> GetBuildings(int customerId);

        Building GetBuilding(int id);

        DeleteMessage DeleteBuilding(int id);

        void SaveBuilding(Building building, out IDictionary<string, string> errors);
        void Commit();
    }

    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BuildingService(
            IBuildingRepository buildingRepository,
            IUnitOfWork unitOfWork)
        {
            _buildingRepository = buildingRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Building> GetBuildings(int customerId)
        {
            return _buildingRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Building GetBuilding(int id)
        {
            return _buildingRepository.GetById(id);
        }

        public DeleteMessage DeleteBuilding(int id)
        {
            var building = _buildingRepository.GetById(id);

            if (building != null)
            {
                try
                {
                    _buildingRepository.Delete(building);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveBuilding(Building building, out IDictionary<string, string> errors)
        {
            if (building == null)
                throw new ArgumentNullException("building");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(building.Name))
                errors.Add("Building.Name", "Du måste ange en byggnad");

            building.ChangedDate = DateTime.UtcNow;

            if (building.Id == 0)
                _buildingRepository.Add(building);
            else
                _buildingRepository.Update(building);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
