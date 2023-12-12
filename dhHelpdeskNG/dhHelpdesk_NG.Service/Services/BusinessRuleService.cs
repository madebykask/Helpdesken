using System.Linq;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.BusinessRules;
    using DH.Helpdesk.Common.Enums.BusinessRule;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.BusinessRules;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;
    

    public interface IBusinessRuleService
    {
        string SaveBusinessRule(BusinessRuleModel businessRule);

        BusinessRuleModel GetRule(int ruleId);

        IList<BusinessRuleModel> GetRules(int customerId);

        IList<BusinessRuleModel> GetRules(int customerId, BREventType occurredEvent);

        IList<BusinessRuleReadModel> GetRuleReadlist(int customerId);

        DeleteMessage DeleteBusinessRule(int id);
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

        public IList<BusinessRuleModel> GetRules(int customerId, BREventType occurredEvent)
        {
            return _businessRuleRepository.GetRules(customerId, occurredEvent);
        }

        public IList<BusinessRuleReadModel> GetRuleReadlist(int customerId)
        {
            var entities = _businessRuleRepository.GetRuleReadList(customerId);
            return entities.Select(ruleEntity => new BusinessRuleReadModel
            {
                Id = ruleEntity.Id, 
                CustomerId = ruleEntity.Customer_Id, 
                Event = (BREventType) ruleEntity.Event_Id, 
                RuleName = ruleEntity.Name, 
                ContinueOnSuccess = ruleEntity.ContinueOnSuccess, 
                ContinueOnError = ruleEntity.ContinueOnError, 
                RuleSequence = ruleEntity.Sequence, 
                RuleActive = ruleEntity.Status.ToBool(), 
                CreatedBy = new UserName(ruleEntity.CreatedByUser.FirstName, ruleEntity.CreatedByUser.SurName), 
                CreatedTime = ruleEntity.CreatedTime, 
                ChangedBy = new UserName(ruleEntity.ChangedByUser.FirstName, ruleEntity.ChangedByUser.SurName), 
                ChangedTime = ruleEntity.ChangedTime, 
                Conditions = ruleEntity.BrConditions.Select(x => x.Field_Id).ToList(), 
                Actions = ruleEntity.BrActions.SelectMany(x => x.BrActionParams).Select(x => x.ParamValue).ToList()
            }).ToList();
        }

        public DeleteMessage DeleteBusinessRule(int id)
        {
            var businessRule = this._businessRuleRepository.GetRule(id);

            if (businessRule != null)
            {
                try
                {
                    this._businessRuleRepository.DeleteRule(id);

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }
    }
}
