namespace DH.Helpdesk.Domain.Servers
{
    using DH.Helpdesk.Common.Collections;

    using global::System;

    public class ServerFieldSettings : Entity, INamedObject
    {
        public int Customer_Id { get; set; }
        public int Required { get; set; }
        public int Show { get; set; }
        public int ShowInList { get; set; }
        public string FieldHelp { get; set; }
        public string Label { get; set; }
        public string Label_ENG { get; set; }
        public string ServerField { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public string GetName()
        {
            return this.ServerField;
        }
    }
}
