using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
        private readonly IUnitOfWork _unitOfWork;

        public AccountActivityService(
            IAccountActivityRepository accountActivityRepository,
            IAccountActivityGroupRepository accountActivityGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _accountActivityRepository = accountActivityRepository;
            _accountActivityGroupRepository = accountActivityGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<AccountActivity> GetAccountActivities(int customerId)
        {
            return _accountActivityRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<AccountActivityGroup> GetAccountActivityGroups()
        {
            return _accountActivityGroupRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public AccountActivity GetAccountActivity(int id)
        {
            return _accountActivityRepository.GetById(id);
        }

        public DeleteMessage DeleteAccountActivity(int id)
        {
            var accountActivity = _accountActivityRepository.GetById(id);

            if (accountActivity != null)
            {
                try
                {
                    _accountActivityRepository.Delete(accountActivity);
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
                _accountActivityRepository.Add(accountActivity);
            else
                _accountActivityRepository.Update(accountActivity);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
