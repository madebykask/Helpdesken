// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeBinder.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DateTimeBinder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace DH.Helpdesk.Web.Infrastructure.Binders
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// The date time binder.
    /// </summary>
    internal sealed class DateTimeBinder : IModelBinder
    {
        /// <summary>
        /// The bind model.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="bindingContext">
        /// The binding context.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null || string.IsNullOrEmpty(value.AttemptedValue))
            {
                throw new ArgumentNullException(bindingContext.ModelName);
            }

            try
            {
                return BinderHelper.ParseDate(value);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
                return null;
            }
        }
    }
}