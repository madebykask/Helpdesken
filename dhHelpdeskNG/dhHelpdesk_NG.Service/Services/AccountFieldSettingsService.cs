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
        private readonly IUnitOfWork _unitOfWork;

        public AccountFieldSettingsService(
            IAccountFieldSettingsRepository accountFieldSettingsRepository,
            IUnitOfWork unitOfWork)
        {
            this._accountFieldSettingsRepository = accountFieldSettingsRepository;
            this._unitOfWork = unitOfWork;
        }

        public IList<AccountFieldSettings> GetAccountFieldSettings(int customerId, int? accountactivityId)
        {
            return this._accountFieldSettingsRepository.GetAccountFieldSettingsForMailTemplate(customerId, accountactivityId).ToList();
        }
    }
}
