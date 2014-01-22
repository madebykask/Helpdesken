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

            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description;
            entity.ResponsibleUser_Id = businessModel.ResponsibleUserId;
            entity.InventoryNumber = string.IsNullOrWhiteSpace(businessModel.InventoryNumber) ? string.Empty : businessModel.InventoryNumber;
            entity.ShowOnStartPage = businessModel.ShowOnStartPage ? 1 : 0;
            entity.ChangedDate = DateTime.Now;
        }
    }
}
