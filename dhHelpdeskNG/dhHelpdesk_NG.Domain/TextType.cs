namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

	public enum TextTypeType {  Helpdesk = 0, Mobile = 500, LineManager = 300, MasterData = 1 };

    public class TextType : Entity
    {
        public int IsActive { get; set; }
        public string Name { get; set; }
        
        //public virtual ICollection<Text> Texts { get; set; }
    }
}
