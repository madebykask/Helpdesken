namespace DH.Helpdesk.Services.Restorers
{
    public interface IRestorer<TBusinessModel, TProcessingSettings>
    {
        void Restore(TBusinessModel updatedModel, TBusinessModel existingModel, TProcessingSettings settings);
    }
}