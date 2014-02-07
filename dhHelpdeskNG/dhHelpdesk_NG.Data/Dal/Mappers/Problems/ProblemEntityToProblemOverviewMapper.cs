namespace DH.Helpdesk.Dal.Dal.Mappers.Problems
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Problem.Output;
    using DH.Helpdesk.Domain.Problems;

    public class ProblemEntityToProblemOverviewMapper : IEntityToBusinessModelMapper<Problem, ProblemOverview>
    {
        public ProblemOverview Map(Problem entity)
        {
            return new ProblemOverview
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ProblemNumber = entity.ProblemNumber,
                ResponsibleUserId = entity.ResponsibleUser == null ? null : (int?)entity.ResponsibleUser.Id,
                ResponsibleUserName = entity.ResponsibleUser == null ? null : entity.ResponsibleUser.FirstName,
                InventoryNumber = entity.InventoryNumber,
                ShowOnStartPage = entity.ShowOnStartPage == 1,
                FinishingDate = entity.FinishingDate,
                IsExistConnectedCases = entity.Cases.Any()
            };
        }
    }
}
