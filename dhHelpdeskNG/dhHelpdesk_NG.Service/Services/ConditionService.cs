using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.Condition;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Services.BusinessLogic.Condition;

namespace DH.Helpdesk.Services.Services
{
    public interface IConditionService
    {
        ConditionResult CheckConditions(int caseId, int parentId, int conditionTypeId);
    }

    public class ConditionService : IConditionService
    {
        private readonly ICaseService _caseService;
        private readonly IConditionRepository _conditionRepository;
        private readonly IExtendedCaseValueRepository _extendedCaseValueRepository;

        private readonly Dictionary<int, List<ExtendedCaseValueEntity>> _extendedCaseDataCache = new Dictionary<int, List<ExtendedCaseValueEntity>>();
        private IList<PropertyInfo> _caseTypePropertiesState = null;
        
        public ConditionService(
            IExtendedCaseValueRepository extendedCaseValueRepository,
            ICaseService caseService,
            IConditionRepository conditionRepository)
        {
            _extendedCaseValueRepository = extendedCaseValueRepository;
            _caseService = caseService;
            _conditionRepository = conditionRepository;
        }
        
        public ConditionResult CheckConditions(int caseId, int parentId, int conditionTypeId)
        {
            //If there are more than one conditions, all conditions must be fulfilled
            var conditions = _conditionRepository.GetConditions(parentId, conditionTypeId);

            //If there are no conditions set, it should be visible
            if (!conditions.Any())
                return ConditionResult.Success();

            var results = false;
            var evaluator = new ConditionEvaluator();

            var _case = _caseService.GetDetachedCaseById(caseId);
            var caseType = _case.GetType();

            var extendedCaseFormId = 0;
            var exCaseData = _case.CaseExtendedCaseDatas?.FirstOrDefault();
            if (exCaseData != null)
            {
                extendedCaseFormId = exCaseData.ExtendedCaseData.ExtendedCaseFormId;
            }
            
            foreach (var condition in conditions)
            {
                var conditionKey = condition.Property_Name.Tidy();
                var conditionValue = condition.Values.Tidy().ToLower();

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
                    if (conditionKey.StartsWith("extendedcase_", StringComparison.OrdinalIgnoreCase))
                    {
                        conditionProperty = conditionProperty.Replace("extendedcase_", "").Trim();

                        var extendedCaseValues =
                            exCaseData != null && exCaseData.ExtendedCaseData_Id > 0
                                ? GetExtendedCaseValuesCached(exCaseData.ExtendedCaseData_Id)
                                : new List<ExtendedCaseValueEntity>();
                        
                        //conditionProperty
                        var ext = 
                            extendedCaseValues.FirstOrDefault(x => x.FieldId.Equals(conditionProperty, StringComparison.OrdinalIgnoreCase));
                            
                        if (ext == null)
                        {
                            var errMsg = $"Could not find key {conditionProperty} in the extended case setup";
                            throw new ExCaseConditionPropertyException(extendedCaseFormId, conditionProperty, condition.GUID, errMsg);
                        }

                        value = ext.Value;
                    }

                    //Get the specific property of Case in "Property_Name"
                    else if (conditionKey.StartsWith("case_", StringComparison.OrdinalIgnoreCase))
                    {
                        conditionProperty = conditionProperty.Replace("case_", "");
                        if (!conditionProperty.Contains("."))
                        {
                            var caseTypePoperties = GetCaseTypeProperties();
                            var propInfo = caseTypePoperties.FirstOrDefault(prop => prop.Name.Equals(conditionProperty, StringComparison.OrdinalIgnoreCase));
                            if (propInfo == null)
                            {
                                var errMsg = $"Could not find key {conditionProperty} in the case type";
                                throw new ConditionPropertyException(conditionProperty, condition.GUID, errMsg);
                            }
                            value = propInfo?.GetValue(_case, null).ToString();
                        }
                        else
                        {
                            var clasNameLiterals = conditionProperty.Split(".");
                            if (clasNameLiterals.Length > 1)
                            {
                                var parentClassName = clasNameLiterals[clasNameLiterals.Length - 2];
                                var propertyName = clasNameLiterals[clasNameLiterals.Length - 1];

                                var parentClass = caseType.GetProperty(parentClassName);
                                if (parentClass == null)
                                {
                                    var errMsg = $"Could not find property {parentClassName} in the case type";
                                    throw new ConditionPropertyException(conditionProperty, condition.GUID, errMsg);
                                }

                                var parentClassInstance = parentClass.GetValue(_case, null);
                                if (parentClassInstance != null)
                                {
                                    var parentClassProperty = parentClassInstance.GetType().GetProperty(propertyName);
                                    if (parentClassProperty == null)
                                    {
                                        var errMsg = $"Could not find property {propertyName} in the {parentClassInstance.GetType().Name} type";
                                        throw new ConditionPropertyException(conditionProperty, condition.GUID, errMsg);
                                    }

                                    value = parentClassProperty.GetValue(parentClassInstance, null).ToString();
                                }
                            }
                        }
                    }

                    results = evaluator.EvaluateCondition(value, conditionOperator, conditionValue);
                    if (!results)
                        break;
                }
                catch (ConditionBaseException parseEx)
                {
                    return ConditionResult.Error(parseEx);
                }
                catch (Exception ex)
                {
                    var errMsg = $"Unknown exception when checking field condition {conditionKey}";
                    return ConditionResult.Error(new ConditionException(conditionKey, errMsg, ex));
                }
            }

            return new ConditionResult(results);
        }

        #region Private Methods

        private List<ExtendedCaseValueEntity> GetExtendedCaseValuesCached(int extendedCaseDataId)
        {
            if (_extendedCaseDataCache.ContainsKey(extendedCaseDataId))
                return _extendedCaseDataCache[extendedCaseDataId];

            var extendedCaseValues =
                _extendedCaseValueRepository.GetMany(x => x.ExtendedCaseDataId == extendedCaseDataId).ToList();

            if (extendedCaseValues.Any())
            {
                _extendedCaseDataCache.Add(extendedCaseDataId, extendedCaseValues);
            }

            return extendedCaseValues;
        }

        private IList<PropertyInfo> GetCaseTypeProperties()
        {
            if (_caseTypePropertiesState == null)
            {
                _caseTypePropertiesState = typeof(Case).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToList();
            }
            return _caseTypePropertiesState;
        }

        #endregion
    }
}