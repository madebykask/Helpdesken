namespace DH.Helpdesk.Web.Models.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GroupEmailsModel
    {
        public GroupEmailsModel(int groupId, List<string> emails)
        {
            this.GroupId = groupId;
            this.Emails = emails;
        }

        [IsId]
        public int GroupId { get; private set; }

        [NotNull]
        public List<string> Emails { get; private set; }
    }
}