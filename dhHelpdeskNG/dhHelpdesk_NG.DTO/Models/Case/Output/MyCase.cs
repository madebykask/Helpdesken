namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    using System;

    public sealed class MyCase
    {
        public MyCase(
                int id, 
                DateTime registrationDate, 
                string subject, 
                string initiatorName)
        {
            this.InitiatorName = initiatorName;
            this.Subject = subject;
            this.RegistrationDate = registrationDate;
            this.Id = id;
        }

        public int Id { get; private set; }

        public DateTime RegistrationDate { get; private set; }

        public string Subject { get; private set; }

        public string InitiatorName { get; private set; }
    }
}