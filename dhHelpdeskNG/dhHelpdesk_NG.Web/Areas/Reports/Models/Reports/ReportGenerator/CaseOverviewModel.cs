namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class CaseOverviewModel
    {
        public CaseOverviewModel(
            int id, 
            List<NewGridRowCellValueModel> fieldValues)
        {
            this.FieldValues = fieldValues;
            this.Id = id;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public List<NewGridRowCellValueModel> FieldValues { get; private set; }
    }
}