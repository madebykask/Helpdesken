﻿namespace DH.Helpdesk.BusinessData.Models.Shared.Input
{
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SortField
    {
        public SortField()
        {

        }

        public SortField(string name, SortBy sortBy)
        {
            this.Name = name;
            this.SortBy = sortBy;
        }

        [NotNullAndEmpty]
        public string Name { get; set; }

        public SortBy SortBy { get; set; }
    }
}
