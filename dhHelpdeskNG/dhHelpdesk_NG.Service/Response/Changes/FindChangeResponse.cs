namespace DH.Helpdesk.Services.Response.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FindChangeResponse
    {
        public FindChangeResponse(
            Change change,
            List<Contact> contacts,
            List<int> affectedProcessIds,
            List<int> affectedDepartmentIds,
            List<int> relatedChangeIds,
            List<File> files,
            List<Log> logs,
            List<HistoriesDifference> histories)
        {
            this.Change = change;
            this.Contacts = contacts;
            this.AffectedProcessIds = affectedProcessIds;
            this.AffectedDepartmentIds = affectedDepartmentIds;
            this.RelatedChangeIds = relatedChangeIds;
            this.Files = files;
            this.Logs = logs;
            this.Histories = histories;
        }

        [NotNull]
        public Change Change { get; private set; }

        [NotNull]
        public List<Contact> Contacts { get; private set; }

        [NotNull]
        public List<int> AffectedProcessIds { get; private set; }

        [NotNull]
        public List<int> AffectedDepartmentIds { get; private set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; private set; }

        [NotNull]
        public List<File> Files { get; private set; }

        [NotNull]
        public List<Log> Logs { get; private set; }

        [NotNull]
        public List<HistoriesDifference> Histories { get; private set; }
    }
}
