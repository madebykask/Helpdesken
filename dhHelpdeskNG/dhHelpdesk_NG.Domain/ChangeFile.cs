using System;

namespace dhHelpdesk_NG.Domain
{
    public class ChangeFile : Entity
    {
        public byte[] File { get; set; }
        public int ChangeArea { get; set; }
        public int Change_Id { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ChangeFileGUID { get; set; }

        public virtual Change Change { get; set; }
    }
}
