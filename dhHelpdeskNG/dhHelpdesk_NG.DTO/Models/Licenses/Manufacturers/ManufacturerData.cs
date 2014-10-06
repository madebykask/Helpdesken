namespace DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers
{
    public sealed class ManufacturerData
    {
        public ManufacturerData(ManufacturerModel manufacturer)
        {
            this.Manufacturer = manufacturer;
        }

        public ManufacturerModel Manufacturer { get; private set; }
    }
}