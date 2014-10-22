namespace DH.Helpdesk.Domain.Interfaces
{
    using global::System;

    public interface IDatedEntity
    {
        DateTime ChangedDate { get; }

        DateTime CreatedDate { get; } 
    }
}