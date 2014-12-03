namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.BulletinBoards;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.BulletinBoards;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface IBulletinBoardService
    {
        IList<BulletinBoard> GetBulletinBoards(int customerId, bool secure = true);

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

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IWorkContext workContext;

        public BulletinBoardService(
            IBulletinBoardRepository bulletinBoardRepository,
            IUnitOfWork unitOfwork,
            IUnitOfWorkFactory unitOfWorkFactory, 
            IWorkContext workContext)
        {
            this._bulletinBoardRepository = bulletinBoardRepository;
            this._unitOfWork = unitOfwork;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.workContext = workContext;
        }

        public IList<BulletinBoard> GetBulletinBoards(int customerId, bool secure = true)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<BulletinBoard>();

                if (secure)
                    return rep.GetAll()
                            .RestrictByWorkingGroups(this.workContext)
                            .GetByCustomer(customerId)
                            .ToList();
                else
                    return rep.GetAll()
                            .GetByCustomer(customerId)
                            .ToList();

            }
        }
        
        public IList<BulletinBoard> SearchAndGenerateBulletinBoard(int customerId, IBulletinBoardSearch SearchBulletinBoards)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<BulletinBoard>();

                var query = from bb in rep.GetAll()
                                        .RestrictByWorkingGroups(this.workContext)
                                        .GetByCustomer(customerId)
                             select bb;

                if (!string.IsNullOrEmpty(SearchBulletinBoards.SearchBbs))
                {
                    query = query.Where(x => x.Text.Trim().ToLower().Contains(SearchBulletinBoards.SearchBbs.Trim().ToLower()));
                }

                var entities = query.ToList();

                if (!string.IsNullOrEmpty(SearchBulletinBoards.SortBy) && (SearchBulletinBoards.SortBy != "undefined"))
                {
                    if (SearchBulletinBoards.Ascending)
                    {
                        entities = entities.OrderBy(x => x.GetType().GetProperty(SearchBulletinBoards.SortBy).GetValue(x, null)).ToList();
                    }
                    else
                    {
                        entities = entities.OrderByDescending(x => x.GetType().GetProperty(SearchBulletinBoards.SortBy).GetValue(x, null)).ToList();
                    }
                }

                return entities;                
            }
        }

        public BulletinBoard GetBulletinBoard(int id)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<BulletinBoard>();

                return rep.GetAll()
                        .RestrictByWorkingGroups(this.workContext)
                        .GetById(id)
                        .IncludePath(o => o.WGs)
                        .SingleOrDefault();
            }
        }

        public DeleteMessage DeleteBulletinBoard(int id)
        {
            try
            {
                using (var uow = this.unitOfWorkFactory.Create())
                {
                    var rep = uow.GetRepository<BulletinBoard>();

                    var entity = rep.GetAll()
                                  .RestrictByWorkingGroups(this.workContext)
                                  .GetById(id)
                                  .SingleOrDefault();

                    if (entity == null)
                    {
                        return DeleteMessage.Error;
                    }

                    entity.WGs.Clear();

                    rep.DeleteById(id);

                    uow.Save();
                    return DeleteMessage.Success;
                }
            }
            catch
            {
                return DeleteMessage.UnExpectedError;
            }
        }

        public void SaveBulletinBoard(BulletinBoard bulletinBoard, int[] wgs, out IDictionary<string, string> errors)
        {
            if (bulletinBoard == null)
            {
                throw new ArgumentNullException("bulletinBoard");
            }

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(bulletinBoard.Text))
            {
                errors.Add("BulletinBoard.Text", "Du måste ange en anslagstavla");
            }

            if (errors.Any())
            {
                return;
            }

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var bulletinBoardRep = uow.GetRepository<BulletinBoard>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();

                BulletinBoard entity;
                var now = DateTime.Now;
                if (bulletinBoard.IsNew())
                {
                    entity = new BulletinBoard();
                    BulletinBoardMapper.MapToEntity(bulletinBoard, entity);
                    entity.CreatedDate = now;
                    entity.ChangedDate = now;
                    bulletinBoardRep.Add(entity);
                }
                else
                {
                    entity = bulletinBoardRep.GetById(bulletinBoard.Id);
                    BulletinBoardMapper.MapToEntity(bulletinBoard, entity);
                    entity.ChangedDate = now;
                    bulletinBoardRep.Update(entity);
                }

                entity.WGs.Clear();
                if (wgs != null)
                {
                    foreach (var wg in wgs)
                    {
                        var workingGroupEntity = workingGroupRep.GetById(wg);
                        entity.WGs.Add(workingGroupEntity);
                    }
                }

                uow.Save();
            }                        
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
                        .GetFromDate()
                        .GetUntilDate()
                        .RestrictByWorkingGroups(this.workContext)
                        .GetForStartPage(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }
    }
}
