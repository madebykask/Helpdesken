namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderEditOptions
    {
        public OrderEditOptions(
                ItemOverview[] statuses, 
                ItemOverview[] administrators, 
                ItemOverview[] domains, 
                ItemOverview[] departments, 
                ItemOverview[] units, 
                ItemOverview[] properties, 
                ItemOverview[] deliveryDepartment, 
                ItemOverview[] deliveryOuId)
        {
            this.DeliveryOuId = deliveryOuId;
            this.DeliveryDepartment = deliveryDepartment;
            this.Properties = properties;
            this.Units = units;
            this.Departments = departments;
            this.Domains = domains;
            this.Administrators = administrators;
            this.Statuses = statuses;
        }

        [NotNull]
        public ItemOverview[] Statuses { get; private set; }

        [NotNull]
        public ItemOverview[] Administrators { get; private set; } 
        
        [NotNull]
        public ItemOverview[] Domains { get; private set; } 
        
        [NotNull]
        public ItemOverview[] Departments { get; private set; } 
        
        [NotNull]
        public ItemOverview[] Units { get; private set; } 
        
        [NotNull]
        public ItemOverview[] Properties { get; private set; } 
        
        [NotNull]
        public ItemOverview[] DeliveryDepartment { get; private set; } 
        
        [NotNull]
        public ItemOverview[] DeliveryOuId { get; private set; } 
    }
}