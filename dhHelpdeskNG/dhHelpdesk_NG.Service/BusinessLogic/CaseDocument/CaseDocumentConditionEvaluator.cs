using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Services.BusinessLogic.CaseDocument
{
	public class CaseDocumentConditionEvaluator
	{
		public CaseDocumentConditionEvaluator()
		{

		}


		public bool EvaluateCondition(string value, string conditionOperator, string conditionValues)
		{
			// Ensure never null when comparing values, null is the same as empty
			value = value ?? "";
			conditionValues = conditionValues ?? "";

			CaseDocumentConditionOperator conditionOp;
			if (!Enum.TryParse<CaseDocumentConditionOperator>(conditionOperator, out conditionOp))
			{
				throw new CaseDocumentConditionParseException(value, conditionOperator, conditionValues, "conditionKey",
					$"Could not parse condition operator {conditionOperator ?? "NULL"}");
			}

			bool result;
			switch (conditionOp)
			{
				case CaseDocumentConditionOperator.Equal:
					{
                        ValueCompare compareResult = DoValueCompare(value.ToLower(), conditionOperator, conditionValues.ToLower());
                        result = compareResult == ValueCompare.Equal;
                        break;
                    }
				case CaseDocumentConditionOperator.EqualOrEmpty:
					{
                        //if empty
                        if (string.IsNullOrEmpty(value))
                        {
                            result = true;
                        }
                        //check if it is equal
                        else
                        {
                            ValueCompare compareResult = DoValueCompare(value.ToLower(), conditionOperator, conditionValues.ToLower());
                            result = compareResult == ValueCompare.Equal;
                        }
                        break;
                    }
				case CaseDocumentConditionOperator.NotEqual:
					{
						ValueCompare compareResult = DoValueCompare(value.ToLower(), conditionOperator, conditionValues.ToLower());
						result = compareResult != ValueCompare.Equal;
						break;
					}
				case CaseDocumentConditionOperator.HasValue:
					{
						result = !string.IsNullOrEmpty(value);
						break;
					}
				case CaseDocumentConditionOperator.IsEmpty:
					{
						result = string.IsNullOrEmpty(value);
						break;
					}
				case CaseDocumentConditionOperator.LargerThan:
					{
						if (string.IsNullOrEmpty(value))
						{
							result = false;
						}
						else
						{
							var compareResult = DoValueCompare(value, conditionOperator, conditionValues);
							result = compareResult == ValueCompare.LargerThan;
						}
						break;
					}
				case CaseDocumentConditionOperator.LargerThanOrEqual:
					{
						if (string.IsNullOrEmpty(value))
						{
							result = false;
						}
						else
						{
							var compareResult = DoValueCompare(value, conditionOperator, conditionValues);
							result = compareResult == ValueCompare.LargerThan || compareResult == ValueCompare.Equal;
						}
						break;
					}
				case CaseDocumentConditionOperator.LessThan:
					{
						if (string.IsNullOrEmpty(value))
						{
							result = false;
						}
						else
						{
							var compareResult = DoValueCompare(value, conditionOperator, conditionValues);
							result = compareResult == ValueCompare.LessThan;
						}
						break;
					}
				case CaseDocumentConditionOperator.LessThanOrEqual:
					{
						if (string.IsNullOrEmpty(value))
						{
							result = false;
						}
						else
						{
							var compareResult = DoValueCompare(value, conditionOperator, conditionValues);
							result = compareResult == ValueCompare.LessThan || compareResult == ValueCompare.Equal;
						}
						break;

					}
				case CaseDocumentConditionOperator.Exists:
					{
						result = conditionValues.Split(',')
							.Select(o => o.ToLower())
							.Any(o => o == value.ToLower());
						break;
					}
				case CaseDocumentConditionOperator.NotExists:
					{
						result = !conditionValues.Split(',')
							.Select(o => o.ToLower())
							.Any(o => o == value.ToLower());
						break;
					}
				default:
					{
						throw new NotImplementedException($"Condition operator not implemented {conditionOp}");
					}
			}
			return result;
		}

		private ValueCompare DoValueCompare(string value, string conditionKey, string conditionValues)
		{
			var comparableValue = TryParse(value);
			var comparableConditionValue = TryParse(conditionValues);

			if (comparableValue == null)
			{
				throw new CaseDocumentConditionParseException(value, conditionKey, conditionValues, "value",
					$"Could not parse value {value ?? "NULL"}");
			}
			if (comparableConditionValue == null)
			{
				throw new CaseDocumentConditionParseException(value, conditionKey, conditionValues, "conditionValues",
					$"Could not parse condition values {conditionValues ?? "NULL"}");
			}
			var compareResult = comparableValue.CompareValueWith(comparableConditionValue);
			return compareResult;
		}


		protected IComparable TryParse(string value)
		{
            // TODO: Add support for other types and refactor
            float compFloat;
            if (float.TryParse(value.Replace(".", ","), out compFloat))
                return compFloat;
            int compInt;
			if (int.TryParse(value, out compInt))
				return compInt;
            DateTime compDate;
			if (DateTime.TryParse(value, out compDate))
				return compDate;
            if (value is string)
                return value;

			throw new NotImplementedException("Type not implemented");
		}
	}
}
