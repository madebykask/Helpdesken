namespace DH.Helpdesk.BusinessData.Models
{
    using System;

    [Flags]
    public enum ModelStates
    {
        Created = 1,

        ForEdit = 2,

        Updated = 4
    }
}