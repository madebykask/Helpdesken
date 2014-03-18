namespace DH.Helpdesk.BusinessData.Models
{
    using System;

    [Flags]
    public enum ModelStates
    {
        Created = 0,

        ForEdit = 1,

        Updated = 2
    }
}