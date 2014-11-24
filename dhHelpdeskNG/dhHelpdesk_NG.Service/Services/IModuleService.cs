namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Modules.Output;

    public interface IModulesService
    {
        
        List<ModulesOverview> GetAllModules ();

    }
}