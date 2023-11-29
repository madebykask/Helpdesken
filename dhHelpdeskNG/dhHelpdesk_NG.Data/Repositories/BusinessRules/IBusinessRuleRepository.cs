using DH.Helpdesk.Domain.BusinessRules;

namespace DH.Helpdesk.Dal.Repositories.BusinessRules
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.BusinessData.Models.BusinessRules;
    using DH.Helpdesk.Common.Enums.BusinessRule;

    public interface IBusinessRuleRepository: INewRepository
    {        
        string SaveBusinessRule(BusinessRuleModel businessRule);

        BusinessRuleModel GetRule(int ruleId);

        IList<BusinessRuleModel> GetRules(int customerId);

        IList<BusinessRuleModel> GetRules(int customerId, BREventType ccurredEvent);

        IList<BRRuleEntity> GetRuleReadList(int customerId);

        void DeleteRule(int ruleId);
    }
}