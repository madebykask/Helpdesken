namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Case;
    

    public interface ICaseSolutionConditionRepository : IRepository<CaseSolutionConditionEntity>
    {
        IEnumerable<CaseSolutionConditionModel> GetCaseSolutionConditions(int caseSolution_Id);
    }
}