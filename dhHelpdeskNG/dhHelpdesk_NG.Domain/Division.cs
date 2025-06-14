﻿namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Division : Entity
    {
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SyncChangedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
