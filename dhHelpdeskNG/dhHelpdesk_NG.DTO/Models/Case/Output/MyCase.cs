namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    using System;

    public sealed class MyCase
    {
        public MyCase(
                int id,
                decimal caseNumber,
                DateTime registrationDate,
                DateTime changedDate, 
                string subject, 
                string initiatorName,
                string description, 
                string customerName)
        {
            this.InitiatorName = initiatorName;
            this.Description = description;
            this.Subject = subject;
            this.RegistrationDate = registrationDate;
            this.ChangedDate = changedDate; 
            this.Id = id;
            this.CaseNumber = caseNumber;
            this.CustomerName = customerName; 
        }

        public int Id { get; private set; }

        public decimal CaseNumber { get; private set; }

        public DateTime RegistrationDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public string Subject { get; private set; }

        public string CustomerName { get; private set; }

        public string InitiatorName { get; private set; }

        public string Description { get; private set; }
    }
}