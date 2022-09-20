namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IRoomService
    {
        IList<Room> GetRooms();

        Room GetRoom(int id);

        DeleteMessage DeleteRoom(int id);

        void SaveRoom(Room room, out IDictionary<string, string> errors);
        void Commit();
    }

    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public RoomService(
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            this._roomRepository = roomRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Room> GetRooms()
        {
            return this._roomRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public Room GetRoom(int id)
        {
            return this._roomRepository.GetById(id);
        }

        public DeleteMessage DeleteRoom(int id)
        {
            var room = this._roomRepository.GetById(id);

            if (room != null)
            {
                try
                {
                    this._roomRepository.Delete(room);
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

        public void SaveRoom(Room room, out IDictionary<string, string> errors)
        {
            if (room == null)
                throw new ArgumentNullException("room");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(room.Name))
                errors.Add("Room.Name", "Du måste ange ett rum");

            room.ChangedDate = DateTime.UtcNow;

            if (room.Id == 0)
                this._roomRepository.Add(room);
            else
                this._roomRepository.Update(room);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
