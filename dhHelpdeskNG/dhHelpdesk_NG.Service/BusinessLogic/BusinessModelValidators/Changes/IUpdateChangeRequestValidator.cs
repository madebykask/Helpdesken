namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;
    using DH.Helpdesk.Services.Requests.Changes;

    public interface IUpdateChangeRequestValidator
    {
        void Validate(UpdateChangeRequest request, Change existingChange, ChangeProcessingSettings settings);
    }
}