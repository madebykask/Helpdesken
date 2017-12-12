using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Services.Services
{
    using Common.Extensions.String;
    using Dal.Repositories.Condition;
    using Dal.Repositories.Cases;
    using BusinessData.Models.Condition;
    using Domain;
    using System.Reflection;
    using BusinessLogic.Condition;
    

    public interface IConditionService
    {
        IEnumerable<ConditionModel> GetConditions(int parent_Id, int conditionType_Id);
        ConditionResult CheckConditions(int caseId, int parent_Id, int conditionType_Id);
    }

    public class ConditionService : IConditionService
    {
        private readonly ICacheProvider _cache;
        private readonly IExtendedCaseValueRepository _extendedCaseValueRepository;
        private readonly ICaseService _caseService;
        private readonly IConditionRepository _conditionRepository;

        //TODO: Move to DB
        private const string DateShortFormat = "dd.MM.yyyy";
        private const string DateLongFormat = "d MMMM yyyy";


        public ConditionService(
            ICacheProvider cache,
            IExtendedCaseValueRepository extendedCaseValueRepository,
            ICaseService caseService,
            IConditionRepository conditionRepository
            
            )
        {
            this._cache = cache;
            this._extendedCaseValueRepository = extendedCaseValueRepository;
            this._caseService = caseService;
            this._conditionRepository = conditionRepository;
        }


        
		public IEnumerable<ConditionModel> GetConditions(int parent_Id, int conditionType_Id)
        {
            var conditions = _conditionRepository.GetConditions(parent_Id);

            return conditions;
        }

        //TODO: REFACTOR
        public ConditionResult CheckConditions(int caseId, int parent_Id, int conditionType_Id)
        {
            Case _case = _caseService.GetDetachedCaseById(caseId);

            //If there are more than one conditions, all conditions must be fulfilled

            //IF there are no conditions set, it should be visible         
            var conditions = this.GetConditions(parent_Id, conditionType_Id).ToList();

            bool results = false;
            //IF there are no conditions set, it should be visible
            if (conditions == null || conditions.Count() == 0)
            {
                return new ConditionResult
                {
                    Show = true
                };
            }


            int extendedCaseFormId = 0;
            if (_case.CaseExtendedCaseDatas != null && _case.CaseExtendedCaseDatas.Count > 0)
            {
                extendedCaseFormId = _case.CaseExtendedCaseDatas.First().ExtendedCaseData.ExtendedCaseFormId;
            }

            foreach (var condition in conditions)
            {

                var conditionValue = condition.Values.Tidy().ToLower();
                var conditionKey = condition.Property_Name.Tidy();


                try
                {
                    //var mapper = _caseCaseDocumentTextConditionIdentifierRepository.GetCaseDocumentTextConditionPropertyName(extendedCaseFormId, conditionKey);

                    //if (mapper == null)
                    //{
                    //    throw new CaseDocumentConditionKeyException(extendedCaseFormId, conditionKey, condition.CaseDocumentTextConditionGUID,
                    //        "Could not find condition key for extended case form");
                    //}

                    var conditionProperty = conditionKey;
                    var conditionOperator = condition.Operator;

                    var value = "";

                    //GET FROM EXTENDEDCASE
                    if (conditionKey.ToLower().StartsWith("extendedcase_"))
                    {
                        conditionProperty = conditionProperty.Replace("extendedcase_", "");
                        var ext = _extendedCaseValueRepository.GetExtendedCaseValue(_case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData_Id, conditionProperty);

                        if (ext == null)
                            throw new ConditionPropertyException(extendedCaseFormId, conditionProperty, condition.GUID, $"Could not find key {conditionProperty} in the extended case setup");

                        value = ext.Value;
                    }

                    //Get the specific property of Case in "Property_Name"
                    else if (_case != null && _case.Id != 0 && conditionKey.ToLower().StartsWith("case_"))
                    {
                        if (!conditionProperty.Contains("."))
                        {
                            //Get value from Case Model by casting to dictionary and look for property name
                            value = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[conditionProperty].ToString();
                        }
                        else
                        {
                            var parentClassName = string.Concat(conditionProperty.TakeWhile((c) => c != '.'));
                            var propertyName = conditionProperty.Substring(conditionProperty.LastIndexOf('.') + 1);

                            var parentClass = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[parentClassName];
                            value = parentClass.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(parentClass, null))[propertyName].ToString();
                        }
                    }


                    var evaluator = new ConditionEvaluator();
                    results = evaluator.EvaluateCondition(value, conditionOperator, conditionValue);

                    if (!results)
                        break;
                }
                catch (ConditionBaseException parseEx)
                {
                    return new ConditionResult
                    {
                        Show = false,
                        FieldException = parseEx
                    };
                }
                catch (Exception ex)
                {
                    return new ConditionResult
                    {
                        Show = false,
                        FieldException = new ConditionException(conditionKey, $"Unknown exception when running field condition {conditionKey}")
                    };
                }
            }


            //If there are more than one conditions, all conditions must be fulfilled
            return new ConditionResult
            {
                Show = results
            };
        }


    }
}
