﻿namespace DH.Helpdesk.SelfService.Models.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.SelfService.Models.Common;

    public sealed class NotifierDetailedOverviewModel
    {
        public NotifierDetailedOverviewModel(int id, List<GridRowCellValueModel> fieldValues)
        {
            this.Id = id;
            this.FieldValues = fieldValues;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public List<GridRowCellValueModel> FieldValues { get; private set; }
    }
}