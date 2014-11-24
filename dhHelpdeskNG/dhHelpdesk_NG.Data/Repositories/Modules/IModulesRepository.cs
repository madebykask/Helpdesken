namespace DH.Helpdesk.Dal.Repositories.Modules
{
    using System.Collections.Generic;
    
    using DH.Helpdesk.Dal.Infrastructure;    
    using DH.Helpdesk.BusinessData.Models.Modules.Output;

    public interface IModulesRepository
    {        
        List<ModulesOverview> GetAllModules();
    }
}
