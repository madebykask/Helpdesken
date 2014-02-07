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

        void SaveLink(Link link, out IDictionary<string, string> errors);
        void Commit();
    }

    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LinkService(
            ILinkRepository linkRepository,
            IUnitOfWork unitOfWork)
        {
            this._linkRepository = linkRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<Link> GetLinks(int customerId)
        {
            return this._linkRepository.GetMany(x => x.Customer_Id == customerId).ToList();
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

        public void SaveLink(Link link, out IDictionary<string, string> errors)
        {
            if (link == null)
                throw new ArgumentNullException("link");

            errors = new Dictionary<string, string>();

            link.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(link.URLName))
                errors.Add("Link.URLName", "Du måste ange ett URL-namn");

            if (string.IsNullOrEmpty(link.URLAddress))
                errors.Add("Link.URLAddress", "Du måste ange en URL-adress");

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
