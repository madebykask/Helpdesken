namespace dhHelpdesk_NG.Data.Dal.Mappers.Problems
{
    using System;

    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;

    public class ProblemEntityFromBusinessModelChanger : IEntityChangerFromBusinessModel<NewProblemDto, Problem>
    {
        public void Map(NewProblemDto businessModel, Problem entity)
        {
            if (string.IsNullOrWhiteSpace(businessModel.InventoryNumber))
            {
                businessModel.InventoryNumber = string.Empty;
            }

            var problem = entity;
            problem.Name = businessModel.Name;
            problem.Description = businessModel.Description;
            problem.ResponsibleUser_Id = businessModel.ResponsibleUserId;
            problem.InventoryNumber = string.IsNullOrWhiteSpace(businessModel.InventoryNumber) ? string.Empty : businessModel.InventoryNumber;
            problem.ShowOnStartPage = businessModel.ShowOnStartPage ? 1 : 0;
            problem.ChangedDate = DateTime.Now;
        }
    }
}
