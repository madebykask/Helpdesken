namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;    
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Case;
    

    public interface ICaseSolutionConditionRepository : IRepository<CaseSolutionConditionEntity>
    {
        /// <summary>
        /// Returns all active conditions for a CaseSolution and cache them on server for 60 minutes
        /// </summary>
        /// <param name="caseSolution_Id">
        /// The id of CaseSolution.
        /// </param>
        IEnumerable<CaseSolutionConditionModel> GetCaseSolutionConditions(int caseSolution_Id);

        IList<CaseSolutionConditionEntity> GetCaseSolutionCondition(int casesolutionid);

        IList<StateSecondary> GetStateSecondaries(int casesolutionid);

    }
}