using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.EForm.FormLib
{
    public class FormLibViewEngine : RazorViewEngine
    {
        public FormLibViewEngine()
        {
            var areaViewAndPartialViewLocationFormats = new List<string>();
            var partialViewLocationFormats = new List<string>();
            var masterLocationFormats = new List<string>();

            masterLocationFormats.Add("~/FormLibContent/Views/{1}/{0}.cshtml");
            masterLocationFormats.Add("~/FormLibContent/Views/Shared/{1}/{0}.cshtml");

            partialViewLocationFormats.Add("~/FormLibContent/Views/{1}/{0}.cshtml");
            partialViewLocationFormats.Add("~/FormLibContent/Views/Shared/{0}.cshtml");

            areaViewAndPartialViewLocationFormats.Add("~/FormLibContent/Areas/Views/{1}/{0}.cshtml");
            areaViewAndPartialViewLocationFormats.Add("~/FormLibContent/Areas/{2}/Views/{1}/{0}.cshtml");
            areaViewAndPartialViewLocationFormats.Add("~/FormLibContent/Areas/{2}/Views/Shared/{0}.cshtml");

            ViewLocationFormats = partialViewLocationFormats.ToArray();
            MasterLocationFormats = masterLocationFormats.ToArray();
            PartialViewLocationFormats = partialViewLocationFormats.ToArray();
            AreaPartialViewLocationFormats = areaViewAndPartialViewLocationFormats.ToArray();
            AreaViewLocationFormats = areaViewAndPartialViewLocationFormats.ToArray();
        }
    }
}