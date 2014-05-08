namespace DH.Helpdesk.Services.Requests.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class NewChangeRequest
    {
        public NewChangeRequest(
            NewChange change,
            List<Contact> contacts,
            List<int> affectedProcessIds,
            List<int> affectedDepartmentIds,
            List<NewFile> newFiles,
            List<ManualLog> newLogs,
            OperationContext context)
        {
            this.Change = change;
            this.Contacts = contacts;
            this.AffectedProcessIds = affectedProcessIds;
            this.AffectedDepartmentIds = affectedDepartmentIds;
            this.NewFiles = newFiles;
            this.NewLogs = newLogs;
            this.Context = context;
        }

        [NotNull]
        public NewChange Change { get; private set; }

        [NotNull]
        public List<Contact> Contacts { get; private set; }

        [NotNull]
        public List<int> AffectedProcessIds { get; private set; }

        [NotNull]
        public List<int> AffectedDepartmentIds { get; private set; }

        [NotNull]
        public List<NewFile> NewFiles { get; private set; }

        [NotNull]
        public List<ManualLog> NewLogs { get; private set; }

        [NotNull]
        public OperationContext Context { get; private set; }
    }
}