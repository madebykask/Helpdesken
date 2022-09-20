using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.Domain.Accounts;

    public interface IAccountFieldSettingsService
    {
        
        IList<AccountFieldSettings> GetAccountFieldSettings(int customerId, int? accountactivityId);
        
    }

    public class AccountFieldSettingsService : IAccountFieldSettingsService
    {
        private readonly IAccountFieldSettingsRepository _accountFieldSettingsRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618


#pragma warning disable 0618
        public AccountFieldSettingsService(
            IAccountFieldSettingsRepository accountFieldSettingsRepository,
            IUnitOfWork unitOfWork)
        {
            this._accountFieldSettingsRepository = accountFieldSettingsRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<AccountFieldSettings> GetAccountFieldSettings(int customerId, int? accountactivityId)
        {
            return this._accountFieldSettingsRepository.GetAccountFieldSettingsForMailTemplate(customerId, accountactivityId).ToList();
        }
    }
}
