using System;

namespace DH.Helpdesk.Domain
{
    public class ApplicationTypeEntity : Entity
    {
        public string Type { get; set; }

        public Guid UniqueId { get; set; }
    }
}