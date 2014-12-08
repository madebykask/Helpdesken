namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Edit
{
    public class ProgramFieldSettings
    {
        public ProgramFieldSettings(FieldSetting programs, FieldSetting infoProduct)
        {
            this.Programs = programs;
            this.InfoProduct = infoProduct;
        }

        public FieldSetting Programs { get; private set; }

        public FieldSetting InfoProduct { get; private set; }
    }
}