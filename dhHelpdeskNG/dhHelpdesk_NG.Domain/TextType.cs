namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class TextType : Entity
    {
        public int IsActive { get; set; }
        public string Name { get; set; }
        
        //public virtual ICollection<Text> Texts { get; set; }
    }
}
