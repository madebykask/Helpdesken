using DH.Helpdesk.BusinessData.Models.Link.Output;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Links;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Links;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

    public interface ILinkService
    {
        IList<Link> GetLinks(int customerId);
        IList<Link> GetLinksBySolutionIdAndCustomer(int id, int customerId);

        Link GetLink(int id);
        LinkGroup GetLinkGroup(int id);
        Link GetLinkByCustomerAndSolution(int id, int customerId);

        DeleteMessage DeleteLink(int id);
        DeleteMessage DeleteLinkGroup(int id);
        IList<LinkGroup> GetLinkGroups(int customerId);

        void SaveLink(Link link, int[] us, int[] wg, out IDictionary<string, string> errors);
        void SaveLinkGroup(LinkGroup linkGroup, out IDictionary<string, string> errors);
        void Commit();

        /// <summary>
        /// The get link overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for start page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<LinkOverview> GetLinkOverviews(int[] customers, int? count, bool forStartPage);

        IEnumerable<LinkOverview> GetLinkOverviewsForStartPage(int[] customers, int? count, bool forStartPage);

        IList<Link> SearchLinks(int customerId, string searchText, List<int> groupIds);
    }

    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILinkGroupRepository _linkGroupRepository;
        private readonly IUserRepository _userRepository;

        private readonly IWorkContext workContext;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public LinkService(
            ILinkRepository linkRepository,
            ILinkGroupRepository linkGroupRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IWorkContext workContext,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this._linkRepository = linkRepository;
            this._unitOfWork = unitOfWork;
            this.workContext = workContext;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this._linkGroupRepository = linkGroupRepository;
            this._userRepository = userRepository;
        }

        public IList<Link> GetLinks(int customerId)
        {
            return this._linkRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<LinkGroup> GetLinkGroups(int customerId)
        {
            return this._linkGroupRepository.GetAll().Where(it => it.Customer_Id == customerId).OrderBy(x => x.LinkGroupName).ToList();
        }

        public Link GetLink(int id)
        {
            return this._linkRepository.GetById(id);
        }

        public IList<Link> GetLinksBySolutionIdAndCustomer(int id, int customerId)
        {
            return this._linkRepository.GetMany(x => x.Customer_Id == customerId && x.CaseSolution_Id == id).ToList();
        }

        public Link GetLinkByCustomerAndSolution(int id, int customerId)
        {
            return this._linkRepository.Get(x => x.CaseSolution_Id == id && x.Customer_Id == customerId);
        }

        public DeleteMessage DeleteLink(int id)
        {
            var link = this._linkRepository.GetById(id);

            if (link != null)
            {
                try
                {
                    this._linkRepository.Delete(link);
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

        public void SaveLink(Link link, int[] us, int[] wg, out IDictionary<string, string> errors)
        {
            if (link == null)
                throw new ArgumentNullException("link");

            errors = new Dictionary<string, string>();

            link.ChangedDate = DateTime.UtcNow;
            link.SortOrder = link.SortOrder ?? "";

            if (string.IsNullOrEmpty(link.URLName))
                errors.Add("Link.URLName", "Du måste ange ett URL-namn");

            if (string.IsNullOrEmpty(link.URLAddress))
                link.URLAddress = link.URLAddress ?? "";
            //    errors.Add("Link.URLAddress", "Du måste ange en URL-adress");


            using (var uow = this.unitOfWorkFactory.Create())
            {
                var linkRep = uow.GetRepository<Link>();
                var userRep = uow.GetRepository<User>();
                var workinggroupRep = uow.GetRepository<WorkingGroupEntity>();

                Link entity;
                var now = DateTime.Now;
                if (link.IsNew())
                {
                    entity = new Link();
                    LinkMapper.MapToEntity(link, entity);
                    entity.CreatedDate = now;
                    entity.ChangedDate = now;
                    linkRep.Add(entity);
                }
                else
                {
                    entity = linkRep.GetById(link.Id);
                    LinkMapper.MapToEntity(link, entity);
                    entity.ChangedDate = now;
                    linkRep.Update(entity);
                }

                if (entity.Us != null)
                    entity.Us.Clear();
                else
                    entity.Us = new List<User>();

                if (us != null)
                {
                    foreach (var u in us)
                    {
                        User userEntity = userRep.GetById(u);
                        entity.Us.Add(userEntity);
                    }
                }

                if (entity.Wg != null)
                    entity.Wg.Clear();
                else
                    entity.Wg = new List<WorkingGroupEntity>();

                if (wg != null)
                {
                    foreach (var w in wg)
                    {
                        WorkingGroupEntity wgEntity = workinggroupRep.GetById(w);
                        entity.Wg.Add(wgEntity);
                    }
                }

                uow.Save();
            }

        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public IEnumerable<LinkOverview> GetLinkOverviews(int[] customers, int? count, bool forStartPage)
        {

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Link>();

                return repository.GetAll()
                        .RestrictByUsers(this.workContext)
                        .GetLinksForStartPage(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }



        public IEnumerable<LinkOverview> GetLinkOverviewsForStartPage(int[] customers, int? count, bool forStartPage)
        {
            int userid = this.workContext.User.UserId;
            IEnumerable<UserWorkingGroup> wg = this.workContext.User.UserWorkingGroups.ToList();
            return _linkRepository.GetLinkOverviewsToStartPage(customers, count, forStartPage, userid, wg);


        }

        public IList<Link> SearchLinks(int customerId, string searchText, List<int> groupIds)
        {
            IEnumerable<Link> result;
            if (!string.IsNullOrEmpty(searchText))
            {
                var searched = searchText.ToLower();
                result = _linkRepository.GetMany(x => x.Customer_Id == customerId && x.URLName.ToLower().Contains(searched));
            }
            else
            {
                result = _linkRepository.GetMany(x => x.Customer_Id == customerId);
            }
            if (groupIds != null && groupIds.Any())
            {
                result = result.Where(x => x.LinkGroup_Id.HasValue && groupIds.Contains(x.LinkGroup_Id.Value));
            }
            return result.ToList();
        }


        public LinkGroup GetLinkGroup(int id)
        {
            return _linkGroupRepository.GetById(id);
        }

        public DeleteMessage DeleteLinkGroup(int id)
        {
            var linkGroup = _linkGroupRepository.GetById(id);

            if (linkGroup != null)
            {
                try
                {
                    _linkGroupRepository.Delete(linkGroup);
                    Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveLinkGroup(LinkGroup linkGroup, out IDictionary<string, string> errors)
        {
            if (linkGroup == null)
                throw new ArgumentNullException("linkGroup");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(linkGroup.LinkGroupName))
                errors.Add("LinkGroup.LinkGroupName", "Du måste ange ett namn");

            if (linkGroup.Id == 0)
                _linkGroupRepository.Add(linkGroup);
            else
                _linkGroupRepository.Update(linkGroup);

            if (errors.Count == 0)
                this.Commit();
        }
    }
}
