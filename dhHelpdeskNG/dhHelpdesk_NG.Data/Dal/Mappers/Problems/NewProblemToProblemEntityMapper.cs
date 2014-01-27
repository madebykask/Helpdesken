namespace dhHelpdesk_NG.Data.Dal.Mappers.Problems
{
    using dhHelpdesk_NG.Domain.Problems;
    using dhHelpdesk_NG.DTO.DTOs.Problem.Input;

    public class NewProblemToProblemEntityMapper : INewBusinessModelToEntityMapper<NewProblemDto, Problem>
    {
        public Problem Map(NewProblemDto businessModel)
        {
            // ToDo Artem: try to never change source object. Better to move this condition to entity assign process
            if (string.IsNullOrWhiteSpace(businessModel.InventoryNumber))
            {
                businessModel.InventoryNumber = string.Empty;
            }

            return new Problem
            {
                Id = businessModel.Id,
                Name = businessModel.Name,
                Description = businessModel.Description,
                ResponsibleUser_Id = businessModel.ResponsibleUserId,
                InventoryNumber = businessModel.InventoryNumber,
                ShowOnStartPage = businessModel.ShowOnStartPage ? 1 : 0,
                Customer_Id = businessModel.CustomerId,
                FinishingDate = businessModel.FinishingDate
            };
        }
    }
}
