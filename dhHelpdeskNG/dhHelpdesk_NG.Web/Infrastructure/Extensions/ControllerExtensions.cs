namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using DH.Helpdesk.Domain;
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

        public static string ResolveFullName(this ProductArea productArea)
        {
            if (productArea.Parent_ProductArea_Id.HasValue)
                return string.Format("{0} - {1}", ResolveFullName(productArea.ParentProductArea), 
                                     Translation.GetMasterDataTranslation(productArea.Name));
            else
                return Translation.GetMasterDataTranslation(productArea.Name);
        }
    }    
}