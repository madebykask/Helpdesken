using DH.Helpdesk.BusinessData.Models.Link.Output;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Infrastructure.Context;
    using Dal.NewInfrastructure;
    using Dal.Repositories;
    using Domain;
    using BusinessLogic.Mappers.Links;
    using BusinessLogic.Specifications;
    using BusinessLogic.Specifications.Links;

    using IUnitOfWork = Dal.Infrastructure.IUnitOfWork;

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

        IEnumerable<LinkOverview> GetLinkOverviewsForStartPage(int[] customers, int? count, bool forStartPage, bool workGroupRestriction = false);

        IList<Link> SearchLinks(int customerId, string searchText, List<int> groupIds);
    }

    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILinkGroupRepository _linkGroupRepository;

        private readonly IWorkContext _workContext;

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public LinkService(
            ILinkRepository linkRepository,
            ILinkGroupRepository linkGroupRepository,
            IUnitOfWork unitOfWork,
            IWorkContext workContext,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _linkRepository = linkRepository;
            _unitOfWork = unitOfWork;
            _workContext = workContext;
            _unitOfWorkFactory = unitOfWorkFactory;
            _linkGroupRepository = linkGroupRepository;
        }

        public IList<Link> GetLinks(int customerId)
        {
            return _linkRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<LinkGroup> GetLinkGroups(int customerId)
        {
            return _linkGroupRepository.GetAll().Where(it => it.Customer_Id == customerId).OrderBy(x => x.LinkGroupName).ToList();
        }

        public Link GetLink(int id)
        {
            return _linkRepository.GetById(id);
        }

        public IList<Link> GetLinksBySolutionIdAndCustomer(int id, int customerId)
        {
            return _linkRepository.GetMany(x => x.Customer_Id == customerId && x.CaseSolution_Id == id).ToList();
        }

        public Link GetLinkByCustomerAndSolution(int id, int customerId)
        {
            return _linkRepository.Get(x => x.CaseSolution_Id == id && x.Customer_Id == customerId);
        }

        public DeleteMessage DeleteLink(int id)
        {
            var link = _linkRepository.GetById(id);

            if (link != null)
            {
                try
                {
                    _linkRepository.Delete(link);
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


            using (var uow = _unitOfWorkFactory.Create())
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
            _unitOfWork.Commit();
        }

        public IEnumerable<LinkOverview> GetLinkOverviews(int[] customers, int? count, bool forStartPage)
        {

            using (var uow = _unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Link>();

                return repository.GetAll()
                        .RestrictByUsers(_workContext)
                        .GetLinksForStartPage(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }



        public IEnumerable<LinkOverview> GetLinkOverviewsForStartPage(int[] customers, int? count, bool forStartPage, bool workGroupRestriction = false)
        {
            var userid = _workContext.User.UserId;

            return _linkRepository.GetLinkOverviewsToStartPage(customers, count, forStartPage, userid, workGroupRestriction);

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
                Commit();
        }
    }
}
