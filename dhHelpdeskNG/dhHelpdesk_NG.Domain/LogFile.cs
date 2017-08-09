namespace DH.Helpdesk.Domain
{
    using global::System;

    public class LogFile : Entity
    {
        public int Log_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool? IsCaseFile { get; set; }
        public int? ParentLog_Id { get; set; }

        public virtual Log Log { get; set; }
    }
}
