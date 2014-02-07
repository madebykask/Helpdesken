namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Form : Entity
    {
        public int Customer_Id { get; set; }
        public string FormHeader { get; set; }
        public string FormLogo { get; set; }
        public string FormName { get; set; }
        public string FormPath { get; set; }
        public Guid FormGUID { get; set; }
    }
}
