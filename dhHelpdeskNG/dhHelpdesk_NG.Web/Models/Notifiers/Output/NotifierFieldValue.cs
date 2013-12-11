namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System;

    using dhHelpdesk_NG.Common.Tools;

    public sealed class NotifierFieldValueModel
    {
        public NotifierFieldValueModel(string fieldName, string value)
        {
            ArgumentsValidator.NotNullAndEmpty(fieldName, "fieldName");

            this.FieldName = fieldName;
            this.Value = value;
        }

        public string FieldName { get; private set; }

        public string Value { get; private set; }
    }
}