using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
        private readonly IUnitOfWork _unitOfWork;

        public FloorService(
            IFloorRepository floorRepository,
            IUnitOfWork unitOfWork)
        {
            _floorRepository = floorRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Floor> GetFloors()
        {
            return _floorRepository.GetAll().OrderBy(x => x.Building.Name).ToList();
        }

        public Floor GetFloor(int id)
        {
            return _floorRepository.GetById(id);
        }

        public DeleteMessage DeleteFloor(int id)
        {
            var floor = _floorRepository.GetById(id);

            if (floor != null)
            {
                try
                {
                    _floorRepository.Delete(floor);
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
                _floorRepository.Add(floor);
            else
                _floorRepository.Update(floor);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
