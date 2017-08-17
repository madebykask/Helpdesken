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
        IEnumerable<CaseDocumentModel> GetCaseDocuments(int customerId, Case _case, UserOverview user, ApplicationType applicationType);
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
					var name = displayName.Replace("<", "").Replace(">", "").Replace("[", "").Replace("]", "");

					failedMappings.Add(new KeyValuePair<string, string>(fieldId, $"Could not find property {fieldId} with display name {name}"));
					return name;
				}

				//Check if SecondaryValue exist
				var value = field.SecondaryValue;

				if (string.IsNullOrEmpty(value))
				{
					value = field.Value;
					if (string.IsNullOrEmpty(value))
					{
						var name = displayName.Replace("<", "").Replace(">", "").Replace("[", "").Replace("]", "");

						failedMappings.Add(new KeyValuePair<string, string>(fieldId, $"Value and secondary value was NULL or empty for property {fieldId} with display name {name}"));
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

				failedMappings.Add(new KeyValuePair<string, string>(fieldId, $"Could not retrieve {fieldId} with display name {name}, cause unknown."));

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
                    //Get value from Case Model by casting to dictionary and look for property name
                    value = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[propertyName].ToString();
                }
                else
                {
                    var parentClassName = string.Concat(propertyName.TakeWhile((c) => c != '.'));
                    var parentPropertyName = propertyName.Substring(propertyName.LastIndexOf('.') + 1);

                    var parentClass = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[parentClassName];
                    value = parentClass.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(parentClass, null))[parentPropertyName].ToString();
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

        private Dictionary<string, string> GetCaseValueDictionary(int id, int caseId, List<KeyValuePair<string, string>> failedMappings)
        {
      
            var _case = _caseService.GetCaseById(caseId);

            //Get Identifiers for Case and for ExtendedCase that is connected

            int extendedCaseFormId = 0;
            if (_case.CaseExtendedCaseDatas != null)
            {
                extendedCaseFormId = _case.CaseExtendedCaseDatas.First().ExtendedCaseData.ExtendedCaseFormId;
            }

            var identifiers = _caseCaseDocumentTextIdentifierRepository.GetCaseDocumentTextIdentifiers(extendedCaseFormId);


            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in identifiers)
            {
                string propertyName = item.PropertyName;
                //TODO: Translate DisplayName from MasterData
                string displayName = (!string.IsNullOrEmpty(item.DisplayName) ? item.DisplayName : item.Identifier);

                if (item.ExtendedCaseFormId == 0)
                {

                    #region  Case or Date
                    if (item.PropertyName.ToLower().Contains("case"))
                    { 
                        dictionary.Add(item.Identifier, GetCaseValue(_case, propertyName, displayName, failedMappings));
                    }
                    else if (item.PropertyName == "Date.NowLong")
                    {
                        dictionary.Add(item.Identifier, DateTime.Now.ToString(DateLongFormat, CultureInfo.InvariantCulture));
                    }

                    else if (item.PropertyName == "Date.NowShort")
                    {
                        dictionary.Add(item.Identifier, DateTime.Now.ToString(DateShortFormat, CultureInfo.InvariantCulture));
                    }

                    #endregion

                }
                else
                {
                    #region Extended Case

                    if (_case.CaseExtendedCaseDatas != null)
                    {
                        dictionary.Add(item.Identifier, GetExtendedCaseValue(_case, propertyName, displayName, failedMappings));
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


            var caseDocument = this._caseDocumentRepository.GetCaseDocument(caseDocumentGUID);

			var errorMessageBuilder = new StringBuilder();

            if (caseDocument.CaseDocumentParagraphs != null)
            {
                foreach (var item in caseDocument.CaseDocumentParagraphs)
                {
                    if (item.CaseDocumentParagraph.CaseDocumentTexts != null)
                    {
                        foreach (var paragraphText in item.CaseDocumentParagraph.CaseDocumentTexts)
                        {
							//TODO: Refactor. For performance, at the moment we are fetching values that dont need to be fetched.
							var conditionResult = CheckCaseDocumentTextConditions(_case, paragraphText.Id);

							if (conditionResult.Show)
                            {
								var results = ReplaceWithValue(caseDocument.Id, caseId, paragraphText.Text);

								var errorMesssage = results.FailedMappings.Count == 0 ? "" :
									results
									.FailedMappings
									.Select(o => $"{o.Value}")
									.Aggregate((o, p) => $"{o}\r\n{p}");

								errorMessageBuilder.AppendLine($"{paragraphText.Name}:\r\n{errorMesssage}\r\n\r\n");
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
      
        public IEnumerable<CaseDocumentModel> GetCaseDocuments(int customerId, Case _case, UserOverview user, ApplicationType applicationType)
        {
            //TODO, do not need to fetch all from customer, filter this out better
            var caseDocuments = this._caseDocumentRepository.GetCaseDocumentsByCustomer(customerId).Select(c => new CaseDocumentModel()
            {
                Id = c.Id,
                Name = c.Name,
                FileType = c.FileType,
                SortOrder = c.SortOrder,
                CaseId = _case.Id,
                CaseDocumentGUID = c.CaseDocumentGUID

            }).OrderBy(c => c.SortOrder).ToList();

            return caseDocuments.Where(c =>
			{
				var result = ShowCaseDocument(customerId, _case, c.Id, user, applicationType);
				return result.Show;
			}).ToList();
        }


		//TODO: REFACTOR, this could be used the "same" way as workflowstep... 
		private CaseDocumentConditionResult ShowCaseDocument(int customerId, Case _case, int caseDocumentId, UserOverview user, ApplicationType applicationType)
		{
			//ALL conditions must be met
			bool showDocument = false;

			//If no conditions are set in (CaseSolutionCondition) for the template, do not show step in list
			var conditions = this.GetCaseDocumentConditions(caseDocumentId);

			if (conditions == null || conditions.Count() == 0)
			{
				return new CaseDocumentConditionResult()
				{
					Show = false
				};
			}

            foreach (var condition in conditions)
            {
                var conditionValue = condition.Value.Tidy().ToLower();
                var conditionKey = condition.Key.Tidy();

				try
				{
					var value = "";

                    //GET FROM APPLICATION
                    if (conditionKey.ToLower() == "application_type")
                    {
                        int appType = (int)((ApplicationType)Enum.Parse(typeof(ApplicationType), applicationType.ToString()));
                        value = appType.ToString();
                    }
                    //GET FROM EXTENDEDCASE
                    else if (conditionKey.ToLower().StartsWith("extendedcase_"))
                    {
                        conditionKey = conditionKey.Replace("extendedcase_", "");
                        value = _extendedCaseValueRepository.GetExtendedCaseValue(_case.CaseExtendedCaseDatas.FirstOrDefault().ExtendedCaseData_Id, conditionKey).Value;
                    }
                    //GET FROM USER
                    else if (conditionKey.ToLower().StartsWith("user_"))
                    {
                        conditionKey = conditionKey.Replace("user_", "");
                        //Get value from Model by casting to dictionary and look for property name
                        value = user.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(user, null))[conditionKey].ToString();
                    }
                    //Get the specific property of Case in "Property_Name"
                    else if (_case != null && _case.Id != 0 && conditionKey.ToLower().StartsWith("case_"))
                    {
                        conditionKey = conditionKey.Replace("case_", "");

                        if (!conditionKey.Contains("."))
                        {
                            //Get value from Case Model by casting to dictionary and look for property name
                            value = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[conditionKey].ToString();
                        }
                        else
                        {
                            var parentClassName = string.Concat(conditionKey.TakeWhile((c) => c != '.'));
                            var propertyName = conditionKey.Substring(conditionKey.LastIndexOf('.') + 1);

                            var parentClass = _case.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(_case, null))[parentClassName];
                            value = parentClass.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(parentClass, null))[propertyName].ToString();
                        }
                    }
                    
                    // Check conditions
                    if (value.Length > 0 && conditionValue.IndexOf(value.ToLower()) > -1)
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
				catch (Exception ex) // TODO: Handle this better
				{
					return new CaseDocumentConditionResult
					{
						Show = false,
						FieldException = new CaseDocumentConditionException(conditionKey, $"Unkown error when running a condition evaluation of key {conditionKey}")
					};
				}
            }

			//To be true all conditions needs to be fulfilled
			return new CaseDocumentConditionResult
			{
				Show = showDocument
			}; 
        }
        //TODO: REFACTOR
        private CaseDocumentConditionResult CheckCaseDocumentTextConditions(Case _case, int caseDocumentText_Id)
        {
            //If there are more than one conditions, all conditions must be fulfilled

            //IF there are no conditions set, it should be visible
            bool showText = true;
         
            var condtions = this.GetCaseDocumentTextConditions(caseDocumentText_Id);

            //IF there are no conditions set, it should be visible
            if (condtions == null || condtions.Count() == 0)
			{
				return new CaseDocumentConditionResult
				{
					Show = true
				};
			}


            int extendedCaseFormId = 0;
            if (_case.CaseExtendedCaseDatas != null)
            {
                extendedCaseFormId = _case.CaseExtendedCaseDatas.First().ExtendedCaseData.ExtendedCaseFormId;
            }

			foreach (var condition in condtions)
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


					var evaluator = new CaseDocumentConditionEvaluator();
					var result = evaluator.EvaluateCondition(value, conditionOperator, conditionValue);

					return new CaseDocumentConditionResult
					{
						Show = result
					};

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
				Show = showText
			};
        }


        public Dictionary<string, string> GetCaseDocumentConditions(int caseDocument_Id)
        {
            Dictionary<string, string> caseDocumentConditions = _caseDocumentConditionRepository.GetCaseDocumentConditions(caseDocument_Id).Select(x => new { x.Property_Name, x.Values }).ToDictionary(x => x.Property_Name, x => x.Values);
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

    }
}
