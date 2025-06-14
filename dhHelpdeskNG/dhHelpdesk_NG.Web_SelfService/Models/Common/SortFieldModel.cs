﻿namespace DH.Helpdesk.SelfService.Models.Common
{
    using DH.Helpdesk.Common.Enums;

    public sealed class SortFieldModel
    {
        public string Name { get; set; }

        public SortBy? SortBy { get; set; }
    }
}