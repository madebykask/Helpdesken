using System;
using System.Linq;
using DH.Helpdesk.Common.Extensions;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.Condition;

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
	public class ConditionEvaluator
	{
		public ConditionEvaluator()
		{
		}

		public bool EvaluateCondition(string value, ConditionOperator conditionOperator, string conditionValues)
		{
			// Ensure never null when comparing values, null is the same as empty
			value = value ?? "";
			conditionValues = conditionValues ?? "";

			//ConditionOperator conditionOp;
			//if (!Enum.TryParse<ConditionOperator>(conditionOperator., out conditionOp))
			//{
			//	throw new ConditionParseException(value, conditionOperator, conditionValues, "conditionKey",
			//		$"Could not parse condition operator {conditionOperator ?? "NULL"}");
			//}

			bool result;
			switch (conditionOperator)
			{
				case ConditionOperator.Equal:
					{
                        ValueCompare compareResult = DoValueCompare(value.ToLower(), conditionOperator, conditionValues.ToLower());
                        result = compareResult == ValueCompare.Equal;
                        break;
                    }
				case ConditionOperator.EqualOrEmpty:
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
				case ConditionOperator.NotEqual:
					{
						ValueCompare compareResult = DoValueCompare(value.ToLower(), conditionOperator, conditionValues.ToLower());
						result = compareResult != ValueCompare.Equal;
						break;
					}
				case ConditionOperator.HasValue:
					{
						result = !string.IsNullOrEmpty(value);
						break;
					}
				case ConditionOperator.IsEmpty:
					{
						result = string.IsNullOrEmpty(value);
						break;
					}
				case ConditionOperator.LargerThan:
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
				case ConditionOperator.LargerThanOrEqual:
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
				case ConditionOperator.LessThan:
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
				case ConditionOperator.LessThanOrEqual:
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
				case ConditionOperator.Exists:
					{
						result = conditionValues.Split(',')
							.Select(o => o.ToLower())
							.Any(o => o == value.ToLower());
						break;
					}
				case ConditionOperator.NotExists:
					{
						result = !conditionValues.Split(',')
							.Select(o => o.ToLower())
							.Any(o => o == value.ToLower());
						break;
					}
				default:
					{
						throw new NotImplementedException($"Condition operator not implemented {conditionOperator}");
					}
			}
			return result;
		}

		private ValueCompare DoValueCompare(string value, ConditionOperator conditionKey, string conditionValues)
		{
			var comparableValue = TryParse(value);
			var comparableConditionValue = TryParse(conditionValues);

			if (comparableValue == null)
			{
				throw new ConditionParseException(value, conditionKey, conditionValues, "value",
					$"Could not parse value {value ?? "NULL"}");
			}
			if (comparableConditionValue == null)
			{
				throw new ConditionParseException(value, conditionKey, conditionValues, "conditionValues",
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
