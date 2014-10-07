namespace DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ManufacturerData
    {
        public ManufacturerData(ManufacturerModel manufacturer)
        {
            this.Manufacturer = manufacturer;
        }

        [NotNull]
        public ManufacturerModel Manufacturer { get; private set; }
    }
}