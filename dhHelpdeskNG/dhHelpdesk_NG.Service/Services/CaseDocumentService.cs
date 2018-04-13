using DH.Helpdesk.BusinessData.Models.CaseSolution;
using DH.Helpdesk.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Repositories.CaseDocument;
    using DH.Helpdesk.Dal.Repositories.Cases;
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Domain;
    using System.Reflection;
    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Common.Enums;
    using BusinessData.Enums;
    using BusinessLogic.CaseDocument;
    using System.Text;

    public interface ICaseDocumentService
    {
        CaseDocumentModel GetCaseDocument(Guid caseDocumentGUID, int caseId);
        IList<CaseDocumentModel> GetCaseDocuments(int customerId, Case _case, UserOverview user, ApplicationType applicationType);
    }

    public class CaseDocumentService : ICaseDocumentService
    {
        private readonly ICaseDocumentRepository _caseDocumentRepository;
        private readonly ICaseDocumentConditionRepository _caseDocumentConditionRepository;
        private readonly ICacheProvider _cache;
        private readonly IExtendedCaseValueRepository _extendedCaseValueRepository;
        private readonly ICaseService _caseService;
        private readonly ICaseDocumentTextConditionRepository _caseDocumentTextConditionRepository;
        private readonly ICaseDocumentTextIdentifierRepository _caseCaseDocumentTextIdentifierRepository;
        private readonly ICaseDocumentTextConditionIdentifierRepository _caseCaseDocumentTextConditionIdentifierRepository;

        //TODO: Move to DB
        private const string DateShortFormat = "dd.MM.yyyy";
        private const string DateLongFormat = "d MMMM yyyy";


        public CaseDocumentService(
            ICaseDocumentRepository caseDocumentRepository,
            ICaseDocumentConditionRepository caseDocumentConditionRepository,
            ICacheProvider cache,
            IExtendedCaseValueRepository extendedCaseValueRepository,
            ICaseService caseService,
            ICaseDocumentTextConditionRepository caseDocumentTextConditionRepository,
            ICaseDocumentTextIdentifierRepository caseCaseDocumentTextIdentifierRepository,
            ICaseDocumentTextConditionIdentifierRepository caseCaseDocumentTextConditionIdentifierRepository
            )
        {
            this._caseDocumentRepository = caseDocumentRepository;
            this._caseDocumentConditionRepository = caseDocumentConditionRepository;
            this._cache = cache;
            this._extendedCaseValueRepository = extendedCaseValueRepository;
            this._caseService = caseService;
            this._caseDocumentTextConditionRepository = caseDocumentTextConditionRepository;
            this._caseCaseDocumentTextIdentifierRepository = caseCaseDocumentTextIdentifierRepository;
            this._caseCaseDocumentTextConditionIdentifierRepository = caseCaseDocumentTextConditionIdentifierRepository;
        }


        private bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetExtendedCaseValue(Case _case, string fieldId, string displayName, List<KeyValuePair<string, string>> failedMappings)
        {
            
            try
            {
                //Check for value
                var field = _case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData.ExtendedCaseValues.Where(x => x.FieldId.ToLower() == fieldId.ToLower()).FirstOrDefault();

                if (field == null)
                {
                    var name = displayName.Replace("<", "[").Replace(">", "]");

                    failedMappings.Add(new KeyValuePair<string, string>(displayName, $"Could not find property {fieldId} with display name {name}"));
                    return name;
                }

                //Check if SecondaryValue exist
                var value = field.SecondaryValue;

                if (string.IsNullOrEmpty(value))
                {
                    value = field.Value;
                    if (string.IsNullOrEmpty(value))
                    {
                        var name = displayName.Replace("<", "[").Replace(">", "]");

                        failedMappings.Add(new KeyValuePair<string, string>(displayName, $"Value and secondary value was NULL or empty for property {displayName} with display name [{name}]"));
                        return name;
                    }	
                }

                if (CheckDate(value))
                {
                    
                    //INPUT NEEDS TO BE like: 2017-07-26
                    DateTime convertedDate = DateTime.ParseExact(value, "d", null);
                    return convertedDate.ToString(DateShortFormat, CultureInfo.InvariantCulture);
                }

                return value;
            }
            catch (Exception ex)
            {
                var name = displayName.Replace("<", "").Replace(">", "").Replace("[", "").Replace("]", "");

                failedMappings.Add(new KeyValuePair<string, string>(displayName, $"Could not retrieve {fieldId} with display name [{name}], cause unknown."));

                return name;
                 
            }
        }

        private string GetCaseValue(Case _case, string propertyName, string displayName, List<KeyValuePair<string, string>> failedMappings)
        {


            string value = string.Empty;

            try
            {
                propertyName = propertyName.Replace("Case.", "");

                if (!propertyName.Contains("."))
                {
                    value = _case.GetType().GetProperty(propertyName)?.GetValue(_case, null).ToString();
                }
                else
                {
                    var parentClassName = string.Concat(propertyName.TakeWhile((c) => c != '.'));
                    var parentPropertyName = propertyName.Substring(propertyName.LastIndexOf('.') + 1);

                    var parentClass = _case.GetType().GetProperty(parentClassName)?.GetValue(_case, null);
                    value = parentClass?.GetType().GetProperty(parentPropertyName)?.GetValue(parentClass, null).ToString();
                }

            }
            catch (Exception)
            {
                //Return back propertyname
                value = "[" + displayName.Replace("<", "").Replace(">","").Replace("[", "").Replace("]","") + "]";
                failedMappings.Add(new KeyValuePair<string, string>(propertyName, $"Could not retrieve property of {propertyName} from display name {value}"));
            }

            return value;
        }

        private string ConvertToDisplayFormat(string value, string dataType, string dataFormat, string displayFormat)
        {
            //TODO: Add support for more datatypes
            //only do if there is a value, datatype and displayformat
            if (value.IsNotNullOrEmpty()  && dataType.IsNotNullOrEmpty()  && displayFormat.IsNotNullOrEmpty())

            {
                //for this to be done, we need dataFormat
                if (dataType == "Date" && dataFormat.IsNotNullOrEmpty())
                {
                    if (CheckDate(value))
                    {
                            // base conversion on format from database
                            DateTime convertedDate = DateTime.ParseExact(value, dataFormat, null);
                            value = convertedDate.ToString(displayFormat, CultureInfo.InvariantCulture);
                    }
                }

                if (dataType == "Decimal")
                {
                    decimal outDecimal;
                    if (decimal.TryParse(value.Replace(".", ","), out outDecimal))
                    {
                        value = outDecimal.ToString(displayFormat, CultureInfo.InvariantCulture);
                    }
                }
            }

            return value;
        }

        private Dictionary<string, string> GetCaseValueDictionary(int id, int caseId, List<KeyValuePair<string, string>> failedMappings)
        {
      
            var _case = _caseService.GetCaseById(caseId);

            //Get Identifiers for Case and for ExtendedCase that is connected
            int extendedCaseFormId = 0;
            if (_case.CaseExtendedCaseDatas != null && _case.CaseExtendedCaseDatas.Count > 0)
            {
                extendedCaseFormId = _case.CaseExtendedCaseDatas.First().ExtendedCaseData.ExtendedCaseFormId;
            }

            var identifiers = _caseCaseDocumentTextIdentifierRepository.GetCaseDocumentTextIdentifiers(extendedCaseFormId);


            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in identifiers)
            {
                string propertyName = item.PropertyName;
                //TODO: Translate DisplayName from MasterData
                string displayName = (item.DisplayName != null ? item.DisplayName : item.Identifier);

                if (item.ExtendedCaseFormId == 0)
                {
                    #region  Case or Date
                    if (item.PropertyName.ToLower().Contains("case"))
                    {
                        var value = GetCaseValue(_case, propertyName, displayName, failedMappings);
                        value = ConvertToDisplayFormat(value, item.DataType, item.DataFormat, item.DisplayFormat);
                        dictionary.Add(item.Identifier, value);
                    }
                    else if (item.PropertyName == "Date.NowLong")
                    {
                        //Get from DB, fallback constant DateLongFormat
                        string displayFormat = (string.IsNullOrEmpty(item.DisplayFormat) ? DateLongFormat : item.DisplayFormat);
                        dictionary.Add(item.Identifier, DateTime.Now.ToString(displayFormat, CultureInfo.InvariantCulture));
                    }

                    else if (item.PropertyName == "Date.NowShort")
                    {
                        //Get from DB, fallback constant DateShortFormat
                        string displayFormat = (string.IsNullOrEmpty(item.DisplayFormat) ? DateShortFormat : item.DisplayFormat);
                        dictionary.Add(item.Identifier, DateTime.Now.ToString(displayFormat, CultureInfo.InvariantCulture));
                    }

                    #endregion

                }
                else
                {
                    #region Extended Case

                    if (_case.CaseExtendedCaseDatas != null && _case.CaseExtendedCaseDatas.Count > 0)
                    {

                        var value = GetExtendedCaseValue(_case, propertyName, displayName, failedMappings);
                        value = ConvertToDisplayFormat(value, item.DataType, item.DataFormat, item.DisplayFormat);

                        dictionary.Add(item.Identifier, value);
                    }

                    #endregion
                }
            }

            return dictionary;

        }


        private CaseDocumentReplacePropertiesResult ReplaceWithValue(int id, int caseId, string text)
        {
            var results = new CaseDocumentReplacePropertiesResult();
            results.Original = text;

            var dictionary = GetCaseValueDictionary(id, caseId, results.FailedMappings);

            foreach (var item in dictionary)
            {
                // text = text.Replace("[" + item.Key + "]", item.Value);
                text = text.Replace(item.Key, item.Value);
            }
            results.Results = text;

            return results;
        }

        public CaseDocumentModel GetCaseDocument(Guid caseDocumentGUID, int caseId)
        {
           var _case = _caseService.GetCaseById(caseId);


            var caseDocument = this._caseDocumentRepository.GetCaseDocumentFull(caseDocumentGUID);

            var errorMessageBuilder = new StringBuilder();

            if (caseDocument.CaseDocumentParagraphs != null)
            {
                foreach (var paragraph in caseDocument.CaseDocumentParagraphs)
                {
                    if (paragraph.CaseDocumentTexts != null)
                    {
                        foreach (var paragraphText in paragraph.CaseDocumentTexts)
                        {
                            //TODO: Refactor. For performance, at the moment we are fetching values that dont need to be fetched.
                            var conditionResult = CheckCaseDocumentTextConditions(_case, paragraphText.Conditions);

                            if (conditionResult.Show)
                            {
                                var results = ReplaceWithValue(caseDocument.Id, caseId, paragraphText.Text);

                                var errors =
                                    results.FailedMappings.Count == 0
                                        ? new List<string>()
                                        : results.FailedMappings.Where(o => paragraphText.Text.Contains(o.Key))
                                            .Select(o => $"{o.Key}: {o.Value}")
                                            .ToList();

                                if (errors.Count > 0)
                                {
                                    var errorMessage = errors.Aggregate((o, p) =>
                                    {
                                        var aggr = $"{o}\r\n{p}";
                                        return aggr;
                                    });
                                    errorMessageBuilder.AppendLine($"{paragraphText.Name}:\r\n{errorMessage}\r\n\r\n");
                                }
                                paragraphText.Text = results.Results;

                            }
                            /*else if (conditionResult.FieldException != null)
                            {
                                paragraphText.Text = $"<p style=\"background-color: red\">{conditionResult.FieldException.Message}</p>";
                            }*/
                            else
                            {
                                paragraphText.Text = "";
                            }

                        }
                    }
                }
            }

            caseDocument.Errors = errorMessageBuilder.ToString();

            return caseDocument;
        }
      
        public IList<CaseDocumentModel> GetCaseDocuments(int customerId, Case _case, UserOverview user, ApplicationType applicationType)
        {
            //TODO, do not need to fetch all from customer, filter this out better
            var caseDocuments = 
                _caseDocumentRepository.GetCustomerCaseDocumentsWithConditions(_case.Id, customerId).OrderBy(c => c.SortOrder).ToList();

            var modelList = new List<CaseDocumentModel>();

            var context = new CaseDocumentConditionsContext
            {
                CustomerId = customerId,
                Case = _case,
                //CaseDocument = 
                User = user,
                ApplicationType = applicationType
            };

            foreach (var cd in caseDocuments)
            {
                context.CaseDocument = cd;

                var res = ShowCaseDocument(context);

                if (res.Show)
                {
                    modelList.Add(new CaseDocumentModel
                    {
                        Id = cd.Id,
                        Name = cd.Name,
                        FileType = cd.FileType,
                        SortOrder = cd.SortOrder,
                        CaseId = _case.Id,
                        CaseDocumentGUID = cd.CaseDocumentGUID
                    });
                }
            }

            return modelList;
        }

        //TODO: REFACTOR, this could be used the "same" way as CaseSolutionService.ShowWorkflowStep... 
        private CaseDocumentConditionResult ShowCaseDocument(CaseDocumentConditionsContext ctx)
        {
            //int customerId, Case _case, CaseDocumentOverview caseDocument, UserOverview user, ApplicationType applicationType

            //ALL conditions must be met
            var showDocument = false;

            var caseDocument = ctx.CaseDocument;
            
            //If no conditions are set in (CaseSolutionCondition) for the template, do not show step in list
            var conditions = caseDocument.Conditions;

            if (conditions == null || conditions.Count() == 0)
            {
                return new CaseDocumentConditionResult()
                {
                    Show = false
                };
            }

            var _case = ctx.Case;
            var user = ctx.User;
            
            foreach (var condition in conditions)
            {
                var conditionValue = condition.Values.Tidy().ToLower();
                var conditionKey = condition.Properpty.Tidy();

                try
                {
                    var value = "";

                    //GET FROM APPLICATION
                    if (conditionKey.ToLower() == "application_type")
                    {
                        int appType = (int)((ApplicationType)Enum.Parse(typeof(ApplicationType), ctx.ApplicationType.ToString())); // is it required?
                        value = appType.ToString();
                    }
                    //GET FROM EXTENDEDCASE
                    else if (conditionKey.ToLower().StartsWith("extendedcase_"))
                    {
                        conditionKey = conditionKey.Replace("extendedcase_", "");

                        var extData = _case.CaseExtendedCaseDatas.FirstOrDefault();
                        if (extData != null)
                        {
                            var extDataId = extData.ExtendedCaseData_Id;
                            if (extDataId > 0)
                            {
                                var extValue = _extendedCaseValueRepository.GetExtendedCaseValue(extDataId, conditionKey);
                                if (extValue == null)
                                    throw new CaseDocumentConditionException(conditionKey,
                                        $"Can't get extended case values from condition key {conditionKey} in extended case data ID {extDataId}");
                                value = extValue.Value;
                            }
                        }
                    }
                    //GET FROM USER
                    else if (conditionKey.ToLower().StartsWith("user_"))
                    {
                        conditionKey = conditionKey.Replace("user_", "");
                        //Get value from Model by casting to dictionary and look for property name
                        value = user.GetType().GetProperty(conditionKey)?.GetValue(user, null).ToString();
                    }
                    //Get the specific property of Case in "Property_Name"
                    else if (_case != null && _case.Id != 0 && conditionKey.ToLower().StartsWith("case_"))
                    {
                        conditionKey = conditionKey.Replace("case_", "");

                        if (!conditionKey.Contains("."))
                        {
                            //Get value from Case Model by casting to dictionary and look for property name
                            value = _case.GetType().GetProperty(conditionKey)?.GetValue(_case, null).ToString();
                        }
                        else
                        {
                            var parentClassName = string.Concat(conditionKey.TakeWhile((c) => c != '.'));
                            var propertyName = conditionKey.Substring(conditionKey.LastIndexOf('.') + 1);

                            var parentClass = _case.GetType().GetProperty(parentClassName)?.GetValue(_case, null);
                            value = parentClass?.GetType().GetProperty(propertyName)?.GetValue(parentClass, null).ToString();
                        }
                    }
                    
                    // Check conditions
                    if (!string.IsNullOrEmpty(value) && conditionValue.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) > -1)
                    {
                        showDocument = true;
                        continue;
                    }
                    else
                    {
                        return new CaseDocumentConditionResult
                        {
                            Show = false
                        };
                    }
                }
                catch (CaseDocumentConditionBaseException ex)
                {
                    return new CaseDocumentConditionResult
                    {
                        Show = false,
                        FieldException = ex
                    };
                }
            /*	catch (Exception ex) // TODO: Handle this better
                {
                    return new CaseDocumentConditionResult
                    {
                        Show = false,
                        FieldException = new CaseDocumentConditionException(conditionKey, $"Unkown error when running a condition evaluation of key {conditionKey}")
                    };
                }*/
            }

            //To be true all conditions needs to be fulfilled
            return new CaseDocumentConditionResult
            {
                Show = showDocument
            }; 
        }

        //TODO: REFACTOR, same approach as in CaseSolutionConditions and CaseDocumentConditions
        private CaseDocumentConditionResult CheckCaseDocumentTextConditions(Case _case, IList<CaseDocumentTextConditionModel> conditions)
        {
            //If there are more than one conditions, all conditions must be fulfilled

            //IF there are no conditions set, it should be visible         
            //var conditions = this.GetCaseDocumentTextConditions(caseDocumentText_Id).ToList();

            var results = false;
            //IF there are no conditions set, it should be visible
            if (conditions == null || conditions.Count() == 0)
            {
                return new CaseDocumentConditionResult
                {
                    Show = true
                };
            }
            
            var extendedCaseFormId = 0;
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
                    var mapper = _caseCaseDocumentTextConditionIdentifierRepository.GetCaseDocumentTextConditionPropertyName(extendedCaseFormId, conditionKey);

                    if (mapper == null)
                    {
                        throw new CaseDocumentConditionKeyException(extendedCaseFormId, conditionKey, condition.CaseDocumentTextConditionGUID,
                            "Could not find condition key for extended case form");
                    }

                    var conditionProperty = mapper.PropertyName;
                    var conditionOperator = condition.Operator.Tidy();

                    var value = "";

                    //GET FROM EXTENDEDCASE
                    if (conditionKey.ToLower().StartsWith("extendedcase_"))
                    {
                        //conditionKey = conditionKey.Replace("extendedcase_", "");
                        var ext = _extendedCaseValueRepository.GetExtendedCaseValue(_case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData_Id, conditionProperty);

                        if (ext == null)
                            throw new CaseDocumentConditionPropertyException(extendedCaseFormId, conditionProperty, condition.CaseDocumentTextConditionGUID, $"Could not find key {conditionProperty} in the extended case setup");

                        value = ext.Value;
                    }

                    //Get the specific property of Case in "Property_Name"
                    else if (_case != null && _case.Id != 0 && conditionKey.ToLower().StartsWith("case_"))
                    {
                        if (!conditionProperty.Contains("."))
                        {
                            //Get value from Case Model by casting to dictionary and look for property name
                            value = _case.GetType().GetProperty(conditionProperty)?.GetValue(_case, null).ToString();
                        }
                        else
                        {
                            var parentClassName = string.Concat(conditionProperty.TakeWhile((c) => c != '.'));
                            var propertyName = conditionProperty.Substring(conditionProperty.LastIndexOf('.') + 1);

                            var parentClass = _case.GetType().GetProperty(parentClassName)?.GetValue(_case, null);
                            value = parentClass?.GetType().GetProperty(propertyName)?.GetValue(parentClass, null).ToString();
                        }
                    }


                    var evaluator = new CaseDocumentConditionEvaluator();
                    results = evaluator.EvaluateCondition(value, conditionOperator, conditionValue);

                    if (!results)
                        break;
                }
                catch (CaseDocumentConditionBaseException parseEx)
                {
                    return new CaseDocumentConditionResult
                    {
                        Show = false,
                        FieldException = parseEx
                    };
                }
                catch (Exception ex)
                {
                    return new CaseDocumentConditionResult
                    {
                        Show = false,
                        FieldException = new CaseDocumentConditionException(conditionKey, $"Unknown exception when running field condition {conditionKey}")
                    };
                }
            }


            //If there are more than one conditions, all conditions must be fulfilled
            return new CaseDocumentConditionResult
            {
                Show = results
            };
        }

        public Dictionary<string, string> GetCaseDocumentConditions(int caseDocument_Id)
        {
            var caseDocumentConditions =
                _caseDocumentConditionRepository.GetCaseDocumentConditions(caseDocument_Id)
                    .Select(x => new { x.Property_Name, x.Values })
                    .ToDictionary(x => x.Property_Name, x => x.Values);

            return caseDocumentConditions;
        }

        //public Tuple<string, string,string> GetCaseDocumentTextConditions(int caseDocumentText_Id)
        //{
        //    //.Select(x => new Tuple<app_subjects, bool>(x, false)).ToList();
        //    //.Select(x => new Tuple<>(x.Property_Name, x.Values, x.Operator>)

        //    Tuple<string, string, string> caseDocumentTextConditions = (Tuple<string, string, string>)_caseDocumentTextConditionRepository.GetCaseDocumentTextConditions(caseDocumentText_Id).Select(x => Tuple.Create(x.Property_Name,x.Values,x.Operator));
        //    return caseDocumentTextConditions;
        //}

        public IEnumerable<CaseDocumentTextConditionModel> GetCaseDocumentTextConditions(int caseDocumentText_Id)
        {
            var caseDocumentTextConditions = _caseDocumentTextConditionRepository.GetCaseDocumentTextConditions(caseDocumentText_Id);

            return caseDocumentTextConditions;
        }

        private class CaseDocumentConditionsContext
        {
            public int CustomerId { get; set; }
            public Case Case { get; set; }
            public CaseDocumentOverview CaseDocument { get; set; }
            public UserOverview User { get; set; }
            public ApplicationType ApplicationType { get; set; }
        }
    }
}
