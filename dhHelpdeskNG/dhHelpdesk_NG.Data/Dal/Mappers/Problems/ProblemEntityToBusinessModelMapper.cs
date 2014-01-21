namespace dhHelpdesk_NG.Data.Dal.Mappers.Problems
{
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;

    public class ProblemEntityToBusinessModelMapper : IEntityToBusinessModelMapper<Problem, NewProblemDto>
    {
        public NewProblemDto Map(Problem entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
