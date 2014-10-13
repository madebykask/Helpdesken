namespace DH.Helpdesk.Web.Areas.Licenses.Models.Manufacturers
{
    using DH.Helpdesk.BusinessData.Models.Licenses;
    using DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers;

    public sealed class ManufacturersContentModel
    {
        public ManufacturersContentModel(ManufacturerOverview[] manufacturers)
        {
            this.Manufacturers = manufacturers;
        }

        public ManufacturerOverview[] Manufacturers { get; private set; }
    }
}