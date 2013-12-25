namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.Tools;

    public sealed class NotifiersGridModel
    {
        public NotifiersGridModel(int notifiersFound, List<NotifierFieldModel> fields, List<NotifierDetailedOverviewModel> notifiers)
        {
            ArgumentsValidator.NotNull(fields, "fields");
            ArgumentsValidator.NotNull(notifiers, "notifiers");

            this.NotifiersFound = notifiersFound;
            this.Fields = fields;
            this.Notifiers = notifiers;
        }

        public List<NotifierDetailedOverviewModel> Notifiers { get; private set; }

        public List<NotifierFieldModel> Fields { get; private set; }

        public int NotifiersFound { get; private set; }
    }
}