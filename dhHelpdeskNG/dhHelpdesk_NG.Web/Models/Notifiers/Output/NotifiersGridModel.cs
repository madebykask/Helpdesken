namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class NotifiersGridModel
    {
        public NotifiersGridModel(int notifiersFound, List<GridColumnHeaderModel> fields, List<NotifierDetailedOverviewModel> notifiers)
        {
            this.NotifiersFound = notifiersFound;
            this.Fields = fields;
            this.Notifiers = notifiers;
        }

        [NotNull]
        public List<NotifierDetailedOverviewModel> Notifiers { get; private set; }

        [NotNull]
        public List<GridColumnHeaderModel> Fields { get; private set; }

        public int NotifiersFound { get; private set; }
    }
}