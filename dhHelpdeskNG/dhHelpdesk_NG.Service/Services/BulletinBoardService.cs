using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.BulletinBoards;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface IBulletinBoardService
    {
        IList<BulletinBoard> GetBulletinBoards(int customerId);
        IList<BulletinBoard> SearchAndGenerateBulletinBoard(int customerId, IBulletinBoardSearch SearchBulletinBoards);

        BulletinBoard GetBulletinBoard(int id);

        DeleteMessage DeleteBulletinBoard(int id);

        void SaveBulletinBoard(BulletinBoard bulletinBoard, int[] wgs, out IDictionary<string, string> errors);
        void Commit();

        IEnumerable<BulletinBoardOverview> GetBulletinBoardOverviews(int[] customers, int? count, bool forStartPage);
    }

    public class BulletinBoardService : IBulletinBoardService
    {
        private readonly IBulletinBoardRepository _bulletinBoardRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkingGroupRepository _workingGroupRepository;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IWorkContext workContext;

        public BulletinBoardService(
            IBulletinBoardRepository bulletinBoardRepository,
            IUnitOfWork unitOfwork,
            IWorkingGroupRepository workingGroupRepository, 
            IUnitOfWorkFactory unitOfWorkFactory, 
            IWorkContext workContext)
        {
            this._bulletinBoardRepository = bulletinBoardRepository;
            this._unitOfWork = unitOfwork;
            this._workingGroupRepository = workingGroupRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.workContext = workContext;
        }

        public IList<BulletinBoard> GetBulletinBoards(int customerId)
        {
            return this._bulletinBoardRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<BulletinBoard> SearchAndGenerateBulletinBoard(int customerId, IBulletinBoardSearch SearchBulletinBoards)
        {
            var query = (from bb in this._bulletinBoardRepository.GetAll().Where(x => x.Customer_Id == customerId)
                         select bb);

            if (!string.IsNullOrEmpty(SearchBulletinBoards.SearchBbs))
                query = query.Where(x => x.Text.ContainsText(SearchBulletinBoards.SearchBbs));

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
            return this._bulletinBoardRepository.GetById(id);
        }

        public DeleteMessage DeleteBulletinBoard(int id)
        {
            var bulletinBoard = this._bulletinBoardRepository.GetById(id);

            if (bulletinBoard != null)
            {
                try
                {
                    this._bulletinBoardRepository.Delete(bulletinBoard);
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
                bulletinBoard.WGs = new List<WorkingGroupEntity>();

            if (wgs != null)
            {
                foreach (int id in wgs)
                {
                    var wg = this._workingGroupRepository.GetById(id);

                    if (wg != null)
                        bulletinBoard.WGs.Add(wg);
                }
            }

            if (bulletinBoard.Id == 0)
                this._bulletinBoardRepository.Add(bulletinBoard);
            else
                this._bulletinBoardRepository.Update(bulletinBoard);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public IEnumerable<BulletinBoardOverview> GetBulletinBoardOverviews(int[] customers, int? count, bool forStartPage)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<BulletinBoard>();

                return repository.GetAll()
                        .RestrictByWorkingGroups(this.workContext)
                        .GetForStartPage(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }
    }
}
