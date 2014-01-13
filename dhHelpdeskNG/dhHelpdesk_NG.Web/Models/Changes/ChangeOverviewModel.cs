namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangeOverviewModel
    {
        public ChangeOverviewModel(int id, List<GridRowCellValueModel> fieldValues)
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