namespace DH.Helpdesk.BusinessData.Models.Inventory.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewItem : INewBusinessModel
    {
        public NewItem(string name, DateTime createdDate)
        {
            this.Name = name;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}