namespace DH.Helpdesk.Services.Response.Account
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class AccountFieldsSettingsForEditResponse
    {
        public AccountFieldsSettingsForEditResponse(
            AccountFieldsSettingsForEdit accountFieldsSettingsForEdit,
            List<ItemOverview> accountTypes)
        {
            this.AccountFieldsSettingsForEdit = accountFieldsSettingsForEdit;
            this.AccountTypes = accountTypes;
        }

        [NotNull]
        public AccountFieldsSettingsForEdit AccountFieldsSettingsForEdit { get; set; }

        [NotNull]
        public List<ItemOverview> AccountTypes { get; set; }
    }
}
