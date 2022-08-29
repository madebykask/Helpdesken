namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;

    public interface IAccountActivityService
    {
        IList<AccountActivity> GetAccountActivities(int customerId);
        IList<AccountActivityGroup> GetAccountActivityGroups();

        AccountActivity GetAccountActivity(int id);
        
        DeleteMessage DeleteAccountActivity(int id);

        void SaveAccountActivity(AccountActivity accountActivity, out IDictionary<string, string> errors);
        void Commit();
    }

    public class AccountActivityService : IAccountActivityService
    {
        private readonly IAccountActivityRepository _accountActivityRepository;
        private readonly IAccountActivityGroupRepository _accountActivityGroupRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public AccountActivityService(
            IAccountActivityRepository accountActivityRepository,
            IAccountActivityGroupRepository accountActivityGroupRepository,
            IUnitOfWork unitOfWork)
        {
            this._accountActivityRepository = accountActivityRepository;
            this._accountActivityGroupRepository = accountActivityGroupRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<AccountActivity> GetAccountActivities(int customerId)
        {
            return this._accountActivityRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<AccountActivityGroup> GetAccountActivityGroups()
        {
            return this._accountActivityGroupRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public AccountActivity GetAccountActivity(int id)
        {
            return this._accountActivityRepository.GetById(id);
        }

        public DeleteMessage DeleteAccountActivity(int id)
        {
            var accountActivity = this._accountActivityRepository.GetById(id);

            if (accountActivity != null)
            {
                try
                {
                    this._accountActivityRepository.Delete(accountActivity);
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

        public void SaveAccountActivity(AccountActivity accountActivity, out IDictionary<string, string> errors)
        {
            if (accountActivity == null)
                throw new ArgumentNullException("accountactivity");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(accountActivity.Name))
                errors.Add("AccountActivity.Name", "Du måste ange en kontoaktivitet");

            accountActivity.ChangedDate = DateTime.UtcNow;

            if (accountActivity.Id == 0)
                this._accountActivityRepository.Add(accountActivity);
            else
                this._accountActivityRepository.Update(accountActivity);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
