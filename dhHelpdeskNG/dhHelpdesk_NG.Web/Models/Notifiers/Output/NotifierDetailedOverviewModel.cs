namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

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