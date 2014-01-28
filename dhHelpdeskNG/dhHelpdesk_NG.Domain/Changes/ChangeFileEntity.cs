namespace dhHelpdesk_NG.Domain.Changes
{
    using global::System;

    public class ChangeFileEntity : Entity
    {
        public Guid ChangeFileGUID { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] ChangeFile { get; set; }

        public int Change_Id { get; set; }

        public virtual ChangeEntity Change { get; set; }

        public int ChangeArea { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}