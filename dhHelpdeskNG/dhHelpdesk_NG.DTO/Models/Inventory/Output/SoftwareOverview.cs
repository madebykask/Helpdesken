namespace DH.Helpdesk.BusinessData.Models.Inventory.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class SoftwareOverview
    {
        public SoftwareOverview(
            int id,
            int? ownerId,
            string manufacturer,
            string name,
            string productKey,
            string registrationCode,
            string version)
        {
            this.Id = id;
            this.OwnerId = ownerId;
            this.Manufacturer = manufacturer;
            this.Name = name;
            this.ProductKey = productKey;
            this.RegistrationCode = registrationCode;
            this.Version = version;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? OwnerId { get; set; }

        [NotNull]
        public string Manufacturer { get; set; }

        
        public string Name { get; set; }

        public string ProductKey { get; set; }

        [NotNull]
        public string RegistrationCode { get; set; }

        [NotNull]
        public string Version { get; set; }
    }
}
