namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IFloorService
    {
        IList<Floor> GetFloors();

        Floor GetFloor(int id);

        DeleteMessage DeleteFloor(int id);

        void SaveFloor(Floor floor, out IDictionary<string, string> errors);
        void Commit();
    }

    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public FloorService(
            IFloorRepository floorRepository,
            IUnitOfWork unitOfWork)
        {
            this._floorRepository = floorRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Floor> GetFloors()
        {
            return this._floorRepository.GetAll().OrderBy(x => x.Building.Name).ToList();
        }

        public Floor GetFloor(int id)
        {
            return this._floorRepository.GetById(id);
        }

        public DeleteMessage DeleteFloor(int id)
        {
            var floor = this._floorRepository.GetById(id);

            if (floor != null)
            {
                try
                {
                    this._floorRepository.Delete(floor);
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

        public void SaveFloor(Floor floor, out IDictionary<string, string> errors)
        {
            if (floor == null)
                throw new ArgumentNullException("floor");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(floor.Name))
                errors.Add("Floor.Name", "Du måste ange en våning");

            floor.ChangedDate = DateTime.UtcNow;

            if (floor.Id == 0)
                this._floorRepository.Add(floor);
            else
                this._floorRepository.Update(floor);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
