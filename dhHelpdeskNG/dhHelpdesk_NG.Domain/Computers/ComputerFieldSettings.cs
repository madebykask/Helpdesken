namespace DH.Helpdesk.Domain.Computers
{
    using DH.Helpdesk.Common.Collections;

    using global::System;

    public class ComputerFieldSettings : Entity, INamedObject
    {
        public int Customer_Id { get; set; }
        public int Show { get; set; }
        public int ShowInList { get; set; }
        public int ReadOnly { get; set; }
        public int Copy { get; set; }
        public int Required { get; set; }
        public string ComputerField { get; set; }
        public string FieldHelp { get; set; }
        public string Label { get; set; }
        public string Label_ENG { get; set; }
        public string XMLElement { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public string GetName()
        {
            return this.ComputerField;
        }
    }
}
