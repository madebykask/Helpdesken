namespace DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers
{
    public sealed class ManufacturerOverview
    {
        public ManufacturerOverview(
                int manufacturerId, 
                string manufcturerName)
        {
            this.ManufcturerName = manufcturerName;
            this.ManufacturerId = manufacturerId;
        }

        public int ManufacturerId { get; private set; } 

        public string ManufcturerName { get; private set; }
    }
}