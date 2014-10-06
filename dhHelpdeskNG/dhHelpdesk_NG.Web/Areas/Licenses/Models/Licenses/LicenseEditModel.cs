namespace DH.Helpdesk.Web.Areas.Licenses.Models.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Licenses;

    public sealed class LicenseEditModel
    {
        public LicenseEditModel(LicenseData data)
        {
            this.Data = data;
        }

        public LicenseEditModel()
        {            
        }

        public LicenseData Data { get; set; }
    }
}