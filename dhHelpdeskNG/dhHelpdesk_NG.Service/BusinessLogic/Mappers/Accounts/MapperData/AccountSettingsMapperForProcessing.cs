namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts.MapperData
{
    using DH.Helpdesk.Common.Collections;

    public class AccountSettingsMapperForProcessing : INamedObject
    {
        public string FieldName { get; set; }

        public int ShowInDetails { get; set; }

        public int Required { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }
    }
}