namespace DH.Helpdesk.BusinessData.Models.Inventory.Output
{
    using System;

    using DH.Helpdesk.Common.Types;

    public class OperationServerLogOverview
    {
        public OperationServerLogOverview(UserName admin, DateTime? createdDate, string description, string action)
        {
            this.Admin = admin;
            this.CreatedDate = createdDate;
            this.Description = description;
            this.Action = action;
        }

        public UserName Admin { get; private set; }

        public DateTime? CreatedDate { get; private set; }

        public string Description { get; private set; }

        public string Action { get; private set; }
    }
}