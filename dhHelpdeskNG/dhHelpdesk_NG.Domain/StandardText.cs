﻿namespace DH.Helpdesk.Domain
{
    using global::System;

    public class StandardText : Entity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public string Text { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StandardTextName { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
