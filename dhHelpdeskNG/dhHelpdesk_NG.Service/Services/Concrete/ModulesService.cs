namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Modules.Output;       
    using DH.Helpdesk.Dal.Repositories.Modules;
    
    public class ModulesService : IModulesService
    {      
        private readonly IModulesRepository _ModuleRepository;

        public ModulesService(IModulesRepository moduleRepository)
        {
            this._ModuleRepository = moduleRepository;            
        }        
        
        public List<ModulesOverview> GetAllModules()
        {
            return this._ModuleRepository.GetAllModules();            
        }
                
    }
}