using System;

namespace dhHelpdesk_NG.Domain
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeFile : Entity
    {
        public byte[] File { get; set; }
        public int ChangeArea { get; set; }
        public int Change_Id { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ChangeFileGUID { get; set; }

        public virtual ChangeEntity Change { get; set; }
    }
}
