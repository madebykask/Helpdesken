namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public static class ControllerExtensions
    {
        public static void Merge(this ModelStateDictionary modelState, IDictionary<string, string> dictionary)
        {
            if(modelState == null)
                throw new ArgumentNullException("modelState");
            if(dictionary == null)
                throw new ArgumentNullException("dictionary");

            foreach(var item in dictionary)
                modelState.AddModelError(item.Key, item.Value);
        }
    }
}