using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IBulletinBoardService
    {
        IList<BulletinBoard> GetBulletinBoards(int customerId);
        IList<BulletinBoard> SearchAndGenerateBulletinBoard(int customerId, IBulletinBoardSearch SearchBulletinBoards);

        BulletinBoard GetBulletinBoard(int id);

        DeleteMessage DeleteBulletinBoard(int id);

        void SaveBulletinBoard(BulletinBoard bulletinBoard, int[] wgs, out IDictionary<string, string> errors);
        void Commit();
    } 

    public class BulletinBoardService : IBulletinBoardService
    {
        private readonly IBulletinBoardRepository _bulletinBoardRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;

        public BulletinBoardService(
            IBulletinBoardRepository bulletinBoardRepository,            
            IUnitOfWork unitOfwork,
            IWorkingGroupRepository workingGroupRepository)
        {
            _bulletinBoardRepository = bulletinBoardRepository;
            _unitOfWork = unitOfwork;
            _workingGroupRepository = workingGroupRepository;
        }

        public IList<BulletinBoard> GetBulletinBoards(int customerId)
        {
            return _bulletinBoardRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<BulletinBoard> SearchAndGenerateBulletinBoard(int customerId, IBulletinBoardSearch SearchBulletinBoards)
        {
            var query = (from bb in _bulletinBoardRepository.GetAll().Where(x => x.Customer_Id == customerId)
                         select bb);

            if (!string.IsNullOrEmpty(SearchBulletinBoards.SearchBbs))
                query = query.Where(x => x.Text.Contains(SearchBulletinBoards.SearchBbs));

            if (!string.IsNullOrEmpty(SearchBulletinBoards.SortBy) && (SearchBulletinBoards.SortBy != "undefined"))
            {
                if (SearchBulletinBoards.Ascending)
                    query = query.OrderBy(x => x.GetType().GetProperty(SearchBulletinBoards.SortBy).GetValue(x, null));
                else
                    query = query.OrderByDescending(x => x.GetType().GetProperty(SearchBulletinBoards.SortBy).GetValue(x, null));
            }

            return query.ToList();

            //return query.OrderBy(x => x.ChangedDate).ToList();
        }

        public BulletinBoard GetBulletinBoard(int id)
        {
            return _bulletinBoardRepository.GetById(id);
        }

        public DeleteMessage DeleteBulletinBoard(int id)
        {
            var bulletinBoard = _bulletinBoardRepository.GetById(id);

            if (bulletinBoard != null)
            {
                try
                {
                    _bulletinBoardRepository.Delete(bulletinBoard);
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

        public void SaveBulletinBoard(BulletinBoard bulletinBoard, int[] wgs, out IDictionary<string, string> errors)
        {
            if (bulletinBoard == null)
                throw new ArgumentNullException("bulletinboard");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(bulletinBoard.Text))
                errors.Add("BulletinBoard.Text", "Du måste ange en anslagstavla");

            if (bulletinBoard.WGs != null)
                foreach (var delete in bulletinBoard.WGs.ToList())
                    bulletinBoard.WGs.Remove(delete);
            else
                bulletinBoard.WGs = new List<WorkingGroup>();

            if (wgs != null)
            {
                foreach (int id in wgs)
                {
                    var wg = _workingGroupRepository.GetById(id);

                    if (wg != null)
                        bulletinBoard.WGs.Add(wg);
                }
            }

            if (bulletinBoard.Id == 0)
                _bulletinBoardRepository.Add(bulletinBoard);
            else
                _bulletinBoardRepository.Update(bulletinBoard);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
