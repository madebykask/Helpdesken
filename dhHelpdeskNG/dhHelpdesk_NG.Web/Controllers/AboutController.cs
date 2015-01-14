namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using System.Diagnostics;
    using System;
    using System.IO;
    using System.Reflection;

    public class AboutController : BaseController
    {
        public AboutController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        [HttpGet]
        public ViewResult Index()        
        {            
            Assembly assembly = Assembly.LoadFrom(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "bin\\DH.Helpdesk.Web.dll"));
            Version ver = assembly.GetName().Version;
            ViewBag.VersionNum = ver.Major.ToString()+ '.' + ver.Minor + '.' + ver.Build.ToString();            
            return this.View(); 
        }
    }
}
