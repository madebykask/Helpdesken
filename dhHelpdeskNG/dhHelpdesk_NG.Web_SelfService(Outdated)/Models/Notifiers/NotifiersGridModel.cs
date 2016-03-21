namespace DH.Helpdesk.SelfService.Models.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.SelfService.Models.Common;

    public sealed class NotifiersGridModel
    {
        public NotifiersGridModel(
            int notifiersFound,
            List<GridColumnHeaderModel> fields, 
            List<NotifierDetailedOverviewModel> notifiers,
            SortFieldModel sortField)
        {
            this.NotifiersFound = notifiersFound;
            this.Headers = fields;
            this.Notifiers = notifiers;
            this.SortField = sortField;
        }

        [NotNull]
        public List<NotifierDetailedOverviewModel> Notifiers { get; private set; }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; private set; }

        public int NotifiersFound { get; private set; }

        public SortFieldModel SortField { get; private set; }
    }
}