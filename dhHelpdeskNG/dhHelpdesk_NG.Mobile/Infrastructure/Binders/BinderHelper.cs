// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinderHelper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the BinderHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Mobile.Infrastructure.Binders
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
    }
}