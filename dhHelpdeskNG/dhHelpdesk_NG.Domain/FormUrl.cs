namespace DH.Helpdesk.Domain
{
    using global::System;

    public class FormUrlEntity : Entity
    {
        public int Id { get; set; }
        public bool ExternalSite { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
    }
}
