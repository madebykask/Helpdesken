// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinderHelper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the BinderHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Infrastructure.Binders
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    /// <summary>
    /// The binder helper.
    /// </summary>
    internal static class BinderHelper
    {
        /// <summary>
        /// The parse date.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime ParseDate(ValueProviderResult value)
        {
            try
            {
                var date = (DateTime)value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
                return date;
            }
            catch (Exception)
            {
            }

            try
            {
                var date = (DateTime)value.ConvertTo(typeof(DateTime), CultureInfo.CurrentUICulture);
                return date;
            }
            catch (Exception)
            {
            }

            try
            {
                var date = (DateTime)value.ConvertTo(typeof(DateTime), CultureInfo.InvariantCulture);
                return date;
            }
            catch (Exception)
            {
            }

            throw new ArgumentException(string.Format("Invalid argument: {0}.", value.RawValue));
        }


        public static Decimal ParseDecimal(ValueProviderResult valueResult)
        {
            var actualValue = 0.00M;
            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
                return actualValue;
            }
            catch (FormatException e)
            {
            }

            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentUICulture);
                return actualValue;
            }
            catch (Exception e)
            {
            }

            try
            {
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.InvariantCulture);
                return actualValue;
            }
            catch (Exception e)
            {
            }

            throw new ArgumentException(string.Format("Invalid argument: {0}.", valueResult.RawValue));
        }
    }
}