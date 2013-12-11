using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;

namespace dhHelpdesk_NG.Service
{
    public interface IDomainService
    {
        IList<Domain.Domain> GetAllDomains();
        IList<Domain.Domain> GetDomains(int customerId);
        
        Domain.Domain GetDomain(int id);
        string GetDomainPassword(int domain_id);
        DeleteMessage DeleteDomain(int id);
        
        void SaveDomain(Domain.Domain domain, out IDictionary<string, string> errors);       
        void Commit();

    }

    public class DomainService : IDomainService
    {
        private readonly IDomainRepository _domainRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DomainService(
            IDomainRepository domainRepository,
            IUnitOfWork unitOfWork)
        {
            _domainRepository = domainRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Domain.Domain> GetAllDomains()
        {
            return _domainRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public IList<Domain.Domain> GetDomains(int customerId)
        {
            return _domainRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Domain.Domain GetDomain(int id)
        {
            return _domainRepository.GetById(id);
        }

        public string GetDomainPassword(int domain_id)
        {
            return _domainRepository.GetDomainPassword(domain_id);
        }

        public DeleteMessage DeleteDomain(int id)
        {
            var domain = _domainRepository.GetById(id);

            if (domain != null)
            {
                try
                {
                    _domainRepository.Delete(domain);
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
                _domainRepository.Add(domain);
            else
                _domainRepository.Update(domain);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
