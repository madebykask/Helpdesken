namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview
{
    public abstract class ProgramFieldSettings
    {
        protected ProgramFieldSettings(FieldSetting programs, FieldSetting infoProduct)
        {
            this.Programs = programs;
            this.InfoProduct = infoProduct;
        }

        public FieldSetting Programs { get; private set; }

        public FieldSetting InfoProduct { get; private set; }
    }
}