namespace dhHelpdesk_NG.Common.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ArgumentsValidator
    {
        public static void IsId(int id, string parameterName)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Must be more than zero.");
            }
        }

        public static void NotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName, "Value cannot be null.");
            }
        }

        public static void NotNullAndEmpty(string parameter, string parameterName)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new ArgumentNullException(parameterName, "Value cannot be null or empty.");
            }
        }

        public static void NotNullAndEmpty<T>(List<T> items, string parameterName)
        {
            if (items == null || !items.Any())
            {
                throw new ArgumentNullException(parameterName, "Value cannot be null or empty.");
            }
        }
    }
}
