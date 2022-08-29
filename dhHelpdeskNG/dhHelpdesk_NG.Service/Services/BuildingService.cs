namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public BuildingService(
            IBuildingRepository buildingRepository,
            IUnitOfWork unitOfWork)
        {
            this._buildingRepository = buildingRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Building> GetBuildings(int customerId)
        {
            return this._buildingRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Building GetBuilding(int id)
        {
            return this._buildingRepository.GetById(id);
        }

        public DeleteMessage DeleteBuilding(int id)
        {
            var building = this._buildingRepository.GetById(id);

            if (building != null)
            {
                try
                {
                    this._buildingRepository.Delete(building);
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
                this._buildingRepository.Add(building);
            else
                this._buildingRepository.Update(building);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
