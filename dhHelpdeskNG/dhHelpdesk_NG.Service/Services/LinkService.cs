namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ILinkService
    {
        IList<Link> GetLinks(int customerId);

        Link GetLink(int id);

        DeleteMessage DeleteLink(int id);
        IList<LinkGroup> GetLinkGroups(int customerId);

        void SaveLink(Link link, int[] us, out IDictionary<string, string> errors);
        void Commit();
    }

    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILinkGroupRepository _linkGroupRepository;
        private readonly IUserRepository _userRepository;

        public LinkService(
            ILinkRepository linkRepository,
            ILinkGroupRepository linkGroupRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            this._linkRepository = linkRepository;
            this._unitOfWork = unitOfWork;
            this._linkGroupRepository = linkGroupRepository;
            this._userRepository = userRepository;
        }

        public IList<Link> GetLinks(int customerId)
        {
            return this._linkRepository.GetMany(x => x.Customer_Id == customerId).ToList();
        }

        public IList<LinkGroup> GetLinkGroups(int customerId)
        {
            return this._linkGroupRepository.GetAll().OrderBy(x => x.LinkGroupName).ToList();
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
                errors.Add("Link.URLAddress", "Du måste ange en URL-adress");


            if (link.LinkUsers != null)
                foreach (var delete in link.LinkUsers.ToList())
                    link.LinkUsers.Remove(delete);
            else
                link.LinkUsers = new List<User>();

            if (us != null)
            {
                foreach (int id in us)
                {
                    var u = this._userRepository.GetById(id);

                    if (u != null)
                        link.LinkUsers.Add(u);
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
    }
}
