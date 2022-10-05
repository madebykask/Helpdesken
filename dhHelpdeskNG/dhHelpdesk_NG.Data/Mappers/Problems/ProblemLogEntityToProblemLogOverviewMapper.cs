// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProblemLogEntityToProblemLogOverviewMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProblemLogEntityToProblemLogOverviewMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.Problems
{
    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Domain.Problems;

    /// <summary>
    /// The problem log entity to problem log overview mapper.
    /// </summary>
    public class ProblemLogEntityToProblemLogOverviewMapper : IEntityToBusinessModelMapper<ProblemLog, ProblemLogOverview>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="ProblemLogOverview"/>.
        /// </returns>
        public ProblemLogOverview Map(ProblemLog entity)
        {
            return new ProblemLogOverview
                       {
                           Id = entity.Id,
                           ProblemId = entity.Problem_Id,
                           ChangedByUserName = entity.ChangedByUser.FirstName, 
                           ChangedByUserSurName = entity.ChangedByUser.SurName,
                           ChangedDate = entity.ChangedDate,
                           LogText = entity.LogText,
                           CreatedDate = entity.CreatedDate,
                           ShowOnCase = entity.ShowOnCase,
                           FinnishConnectedCases = entity.FinishConnectedCases
                       };
        }
    }
}