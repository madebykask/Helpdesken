﻿namespace DH.Helpdesk.Web.Models.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;

    public sealed class CustomerChangesModel
    {
        public CustomerChangesModel(CustomerChanges[] customerChanges, bool showIcon)
        {
            this.ShowIcon = showIcon;
            this.CustomerChanges = customerChanges;
        }

        public CustomerChangesModel()
        {
        }

        public CustomerChanges[] CustomerChanges { get; private set; }

        public bool ShowIcon { get; private set; }
    }
}