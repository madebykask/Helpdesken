namespace DH.Helpdesk.Services.Infrastructure.BusinessModelValidators.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;

    using UpdateChangeRequest = DH.Helpdesk.Services.Requests.Changes.UpdateChangeRequest;

    public interface IUpdateChangeRequestValidator
    {
        void Validate(UpdateChangeRequest request, Change existingChange, ChangeProcessingSettings settings);
    }
}