namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts.MapperData
{
    using DH.Helpdesk.Common.Collections;

    public class AccountSettingsMapperDataOverview : INamedObject
    {
        public string FieldName { get; set; }

        public int ShowInList { get; set; }

        public string Caption { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }
    }
}