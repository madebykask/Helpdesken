namespace DH.Helpdesk.Dal.Mappers.Problems
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Problem.Input;
    using DH.Helpdesk.Domain.Problems;

    public class ProblemEntityFromBusinessModelChanger : IBusinessModelToEntityMapper<NewProblemDto, Problem>
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
