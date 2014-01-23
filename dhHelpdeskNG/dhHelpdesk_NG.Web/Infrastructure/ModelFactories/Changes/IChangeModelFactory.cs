namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.ChangeAggregate;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface IChangeModelFactory
    {
        ChangeModel Create(ChangeAggregate change, ChangeOptionalData optionalData);
    }
}