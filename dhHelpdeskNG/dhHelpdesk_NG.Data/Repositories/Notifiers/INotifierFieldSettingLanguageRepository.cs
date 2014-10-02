namespace DH.Helpdesk.Dal.Repositories.Notifiers
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.BusinessData.Models;

    public interface INotifierFieldSettingLanguageRepository : IRepository<ComputerUserFieldSettingsLanguage>
    {
        List<Caption> FindByCustomerIdAndLanguageId(int customerId, int languageId);
        IEnumerable<ComputerUserFieldSettingsLanguage> GetComputerUserFieldSettingsLanguage(int? customerId, int? languageId);

        IEnumerable<ComputerUserFieldSettingsLanguageModel> GetComputerUserFieldSettingsWithLanguagesForDefaultCust(int languageId);
    }

    
}
