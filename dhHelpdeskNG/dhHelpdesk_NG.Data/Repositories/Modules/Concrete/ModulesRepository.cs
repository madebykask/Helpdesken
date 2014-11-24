namespace DH.Helpdesk.Dal.Repositories.Modules.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Modules.Output;
        
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Dal;    


    public sealed class ModulesRepository : Repository, IModulesRepository
    {
        public ModulesRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }


        public List<ModulesOverview> GetAllModules()
        {            
            return DbContext.Modules                
                .Select(u => new ModulesOverview()
                {
                    Id = u.Id,
                    Name = u.Name,                    
                    Description = u.Description
                })
                .ToList();            
        }

        
    }
}