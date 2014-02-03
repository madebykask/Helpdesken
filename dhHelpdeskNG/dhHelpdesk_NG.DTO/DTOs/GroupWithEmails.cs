namespace dhHelpdesk_NG.DTO.DTOs
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class GroupWithEmails
    {
        public GroupWithEmails(int id, string name, List<string> emails)
        {
            this.Id = id;
            this.Name = name;
            this.Emails = emails;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        [NotNull]
        public List<string> Emails { get; private set; }
    }
}
