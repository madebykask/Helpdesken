namespace dhHelpdesk_NG.Service.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.Domain;

    public interface IChangeService
    {
        FieldSettingsDto FindSettings(int customerId, int languageId);

        IDictionary<string, string> Validate(Change changeToValidate);

        IList<Change> GetChange(int customerId);
        IList<Change> GetChanges(int customerId);
        Change GetChange(int id, int customerId);

        void DeleteChange(Change change);
        void NewChange(Change change);
        void UpdateChange(Change change);
        void Commit();
    }
}