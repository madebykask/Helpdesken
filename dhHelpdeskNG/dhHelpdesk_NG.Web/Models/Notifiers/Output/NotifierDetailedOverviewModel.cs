namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System;
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.Tools;

    public sealed class NotifierDetailedOverviewModel
    {
        public NotifierDetailedOverviewModel(int id, List<NotifierFieldValueModel> fieldValues)
        {
            ArgumentsValidator.IsId(id, "id");
            ArgumentsValidator.NotNull(fieldValues, "values");

            this.Id = id;
            this.FieldValues = fieldValues;
        }

        public int Id { get; private set; }

        public List<NotifierFieldValueModel> FieldValues { get; private set; }
    }
}