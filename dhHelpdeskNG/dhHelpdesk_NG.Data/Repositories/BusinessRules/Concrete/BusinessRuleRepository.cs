using DH.Helpdesk.Common.Types;
using DH.Helpdesk.Dal.NewInfrastructure;
using System.Data.Entity;

namespace DH.Helpdesk.Dal.Repositories.BusinessRules.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.BusinessData.Models.BusinessRules;
    using DH.Helpdesk.Domain.BusinessRules;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Common.Extensions.Integer;
    using System;
    using DH.Helpdesk.Common.Enums.BusinessRule;
    using DH.Helpdesk.Common.Constants;

    public sealed class BusinessRuleRepository: Repository, IBusinessRuleRepository
    {
        #region Constructors and Destructors

        public BusinessRuleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        public string SaveBusinessRule(BusinessRuleModel businessRule)
        {
            var isNew = businessRule.Id == 0;
            try
            {                
                if (isNew)
                {
                    var ruleEntity = new BRRuleEntity()
                    {
                        Id = businessRule.Id,
                        Customer_Id = businessRule.CustomerId,
                        Event_Id = businessRule.EventId,
                        Name = businessRule.RuleName,
                        Sequence = businessRule.RuleSequence,
                        ContinueOnSuccess = businessRule.ContinueOnSuccess,
                        ContinueOnError = businessRule.ContinueOnError,
                        CreatedTime = businessRule.CreatedTime,
                        ChangedTime = businessRule.ChangedTime,
                        CreatedByUser_Id = businessRule.CreatedByUserId,
                        ChangedByUser_Id = businessRule.ChangedByUserId,
                        Status = businessRule.RuleActive.ToInt()
                    };

                    this.DbContext.BRRules.Add(ruleEntity);
                    this.InitializeAfterCommit(businessRule, ruleEntity);                    
                }
                else
                {
                    var ruleEntity = this.DbContext.BRRules.Find(businessRule.Id);
                    
                    ruleEntity.Event_Id = businessRule.EventId;
                    ruleEntity.Name = businessRule.RuleName;
                    ruleEntity.Sequence = businessRule.RuleSequence;
                    ruleEntity.ContinueOnSuccess = businessRule.ContinueOnSuccess;
                    ruleEntity.ContinueOnError = businessRule.ContinueOnError;
                    ruleEntity.ChangedTime = businessRule.ChangedTime;
                    ruleEntity.ChangedByUser_Id = businessRule.ChangedByUserId;
                    ruleEntity.Status = businessRule.RuleActive.ToInt();                                                
                }
                this.Commit();
            }
            catch (Exception ex)
            {
                return ex.Message + " " + ex.InnerException != null? ex.InnerException.Message: "";
            }

            var conResult = SaveBRConditions(businessRule, isNew);
            if (conResult != "")
                return conResult;

            if (businessRule.EventId == (int)BREventType.OnSaveCaseAfter)
            {
                var actResult = SaveBRActionsSendEmail(businessRule, isNew);
                if (actResult != "")
                    return actResult;
            }
            else if (businessRule.EventId == (int)BREventType.OnCreateCaseM2T)
            {
                var actResult = SaveBRActionsEditCaseField(businessRule, isNew);
                if (actResult != "")
                    return actResult;
            }

            else if ((businessRule.EventId == (int)BREventType.OnLoadCase) || (businessRule.EventId == (int)BREventType.OnSaveCaseBefore))
            {
                var actResult = SaveBRActionsDisableCaseField(businessRule, isNew);
                if (actResult != "")
                    return actResult;
            }




            return string.Empty;
        }

        private string SaveBRConditions(BusinessRuleModel businessRule, bool isNew)
        {
            try
            {                
                if (isNew)
                {
                    var conditionEntity1 = new BRConditionEntity()
                    {
                        Id = 0,
                        Rule_Id = businessRule.Id,
                        Field_Id = BRFieldType.Process,
                        FromValue = businessRule.ProcessFrom.GetSelectedStr(),
                        ToValue = businessRule.ProcessTo.GetSelectedStr(),
                        Sequence = 1
                    };
                    this.DbContext.BRConditions.Add(conditionEntity1);
                    
                    var conditionEntity2 = new BRConditionEntity()
                    {
                        Id = 0,
                        Rule_Id = businessRule.Id,
                        Field_Id = BRFieldType.SubStatus,
                        FromValue = businessRule.SubStatusFrom.GetSelectedStr(),
                        ToValue = businessRule.SubStatusTo.GetSelectedStr(),
                        Sequence = 2
                    };
                    this.DbContext.BRConditions.Add(conditionEntity2);

                    var conditionEntity3 = new BRConditionEntity()
                    {
                        Id = 0,
                        Rule_Id = businessRule.Id,
                        Field_Id = BRFieldType.Domain,
                        FromValue = businessRule.DomainFrom == null ? "": businessRule.DomainFrom,
                        ToValue = businessRule.DomainTo == null ? "" : businessRule.DomainTo,
                        Sequence = 3
                    };
                    this.DbContext.BRConditions.Add(conditionEntity3);

                    var conditionEntity4 = new BRConditionEntity()
                    {
                        Id = 0,
                        Rule_Id = businessRule.Id,
                        Field_Id = BRFieldType.Status,
                        FromValue = businessRule.StatusFrom.GetSelectedStr(),
                        ToValue = businessRule.StatusTo.GetSelectedStr(),
                        Sequence = 4
                    };
                    this.DbContext.BRConditions.Add(conditionEntity4);
                }
                else
                {
                    #region Save Process
                    var conditionEntity1 = this.DbContext.BRConditions.Where(c=> c.Rule_Id == businessRule.Id && c.Field_Id == BRFieldType.Process)
                                                                      .FirstOrDefault();
                    
                    if (conditionEntity1 == null)
                    {
                        conditionEntity1 = new BRConditionEntity()
                        {
                            Id = 0,
                            Rule_Id = businessRule.Id,
                            Field_Id = BRFieldType.Process,
                            FromValue = businessRule.ProcessFrom.GetSelectedStr(),
                            ToValue = businessRule.ProcessTo.GetSelectedStr(),
                            Sequence = 1
                        };

                        this.DbContext.BRConditions.Add(conditionEntity1);
                    }
                    else
                    {
                        conditionEntity1.FromValue = businessRule.ProcessFrom.GetSelectedStr();
                        conditionEntity1.ToValue = businessRule.ProcessTo.GetSelectedStr();                                                
                    }

                    #endregion

                    #region Save SubStatus
                    var conditionEntity2 = this.DbContext.BRConditions.Where(c=> c.Rule_Id == businessRule.Id && c.Field_Id == BRFieldType.SubStatus)
                                                                      .FirstOrDefault();
                    
                    if (conditionEntity2 == null)
                    {
                        conditionEntity2 = new BRConditionEntity()
                        {
                            Id = 0,
                            Rule_Id = businessRule.Id,
                            Field_Id = BRFieldType.SubStatus,
                            FromValue = businessRule.SubStatusFrom.GetSelectedStr(),
                            ToValue = businessRule.SubStatusTo.GetSelectedStr(),
                            Sequence = 2
                        };

                        this.DbContext.BRConditions.Add(conditionEntity2);
                    }
                    else
                    {
                        conditionEntity2.FromValue = businessRule.SubStatusFrom.GetSelectedStr();
                        conditionEntity2.ToValue = businessRule.SubStatusTo.GetSelectedStr();                                                
                    }

                    #endregion

                    #region Save Domain
                    var conditionEntity3 = this.DbContext.BRConditions.Where(c => c.Rule_Id == businessRule.Id && c.Field_Id == BRFieldType.Domain)
                                                                      .FirstOrDefault();

                    if (conditionEntity3 == null)
                    {
                        conditionEntity3 = new BRConditionEntity()
                        {
                            Id = 0,
                            Rule_Id = businessRule.Id,
                            Field_Id = BRFieldType.Domain,
                            FromValue = businessRule.DomainFrom == null ? "" : businessRule.DomainFrom,
                            ToValue = businessRule.DomainTo == null ? "" : businessRule.DomainTo,
                            Sequence = 3
                        };

                        this.DbContext.BRConditions.Add(conditionEntity3);
                    }
                    else
                    {
                        conditionEntity3.FromValue = businessRule.DomainFrom == null ? "" : businessRule.DomainFrom;
                        conditionEntity3.ToValue = businessRule.DomainTo == null ? "" : businessRule.DomainTo;
                    }
                    #endregion

                    #region Save Status
                    var conditionEntity4 = this.DbContext.BRConditions.Where(c => c.Rule_Id == businessRule.Id && c.Field_Id == BRFieldType.Status)
                                                                      .FirstOrDefault();

                    if (conditionEntity4 == null)
                    {
                        conditionEntity4 = new BRConditionEntity()
                        {
                            Id = 0,
                            Rule_Id = businessRule.Id,
                            Field_Id = BRFieldType.Status,
                            FromValue = businessRule.StatusFrom.GetSelectedStr(),
                            ToValue = businessRule.StatusTo.GetSelectedStr(),
                            Sequence = 4
                        };

                        this.DbContext.BRConditions.Add(conditionEntity4);
                    }
                    else
                    {
                        conditionEntity4.FromValue = businessRule.StatusFrom.GetSelectedStr();
                        conditionEntity4.ToValue = businessRule.StatusTo.GetSelectedStr();
                    }

                    #endregion

                }

                this.Commit();
            }
            catch (Exception ex)
            {
                return ex.Message + " " + (ex.InnerException != null? ex.InnerException.Message: "");
            }
            
            return string.Empty;
        }

        private string SaveBRActionsSendEmail(BusinessRuleModel businessRule, bool isNew)
        {
            try
            {
                if (isNew)
                {
                    var actionEntity1 = new BRActionEntity()
                    {
                        Id = 0,
                        Rule_Id = businessRule.Id,
                        ActionType_Id = BRActionType.SendEmail,
                        Sequence = 1
                    };
                    this.DbContext.BRActions.Add(actionEntity1);                             
                }
                else
                {
                    #region Save Action
                    var actionEntity1 = this.DbContext.BRActions.Where(a => a.Rule_Id == businessRule.Id && a.ActionType_Id == BRActionType.SendEmail)
                                                                   .FirstOrDefault();

                    if (actionEntity1 == null)
                    {
                        var conditionEntity1 = new BRActionEntity()
                        {
                            Id = 0,
                            Rule_Id = businessRule.Id,
                            ActionType_Id = BRActionType.SendEmail,
                            Sequence = 1
                        };
                        this.DbContext.BRActions.Add(actionEntity1);  
                    }                             
                    #endregion                                        
                }
                this.Commit();

                var resultActionParam = SaveBRActionParamsSendEmail(businessRule, isNew);
                if (resultActionParam != "")
                    return resultActionParam;
            }
            catch (Exception ex)
            {
                return ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : "");
            }

            return string.Empty;
        }

        private string SaveBRActionsDisableCaseField(BusinessRuleModel businessRule, bool isNew)
        {
            try
            {
                if (isNew)
                {
                    var actionEntity1 = new BRActionEntity()
                    {
                        Id = 0,
                        Rule_Id = businessRule.Id,
                        ActionType_Id = BRActionType.DisableCaseField,
                        Sequence = 1
                    };
                    this.DbContext.BRActions.Add(actionEntity1);
                }
                else
                {
                    #region Save Action
                    var actionEntity1 = this.DbContext.BRActions.Where(a => a.Rule_Id == businessRule.Id && a.ActionType_Id == BRActionType.DisableCaseField)
                                                                   .FirstOrDefault();

                    if (actionEntity1 == null)
                    {
                        var conditionEntity1 = new BRActionEntity()
                        {
                            Id = 0,
                            Rule_Id = businessRule.Id,
                            ActionType_Id = BRActionType.DisableCaseField,
                            Sequence = 1
                        };
                        this.DbContext.BRActions.Add(actionEntity1);
                    }
                    #endregion
                }
                this.Commit();

                var resultActionParam = SaveBRActionParamsDisableCaseField(businessRule, isNew);
                if (resultActionParam != "")
                    return resultActionParam;
            }
            catch (Exception ex)
            {
                return ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : "");
            }

            return string.Empty;
        }

        private string SaveBRActionsEditCaseField(BusinessRuleModel businessRule, bool isNew)
        {
            try
            {
                if (isNew)
                {
                    var actionEntity1 = new BRActionEntity()
                    {
                        Id = 0,
                        Rule_Id = businessRule.Id,
                        ActionType_Id = BRActionType.EditCaseField,
                        Sequence = 1
                    };
                    this.DbContext.BRActions.Add(actionEntity1);
                }
                else
                {
                    #region Save Action
                    var actionEntity1 = this.DbContext.BRActions.Where(a => a.Rule_Id == businessRule.Id && a.ActionType_Id == BRActionType.EditCaseField)
                                                                   .FirstOrDefault();

                    if (actionEntity1 == null)
                    {
                        var conditionEntity1 = new BRActionEntity()
                        {
                            Id = 0,
                            Rule_Id = businessRule.Id,
                            ActionType_Id = BRActionType.EditCaseField,
                            Sequence = 1
                        };
                        this.DbContext.BRActions.Add(actionEntity1);
                    }
                    #endregion
                }
                this.Commit();

                var resultActionParam = SaveBRActionParamsEditCaseField(businessRule, isNew);
                if (resultActionParam != "")
                    return resultActionParam;
            }
            catch (Exception ex)
            {
                return ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : "");
            }

            return string.Empty;
        }

        private string SaveBRActionParamsSendEmail(BusinessRuleModel businessRule, bool isNew)
        {
            var action = this.DbContext.BRActions.Where(a => a.Rule_Id == businessRule.Id && a.ActionType_Id == BRActionType.SendEmail)
                                                 .FirstOrDefault();

            try
            {
                if (isNew)
                {
                    var actionParamEntity1 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.EMailTemplate,
                        ParamValue = businessRule.EmailTemplate.ToString()                       
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity1);                                       

                    var actionParamEntity2 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.EmailGroup,
                        ParamValue = businessRule.EmailGroups.GetSelectedStr()
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity2);  

                    var actionParamEntity3 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.WorkingGroup,
                        ParamValue = businessRule.WorkingGroups.GetSelectedStr()
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity3);  

                    var actionParamEntity4 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.Administrator,
                        ParamValue = businessRule.Administrators.GetSelectedStr()
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity4);  

                    var actionParamEntity5 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.Recipients,
                        ParamValue = string.Join(BRConstItem.Email_Separator, businessRule.Recipients)
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity5);

                    var actionParamEntity6 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.CaseCreator,
                        ParamValue = businessRule.CaseCreator.ToInt().ToString()
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity6);

                    var actionParamEntity7 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.Initiator,
                        ParamValue = businessRule.Initiator.ToInt().ToString()
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity7);

                    var actionParamEntity8 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.CaseIsAbout,
                        ParamValue = businessRule.CaseIsAbout.ToInt().ToString()
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity8); 
                }
                else
                {                    
                    var actionParamEntity1 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.EMailTemplate)
                                                                        .FirstOrDefault();
                    if (actionParamEntity1 == null)
                    {
                        actionParamEntity1 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.EMailTemplate,
                            ParamValue = businessRule.EmailTemplate.ToString()                       
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity1);  
                    }else{
                        actionParamEntity1.ParamValue = businessRule.EmailTemplate.ToString();
                    }

                    var actionParamEntity2 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.EmailGroup)
                                                                       .FirstOrDefault();
                    if (actionParamEntity2 == null)
                    {
                        actionParamEntity2 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.EmailGroup,
                            ParamValue = businessRule.EmailGroups.GetSelectedStr()
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity2);
                    }
                    else
                    {
                        actionParamEntity2.ParamValue = businessRule.EmailGroups.GetSelectedStr();
                    }

                    var actionParamEntity3 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.WorkingGroup)
                                                                       .FirstOrDefault();
                    if (actionParamEntity3 == null)
                    {
                        actionParamEntity3 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.WorkingGroup,
                            ParamValue = businessRule.WorkingGroups.GetSelectedStr()
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity3);
                    }
                    else
                    {
                        actionParamEntity3.ParamValue = businessRule.WorkingGroups.GetSelectedStr();
                    }

                    var actionParamEntity4 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.Administrator)
                                                                       .FirstOrDefault();
                    if (actionParamEntity4 == null)
                    {
                        actionParamEntity4 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.Administrator,
                            ParamValue = businessRule.Administrators.GetSelectedStr()
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity4);
                    }
                    else
                    {
                        actionParamEntity4.ParamValue = businessRule.Administrators.GetSelectedStr();
                    }

                    var actionParamEntity5 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.Recipients)
                                                                       .FirstOrDefault();
                    if (actionParamEntity5 == null)
                    {
                        actionParamEntity5 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.Recipients,
                            ParamValue = string.Join(BRConstItem.Email_Separator, businessRule.Recipients)
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity5);
                    }
                    else
                    {
                        actionParamEntity5.ParamValue = string.Join(BRConstItem.Email_Separator, businessRule.Recipients);
                    }

                    var actionParamEntity6 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.CaseCreator)
                                                                          .FirstOrDefault();
                    if (actionParamEntity6 == null)
                    {
                        actionParamEntity6 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.CaseCreator,
                            ParamValue = businessRule.CaseCreator.ToInt().ToString()
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity6);
                    }
                    else
                    {
                        actionParamEntity6.ParamValue = businessRule.CaseCreator.ToInt().ToString();
                    }

                    var actionParamEntity7 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.Initiator)
                                                                       .FirstOrDefault();
                    if (actionParamEntity7 == null)
                    {
                        actionParamEntity7 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.Initiator,
                            ParamValue = businessRule.Initiator.ToInt().ToString()
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity7);
                    }
                    else
                    {
                        actionParamEntity7.ParamValue = businessRule.Initiator.ToInt().ToString();
                    }

                    var actionParamEntity8 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.CaseIsAbout)
                                                                       .FirstOrDefault();
                    if (actionParamEntity8 == null)
                    {
                        actionParamEntity8 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.CaseIsAbout,
                            ParamValue = businessRule.CaseIsAbout.ToInt().ToString()
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity8);
                    }
                    else
                    {
                        actionParamEntity8.ParamValue = businessRule.CaseIsAbout.ToInt().ToString();
                    }
                }
                this.Commit();
            }
            catch (Exception ex)
            {
                return ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : "");
            }

            return string.Empty;
        }

        private string SaveBRActionParamsDisableCaseField(BusinessRuleModel businessRule, bool isNew)
        {

            var action = this.DbContext.BRActions.Where(a => a.Rule_Id == businessRule.Id && a.ActionType_Id == BRActionType.DisableCaseField)
                                                .FirstOrDefault();

            try
            {
                if (isNew)
                {


                    var actionParamEntity4 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.DisableFinishingType,
                        ParamValue = businessRule.DisableFinishingType.ToInt().ToString()
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity4);


                }
                else
                {


                    var actionParamEntity4 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.DisableFinishingType)
                                                                       .FirstOrDefault();
                    if (actionParamEntity4 == null)
                    {
                        actionParamEntity4 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.DisableFinishingType,
                            ParamValue = businessRule.DisableFinishingType.ToInt().ToString()
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity4);
                    }
                    else
                    {
                        actionParamEntity4.ParamValue = businessRule.DisableFinishingType.ToInt().ToString();
                    }


                }
                this.Commit();
            }
            catch (Exception ex)
            {
                return ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : "");
            }

            return string.Empty;

        }

        private string SaveBRActionParamsEditCaseField(BusinessRuleModel businessRule, bool isNew)
        {
            var action = this.DbContext.BRActions.Where(a => a.Rule_Id == businessRule.Id && a.ActionType_Id == BRActionType.EditCaseField)
                                                 .FirstOrDefault();

            try
            {
                if (isNew)
                {
                   

                    var actionParamEntity4 = new BRActionParamEntity()
                    {
                        Id = 0,
                        RuleAction_Id = action.Id,
                        ParamType_Id = BRActionParamType.Administrator,
                        ParamValue = businessRule.Administrators.GetSelectedStr()
                    };
                    this.DbContext.BRActionParams.Add(actionParamEntity4);

                   
                }
                else
                {
                   

                    var actionParamEntity4 = this.DbContext.BRActionParams.Where(a => a.RuleAction_Id == action.Id && a.ParamType_Id == BRActionParamType.Administrator)
                                                                       .FirstOrDefault();
                    if (actionParamEntity4 == null)
                    {
                        actionParamEntity4 = new BRActionParamEntity()
                        {
                            Id = 0,
                            RuleAction_Id = action.Id,
                            ParamType_Id = BRActionParamType.Administrator,
                            ParamValue = businessRule.Administrators.GetSelectedStr()
                        };
                        this.DbContext.BRActionParams.Add(actionParamEntity4);
                    }
                    else
                    {
                        actionParamEntity4.ParamValue = businessRule.Administrators.GetSelectedStr();
                    }

                   
                }
                this.Commit();
            }
            catch (Exception ex)
            {
                return ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : "");
            }

            return string.Empty;
        }

        public IList<BusinessRuleModel> GetRules(int customerId)
        {
            var ret = new List<BusinessRuleModel>();
            var ruleEntities = this.DbContext.BRRules.Where(r => r.Customer_Id == customerId).ToList();
            foreach (var ruleEntity in ruleEntities)
                ret.Add(GetRule(ruleEntity.Id));

            return ret;
        }

        public BusinessRuleModel GetRule(int ruleId)
        {
            var ret = new BusinessRuleModel();

            var ruleEntity = this.DbContext.BRRules.Where(r => r.Id == ruleId).FirstOrDefault();
            if (ruleEntity != null)
            {
                ret.Id = ruleEntity.Id;
                ret.CustomerId = ruleEntity.Customer_Id;
                ret.EventId = ruleEntity.Event_Id;
                ret.RuleName = ruleEntity.Name;
                ret.ContinueOnSuccess = ruleEntity.ContinueOnSuccess;
                ret.ContinueOnError = ruleEntity.ContinueOnError;
                ret.RuleSequence = ruleEntity.Sequence;
                ret.RuleActive = ruleEntity.Status.ToBool();
                ret.CreatedByUserId = ruleEntity.CreatedByUser_Id;
                ret.CreatedTime = ruleEntity.CreatedTime;
                ret.ChangedByUserId = ruleEntity.ChangedByUser_Id;
                ret.ChangedTime = ruleEntity.ChangedTime;

                #region conditions
                var conditionEntity1 = this.DbContext.BRConditions.Where(c => c.Rule_Id == ruleId && c.Field_Id == BRFieldType.Process).FirstOrDefault();
                if (conditionEntity1 != null)
                {
                    ret.ProcessFrom.AddItems(conditionEntity1.FromValue, false);
                    ret.ProcessTo.AddItems(conditionEntity1.ToValue, false);
                }

                var conditionEntity2 = this.DbContext.BRConditions.Where(c => c.Rule_Id == ruleId && c.Field_Id == BRFieldType.SubStatus).FirstOrDefault();
                if (conditionEntity2 != null)
                {
                    ret.SubStatusFrom.AddItems(conditionEntity2.FromValue, false);
                    ret.SubStatusTo.AddItems(conditionEntity2.ToValue, false);
                }

                var conditionEntity3 = this.DbContext.BRConditions.Where(c => c.Rule_Id == ruleId && c.Field_Id == BRFieldType.Domain).FirstOrDefault();
                if (conditionEntity3!= null)
                {
                    ret.DomainFrom = conditionEntity3.FromValue;
                    ret.DomainTo = conditionEntity3.ToValue;
                }

                var conditionEntity4 = this.DbContext.BRConditions.Where(c => c.Rule_Id == ruleId && c.Field_Id == BRFieldType.Status).FirstOrDefault();
                if (conditionEntity4 != null)
                {
                    ret.StatusFrom.AddItems(conditionEntity4.FromValue, false);
                    ret.StatusTo.AddItems(conditionEntity4.ToValue, false);
                }

                #endregion

                #region actions
                char[] _SEPARATOR = {';'};

                var actionEntitySendEmail = this.DbContext.BRActions.Where(a => a.Rule_Id == ruleId && a.ActionType_Id == BRActionType.SendEmail).FirstOrDefault();
                if (actionEntitySendEmail != null)
                {
                    var actionParams = this.DbContext.BRActionParams.Where(p => p.RuleAction_Id == actionEntitySendEmail.Id).ToList();
                    foreach(var param in actionParams)
                    {
                        switch (param.ParamType_Id)
                        {
                            case BRActionParamType.EMailTemplate:
                                var val = -1;
                                int.TryParse(param.ParamValue, out val);
                                ret.EmailTemplate = val;
                                break;

                            case BRActionParamType.EmailGroup:
                                ret.EmailGroups.AddItems(param.ParamValue, false);
                                break;

                            case BRActionParamType.WorkingGroup:
                                ret.WorkingGroups.AddItems(param.ParamValue, false);
                                break;

                            case BRActionParamType.Administrator:
                                ret.Administrators.AddItems(param.ParamValue, false);
                                break;

                            case BRActionParamType.Recipients:
                                ret.Recipients = param.ParamValue.Split(_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case BRActionParamType.CaseCreator:
                                ret.CaseCreator = Int32.Parse(param.ParamValue).ToBool();
                                break;
                            case BRActionParamType.Initiator:
                                ret.Initiator = Int32.Parse(param.ParamValue).ToBool();
                                break;
                            case BRActionParamType.CaseIsAbout:
                                ret.CaseIsAbout = Int32.Parse(param.ParamValue).ToBool();
                                break;
                        }                    
                    }
                }

                var actionEntityEditCaseField = this.DbContext.BRActions.Where(a => a.Rule_Id == ruleId && a.ActionType_Id == BRActionType.EditCaseField).FirstOrDefault();
                if (actionEntityEditCaseField != null)
                {
                    var actionParams = this.DbContext.BRActionParams.Where(p => p.RuleAction_Id == actionEntityEditCaseField.Id).ToList();
                    foreach (var param in actionParams)
                    {
                        switch (param.ParamType_Id)
                        {
                            
                            case BRActionParamType.Administrator:
                                ret.Administrators.AddItems(param.ParamValue, false);
                                break;

                        }
                    }
                }

                var actionEntityDisableCaseField = this.DbContext.BRActions.Where(a => a.Rule_Id == ruleId && a.ActionType_Id == BRActionType.DisableCaseField).FirstOrDefault();
                if (actionEntityDisableCaseField != null)
                {
                    var actionParams = this.DbContext.BRActionParams.Where(p => p.RuleAction_Id == actionEntityDisableCaseField.Id).ToList();
                    foreach (var param in actionParams)
                    {
                        switch (param.ParamType_Id)
                        {

                            case BRActionParamType.DisableFinishingType:
                                ret.DisableFinishingType = Int32.Parse(param.ParamValue).ToBool();
                                break;

                        }
                    }
                }

                #endregion

                return ret;
            }
            
            return null;       
        }

        public IList<BusinessRuleModel> GetRules(int customerId, BREventType occurredEvent)
        {
            var ret = new List<BusinessRuleModel>();
            var ruleEntities = this.DbContext.BRRules.Where(r => r.Customer_Id == customerId && r.Event_Id == (int)occurredEvent).OrderBy(x => x.Sequence).ToList();
            foreach (var ruleEntity in ruleEntities)
                ret.Add(GetRule(ruleEntity.Id));

            return ret;
        }

        public IList<BRRuleEntity> GetRuleReadList(int customerId)
        {
            return DbContext.BRRules
                .Include(x => x.BrActions)
                .Include(x => x.BrActions.Select(y => y.BrActionParams))
                .Include(x => x.BrConditions)
                .Include(x => x.ChangedByUser)
                .Include(x => x.CreatedByUser)
                .Where(r => r.Customer_Id == customerId)
                .OrderBy(x => x.Name).ToList();
        }

        public void DeleteRule(int ruleId)
        {
            var ruleEntity = this.DbContext.BRRules.FirstOrDefault(r => r.Id == ruleId);
            if (ruleEntity != null)
            {
                // Since the ON DELETE CASCADE is not set for action parameters, manually delete them.
                var actionIds = this.DbContext.BRActions
                                  .Where(a => a.Rule_Id == ruleId)
                                  .Select(a => a.Id)
                                  .ToList();

                var actionParamEntities = this.DbContext.BRActionParams
                                            .Where(p => actionIds.Contains(p.RuleAction_Id))
                                            .ToList();

                foreach (var actionParamEntity in actionParamEntities)
                {
                    this.DbContext.BRActionParams.Remove(actionParamEntity);
                }

                // Directly remove the rule. This will cascade and remove related rule actions and conditions.
                this.DbContext.BRRules.Remove(ruleEntity);

                this.DbContext.SaveChanges();
            }

            this.Commit();
        }
    }
}