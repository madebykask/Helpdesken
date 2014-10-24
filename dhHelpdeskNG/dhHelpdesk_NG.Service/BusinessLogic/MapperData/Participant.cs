namespace DH.Helpdesk.Services.BusinessLogic.MapperData
{
    using System;

    public class Participant
    {
        public Guid Guid { get; set; }

        public Decimal CaseNumber { get; set; }

        public string CaseDescription { get; set; }

        public string Caption { get; set; }

        public string Email { get; set; }
    }
}