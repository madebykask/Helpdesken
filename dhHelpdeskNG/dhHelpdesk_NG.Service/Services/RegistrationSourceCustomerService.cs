using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Services.Services
{    
    using DH.Helpdesk.BusinessData.Models.Status.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IRegistrationSourceCustomerService
    {
        IList<RegistrationSourceCustomer> GetRegistrationSources(int customerId);
        RegistrationSourceCustomer GetRegistrationSouceCustomer(int id);

        void SaveRegistrationSourceCustomer(RegistrationSourceCustomer registrationsourcecustomer, out IDictionary<string, string> errors);
        void Commit();
        DeleteMessage DeleteRegistrationSourceCustomer(int id);
    }

    public class RegistrationSourceCustomerService : IRegistrationSourceCustomerService
    {
        private readonly IRegistrationSourceCustomerRepository _registrationSourceCustomerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationSourceCustomerService(
            IRegistrationSourceCustomerRepository registrationSourceCustomerRepository,
            IUnitOfWork unitOfWork)
        {
            this._registrationSourceCustomerRepository = registrationSourceCustomerRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<RegistrationSourceCustomer> GetRegistrationSources(int customerId)
        {
            return this._registrationSourceCustomerRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.SourceName).ToList();
        }

        public RegistrationSourceCustomer GetRegistrationSouceCustomer(int id)
        {
            return this._registrationSourceCustomerRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteRegistrationSourceCustomer(int id)
        {
            var registrationsourcecustomer = this._registrationSourceCustomerRepository.GetById(id);

            if (registrationsourcecustomer != null)
            {
                try
                {
                    this._registrationSourceCustomerRepository.Delete(registrationsourcecustomer);
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

        public void SaveRegistrationSourceCustomer(RegistrationSourceCustomer registrationsourcecustomer, out IDictionary<string, string> errors)
        {
            if (registrationsourcecustomer == null)
                throw new ArgumentNullException("registrationsourcecustomer");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(registrationsourcecustomer.SourceName))
                errors.Add("RegistratonSourceCustomer.SourceName", "Du måste ange en källa");

            if (registrationsourcecustomer.Id == 0)
                this._registrationSourceCustomerRepository.Add(registrationsourcecustomer);
            else
                this._registrationSourceCustomerRepository.Update(registrationsourcecustomer);

            //if (registrationsourcecustomer.IsDefault == 1)
            //    this._registrationSourceCustomerRepository.ResetDefault(registrationsourcecustomer.Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}

