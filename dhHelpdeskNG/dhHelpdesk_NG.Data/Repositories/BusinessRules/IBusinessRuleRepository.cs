namespace DH.Helpdesk.Dal.Repositories.BusinessRules
{
    using System.Collections.Generic;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.BusinessData.Models.BusinessRules;

    public interface IBusinessRuleRepository: INewRepository
    {        
        string SaveBusinessRule(BusinessRuleModel businessRule);

        BusinessRuleModel GetRuleData(int ruleId);
    }
}