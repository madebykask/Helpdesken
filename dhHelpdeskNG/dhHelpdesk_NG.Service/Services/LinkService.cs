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

        Link GetLink(int id);
        LinkGroup GetLinkGroup(int id);

        DeleteMessage DeleteLink(int id);
        DeleteMessage DeleteLinkGroup(int id);
        IList<LinkGroup> GetLinkGroups(int customerId);

        void SaveLink(Link link, int[] us, out IDictionary<string, string> errors);
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

        public void SaveLink(Link link, int[] us, out IDictionary<string, string> errors)
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


            if (link.Us != null)
                foreach (var delete in link.Us.ToList())
                    link.Us.Remove(delete);
            else
                link.Us = new List<User>();

            if (us != null)
            {
                foreach (int id in us)
                {
                    var u = this._userRepository.GetById(id);

                    if (u != null)
                        link.Us.Add(u);
                }
            }

            if (link.Id == 0)
                this._linkRepository.Add(link);
            else
                this._linkRepository.Update(link);

            if (errors.Count == 0)
                this.Commit();
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


        public LinkGroup GetLinkGroup(int id)
        {
            return _linkGroupRepository.GetById(id);
        }

        public DeleteMessage DeleteLinkGroup(int id)
        {
            var linkGroup = _linkGroupRepository.GetById(id);

            if(linkGroup != null)
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
            if(linkGroup == null)
                throw new ArgumentNullException("linkGroup");

            errors = new Dictionary<string, string>();

            if(string.IsNullOrEmpty(linkGroup.LinkGroupName))
                errors.Add("LinkGroup.LinkGroupName", "Du måste ange ett namn");

            if(linkGroup.Id == 0)
                _linkGroupRepository.Add(linkGroup);
            else
                _linkGroupRepository.Update(linkGroup);

            if(errors.Count == 0)
                this.Commit();
        }
    }
}
