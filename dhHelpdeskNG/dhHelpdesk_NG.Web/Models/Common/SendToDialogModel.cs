namespace DH.Helpdesk.Web.Models.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SendToDialogModel
    {
        public SendToDialogModel(
            MultiSelectList emailGroups,
            List<GroupEmailsModel> emailGroupEmails,
            MultiSelectList workingGroups,
            List<GroupEmailsModel> workingGroupEmails,
            MultiSelectList administrators)
        {
            if (emailGroupEmails.Any(e => emailGroups.All(g => int.Parse(g.Value) != e.GroupId)))
            {
                throw new ArgumentOutOfRangeException(
                    "emailGroupEmails",
                    "emailGroups doesn't contain specified group.");
            }

            if (workingGroupEmails.Any(e => workingGroups.All(g => int.Parse(g.Value) != e.GroupId)))
            {
                throw new ArgumentOutOfRangeException(
                    "workingGroupEmails",
                    "workingGroups doesn't contain specified group.");
            }

            this.EmailGroups = emailGroups;
            this.EmailGroupEmails = emailGroupEmails;
            this.WorkingGroups = workingGroups;
            this.WorkingGroupEmails = workingGroupEmails;
            this.Administrators = administrators;
        }

        [NotNull]
        public MultiSelectList EmailGroups { get; private set; }

        [NotNull]
        public List<GroupEmailsModel> EmailGroupEmails { get; private set; }

        [NotNull]
        public MultiSelectList WorkingGroups { get; private set; }

        [NotNull]
        public List<GroupEmailsModel> WorkingGroupEmails { get; private set; }

        [NotNull]
        public MultiSelectList Administrators { get; private set; }
    }
}