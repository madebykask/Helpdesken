namespace DH.Helpdesk.BusinessData.Models.Inventory.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerModule : BusinessModel
    {
        public ComputerModule(ModelStates modelState, string name)
        {
            this.State = modelState;
            this.Name = name;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        [AllowRead(ModelStates.Created)]
        public DateTime CreatedDate { get; set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDate { get; set; }

        public static ComputerModule CreateNew(string name, DateTime createdDate)
        {
            return new ComputerModule(ModelStates.Created, name) { CreatedDate = createdDate };
        }

        public static ComputerModule CreateUpdated(int id, string name, DateTime changedDate)
        {
            return new ComputerModule(ModelStates.Created, name) { ChangedDate = changedDate };
        }
    }
}