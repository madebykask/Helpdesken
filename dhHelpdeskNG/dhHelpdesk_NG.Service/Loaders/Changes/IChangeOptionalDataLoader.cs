namespace dhHelpdesk_NG.Service.Loaders.Changes
{
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;

    public interface IChangeOptionalDataLoader
    {
        ChangeOptionalData Load(int customerId, int changeId, ChangeEditSettings editSettings);
    }
}
