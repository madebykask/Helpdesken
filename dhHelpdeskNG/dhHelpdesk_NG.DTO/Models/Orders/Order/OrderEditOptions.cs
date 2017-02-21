namespace DH.Helpdesk.BusinessData.Models.Orders.Order
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderEditOptions
    {
        public OrderEditOptions(
                string orderTypeName,
                OrderStatusItem[] statuses, 
                ItemOverview[] administrators, 
                ItemOverview[] domains, 
                ItemOverview[] departments, 
                ItemDependentOverview[] units, 
                ItemOverview[] properties, 
                ItemOverview[] deliveryDepartment, 
                ItemOverview[] deliveryOuId, 
                List<GroupWithEmails> emailGroups, 
                List<GroupWithEmails> workingGroupsWithEmails, 
                List<ItemOverview> administratorsWithEmails,
                ItemOverview[] employmentTypes,
                ItemOverview[] regions,
                ItemOverview[] accountTypes,
                ItemOverview[] accountTypes2,
                ItemOverview[] accountTypes3,
                ItemOverview[] accountTypes4,
                ItemOverview[] accountTypes5,
                ItemOverview[] programs,
                string orderTypeDecription)
        {
            OrderTypeName = orderTypeName;
            AdministratorsWithEmails = administratorsWithEmails;
            WorkingGroupsWithEmails = workingGroupsWithEmails;
            EmailGroups = emailGroups;
            DeliveryOuId = deliveryOuId;
            DeliveryDepartment = deliveryDepartment;
            Properties = properties;
            Units = units;
            Departments = departments;
            Domains = domains;
            Administrators = administrators;
            Statuses = statuses;
            EmploymentTypes = employmentTypes;
            Regions = regions;
            AccountTypes = accountTypes;
            AccountTypes2 = accountTypes2;
            AccountTypes3 = accountTypes3;
            AccountTypes4 = accountTypes4;
            AccountTypes5 = accountTypes5;
            Programs = programs;
            OrderTypeDescription = orderTypeDecription;
        }

        public string OrderTypeName { get; private set; }

        public string OrderTypeDescription { get; private set; }

        [NotNull]
        public OrderStatusItem[] Statuses { get; private set; }

        [NotNull]
        public ItemOverview[] Administrators { get; private set; } 
        
        [NotNull]
        public ItemOverview[] Domains { get; private set; } 
        
        [NotNull]
        public ItemOverview[] Departments { get; private set; } 
        
        [NotNull]
        public ItemDependentOverview[] Units { get; private set; } 
        
        [NotNull]
        public ItemOverview[] Properties { get; private set; } 
        
        [NotNull]
        public ItemOverview[] DeliveryDepartment { get; private set; } 
        
        [NotNull]
        public ItemOverview[] DeliveryOuId { get; private set; }

        [NotNull]
        public List<GroupWithEmails> EmailGroups { get; private set; }

        [NotNull]
        public List<GroupWithEmails> WorkingGroupsWithEmails { get; private set; }

        [NotNull]
        public List<ItemOverview> AdministratorsWithEmails { get; private set; }

        [NotNull]
        public ItemOverview[] EmploymentTypes { get; private set; }
        
        [NotNull]
        public ItemOverview[] Regions { get; private set; }

        [NotNull]
        public ItemOverview[] AccountTypes { get; private set; }

        [NotNull]
        public ItemOverview[] AccountTypes2 { get; private set; }

        [NotNull]
        public ItemOverview[] AccountTypes3 { get; private set; }

        [NotNull]
        public ItemOverview[] AccountTypes4 { get; private set; }

        [NotNull]
        public ItemOverview[] AccountTypes5 { get; private set; }

        [NotNull]
        public ItemOverview[] Programs { get; private set; }

    }
}