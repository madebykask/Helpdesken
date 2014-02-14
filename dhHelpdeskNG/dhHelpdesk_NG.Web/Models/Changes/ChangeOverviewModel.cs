namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Common;

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