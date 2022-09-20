namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IRegistrationSourceCustomerService
    {
        IList<RegistrationSourceCustomer> GetRegistrationSources(int customerId);
        RegistrationSourceCustomer GetRegistrationSouceCustomer(int id);

        /// <summary>
        /// Retruns active case sources for specified customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IEnumerable<RegistrationSourceCustomer> GetCustomersActiveRegistrationSources(int customerId);

        void SaveRegistrationSourceCustomer(RegistrationSourceCustomer srcIntance, out IDictionary<string, string> errors);
        void Commit();
        DeleteMessage DeleteRegistrationSourceCustomer(int id);
    }

    public class RegistrationSourceCustomerService : IRegistrationSourceCustomerService
    {
        private readonly IRegistrationSourceCustomerRepository _registrationSourceCustomerRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public RegistrationSourceCustomerService(
            IRegistrationSourceCustomerRepository registrationSourceCustomerRepository,
            IUnitOfWork unitOfWork)
        {
            this._registrationSourceCustomerRepository = registrationSourceCustomerRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<RegistrationSourceCustomer> GetRegistrationSources(int customerId)
        {
            return this._registrationSourceCustomerRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.SourceName).ToList();
        }

        public IEnumerable<RegistrationSourceCustomer> GetCustomersActiveRegistrationSources(int customerId)
        {
            return this._registrationSourceCustomerRepository.GetMany(x => x.Customer_Id == customerId).Where(it => it.IsActive == 1).OrderBy(x => x.SourceName).ToList();
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

        public void SaveRegistrationSourceCustomer(
            RegistrationSourceCustomer srcIntance,
            out IDictionary<string, string> errors)
        {
            if (srcIntance == null)
            {
                throw new ArgumentNullException("srcIntance");
            }

            errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(srcIntance.SourceName))
            {
                errors.Add("RegistratonSourceCustomer.SourceName", "Du måste ange en källa");
            }

            var isCreateNew = srcIntance.Id == 0;
            RegistrationSourceCustomer instanceToWrite;
            if (!isCreateNew)
            {
                instanceToWrite = this._registrationSourceCustomerRepository.GetById(srcIntance.Id);
                if (instanceToWrite == null)
                {
                    throw new ArgumentException("bad instance id");
                }

                instanceToWrite.IsActive = srcIntance.IsActive;
                instanceToWrite.SourceName = srcIntance.SourceName;
                instanceToWrite.SystemCode = srcIntance.SystemCode;
                instanceToWrite.CreatedDate = srcIntance.CreatedDate;
                instanceToWrite.ChangedDate = DateTime.UtcNow;
            }
            else
            {
                instanceToWrite = srcIntance;
                instanceToWrite.CreatedDate = DateTime.UtcNow;
                instanceToWrite.ChangedDate = DateTime.UtcNow;
            }
            
            // check items in DB with the same SystemCode as we have.
            // It is only item with one SystemCode is alowed 
            if (instanceToWrite.SystemCode.HasValue)
            {
                var itemWithSameSystemCode =
                    this._registrationSourceCustomerRepository.GetAll()
                        .Where(
                            it => it.Id != instanceToWrite.Id 
                            && it.Customer_Id == srcIntance.Customer_Id
                            && it.SystemCode.HasValue
                            && it.SystemCode.Value == instanceToWrite.SystemCode.Value)
                        .FirstOrDefault();
                if (itemWithSameSystemCode != null)
                {
                    itemWithSameSystemCode.SystemCode = null;
                    this._registrationSourceCustomerRepository.Update(itemWithSameSystemCode);
                }
            }

            if (isCreateNew)
            {
                this._registrationSourceCustomerRepository.Add(instanceToWrite);
            }
            else
            {
                this._registrationSourceCustomerRepository.Update(instanceToWrite);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}