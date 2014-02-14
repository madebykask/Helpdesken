namespace DH.Helpdesk.BusinessData.Models.Inventory.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class UpdatedItem : INewBusinessModel
    {
        public UpdatedItem(int id, string name, DateTime changedDate)
        {
            this.Id = id;
            this.Name = name;
            this.ChangedDate = changedDate;
        }

        [IsId]
        public int Id { get; set; }

        [NotNullAndEmpty]
        public string Name { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}