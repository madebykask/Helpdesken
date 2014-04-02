namespace DH.Helpdesk.Services.Infrastructure.BusinessModelRestorers
{
    public interface IRestorer<TBusinessModel, TProcessingSettings>
    {
        void Restore(TBusinessModel updatedModel, TBusinessModel existingModel, TProcessingSettings settings);
    }
}