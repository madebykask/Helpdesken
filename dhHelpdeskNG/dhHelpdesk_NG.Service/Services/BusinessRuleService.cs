namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.BusinessRules;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.BusinessRules;
using DH.Helpdesk.Domain;
using System.Collections.Generic;
    

    public interface IBusinessRuleService
    {
        string SaveBusinessRule(BusinessRuleModel businessRule);

        BusinessRuleModel GetRule(int ruleId);

        IList<BusinessRuleModel> GetRules(int customerId);
    }

    public class BusinessRuleService : IBusinessRuleService
    {
        private readonly IBusinessRuleRepository _businessRuleRepository;

        public BusinessRuleService(
            IBusinessRuleRepository businessRuleRepository)
        {
            _businessRuleRepository = businessRuleRepository;
        }

        public string SaveBusinessRule(BusinessRuleModel businessRule)
        {
            var res = _businessRuleRepository.SaveBusinessRule(businessRule);            
            return res;
        }

        public BusinessRuleModel GetRule(int ruleId)
        {
            return _businessRuleRepository.GetRule(ruleId);
        }

        public IList<BusinessRuleModel> GetRules(int customerId)
        {
            return _businessRuleRepository.GetRules(customerId);
        }        
    }
}
