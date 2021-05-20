namespace DH.Helpdesk.BusinessData.Models.Inventory.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerModule : BusinessModel
    {
        private ComputerModule(ModelStates modelState, string name, string description = null)
        {
            this.State = modelState;
            this.Name = name;
            this.Description = description;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public string Description { get; set; }

        [AllowRead(ModelStates.Created)]
        public DateTime CreatedDate { get; set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDate { get; set; }
        public int? Price { get; internal set; }

        public static ComputerModule CreateNew(string name, DateTime createdDate)
        {
            return new ComputerModule(ModelStates.Created, name) { CreatedDate = createdDate };
        }

        public static ComputerModule CreateUpdated(int id, string name, DateTime changedDate, int? price)
        {
            return new ComputerModule(ModelStates.Updated, name) { Id = id, ChangedDate = changedDate, Price = price };
        }
    }
}