using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ChangeEditData
    {
        [NotNull]
        public Change Change { get; set; }

        [NotNull]
        public List<Contact> Contacts { get; set; }

        [NotNull]
        public List<int> AffectedProcessIds { get; set; }

        [NotNull]
        public List<int> AffectedDepartmentIds { get; set; }

        [NotNull]
        public List<int> RelatedChangeIds { get; set; }

        [NotNull]
        public List<File> Files { get; set; }

        [NotNull]
        public List<Log> Logs { get; set; }

        [NotNull]
        public List<HistoriesDifference> Histories { get; set; }
    }
}
