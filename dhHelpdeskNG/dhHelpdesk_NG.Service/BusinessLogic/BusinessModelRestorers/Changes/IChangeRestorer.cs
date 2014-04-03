namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;

    public interface IChangeRestorer
    {
        void Restore(UpdatedChange updatedChange, Change existingChange, ChangeProcessingSettings settings);
    }
}