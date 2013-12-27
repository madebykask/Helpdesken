namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NotifierDetailedOverviewModel
    {
        public NotifierDetailedOverviewModel(int id, List<NotifierFieldValueModel> fieldValues)
        {
            this.Id = id;
            this.FieldValues = fieldValues;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public List<NotifierFieldValueModel> FieldValues { get; private set; }
    }
}