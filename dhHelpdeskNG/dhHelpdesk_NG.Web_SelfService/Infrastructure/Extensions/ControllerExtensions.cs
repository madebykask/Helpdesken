using System.IO;

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
                throw new ArgumentNullException(nameof(modelState));

            if(dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            foreach(var item in dictionary)
            {
                modelState.AddModelError(item.Key, item.Value);
            }
        }

        public static string RenderViewToString(this ControllerBase controller, string viewName, object model, bool partial = true)
        {
            var ctx = controller.ControllerContext;

            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            var viewResult = partial
                ? ViewEngines.Engines.FindPartialView(ctx, viewName)
                : ViewEngines.Engines.FindView(ctx, viewName, null);

            if (viewResult == null || viewResult != null && viewResult.View == null)
                throw new FileNotFoundException($"View '{viewName}' could not be found");

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(ctx, viewResult.View, controller.ViewData, controller.TempData, sw);
                
                // copy model state items to the html helper 
                foreach (var item in viewContext.Controller.ViewData.ModelState)
                {
                    if (!viewContext.ViewData.ModelState.Keys.Contains(item.Key))
                    {
                        viewContext.ViewData.ModelState.Add(item);
                    }
                }

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ctx, viewResult.View);

                var viewMarkup = sw.GetStringBuilder().ToString();
                return viewMarkup;
            }
        }
    }
}