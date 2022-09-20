namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;

    public interface IDomainService
    {
        IList<Domain.Domain> GetAllDomains();
        IList<Domain.Domain> GetDomains(int customerId);
        
        Domain.Domain GetDomain(int id);
        string GetDomainPassword(int domain_id);
        DeleteMessage DeleteDomain(int id);

        void SavePassword(int id, string password);
        void SaveDomain(Domain.Domain domain, out IDictionary<string, string> errors);       
        void Commit();

    }

    public class DomainService : IDomainService
    {
        private readonly IDomainRepository _domainRepository;

#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public DomainService(
            IDomainRepository domainRepository,
            IUnitOfWork unitOfWork)
        {
            this._domainRepository = domainRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Domain.Domain> GetAllDomains()
        {
            return this._domainRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public IList<Domain.Domain> GetDomains(int customerId)
        {
            return this._domainRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Domain.Domain GetDomain(int id)
        {
            return this._domainRepository.GetById(id);
        }

        public string GetDomainPassword(int domain_id)
        {
            return this._domainRepository.GetDomainPassword(domain_id);
        }

        public void SavePassword(int id, string password)
        {
            var domain = this._domainRepository.GetById(id);
            domain.Password = password;
            this._domainRepository.Update(domain);
            this.Commit();
        }

        public DeleteMessage DeleteDomain(int id)
        {
            var domain = this._domainRepository.GetById(id);

            if (domain != null)
            {
                try
                {
                    this._domainRepository.Delete(domain);
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

        public void SaveDomain(Domain.Domain domain, out IDictionary<string, string> errors)
        {
            if (domain == null)
                throw new ArgumentNullException("domain");

            domain.ServerName = domain.ServerName ?? "";
            domain.Filter = domain.Filter ?? "";
            domain.Password = domain.Password ?? "";
            domain.Base = domain.Base ?? "";
            domain.UserName = domain.UserName ?? "";


            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(domain.Name))
                errors.Add("Domain.Domain.Name", "Du måste ange en domän");

            domain.ChangedDate = DateTime.UtcNow;

            if (domain.Id == 0)
                this._domainRepository.Add(domain);
            else
                this._domainRepository.Update(domain);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
