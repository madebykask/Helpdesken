﻿namespace DH.Helpdesk.Domain
{
    public class UrgencyLanguage : Entity
    {
        public int Language_Id { get; set; }
        public int Urgency_Id { get; set; }
        public string Urgency { get; set; }
    }
}
