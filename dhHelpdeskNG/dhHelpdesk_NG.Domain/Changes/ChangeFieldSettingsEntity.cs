namespace DH.Helpdesk.Domain.Changes
{
    using DH.Helpdesk.Common.Collections;

    using global::System;

    public class ChangeFieldSettingsEntity : Entity, INamedObject
    {
        public int Customer_Id { get; set; }
        
        public int Required { get; set; }
        
        public int Show { get; set; }
        
        public int ShowInList { get; set; }
        
        public int ShowExternal { get; set; }
        
        public string Bookmark { get; set; }
        
        public string ChangeField { get; set; }
        
        public string FieldHelp { get; set; }
        
        public string InitialValue { get; set; }
        
        public string Label { get; set; }
        
        public string Label_ENG { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public string GetName()
        {
            return this.ChangeField;
        }
    }
}
