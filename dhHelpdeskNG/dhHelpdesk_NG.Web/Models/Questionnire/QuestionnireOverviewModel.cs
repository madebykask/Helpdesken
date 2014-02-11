namespace DH.Helpdesk.Web.Models.Questionnire
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class QuestionnireOverviewModel
    {
        public QuestionnireOverviewModel(int id, List<GridRowCellValueModel> fieldValues)
        {
            this.Id = id;
            this.FieldValues = fieldValues;
        }

        [IsId]
        public int Id { get; set; }

        [NotNull]
        public List<GridRowCellValueModel> FieldValues { get; set; }
    }
}