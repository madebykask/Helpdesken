namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Accounts.MapperData
{
    using DH.Helpdesk.Common.Collections;

    public class AccountSettingsMapperDataForEdit : INamedObject
    {
        public string AccountName { get; set; }

        public int ShowInDetails { get; set; }

        public int ShowInList { get; set; }

        public string Caption { get; set; }

        public string Help { get; set; }

        public int Required { get; set; }

        public int MultiValue { get; set; }

        public string GetName()
        {
            return this.AccountName;
        }
    }
}
