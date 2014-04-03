namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelRestorers
{
    public interface IRestorer<TBusinessModel, TProcessingSettings>
    {
        void Restore(TBusinessModel updatedModel, TBusinessModel existingModel, TProcessingSettings settings);
    }
}