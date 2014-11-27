namespace DH.Helpdesk.BusinessData.Models.Licenses.Computers
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ComputerOverview
    {
        public ComputerOverview(
                int computerId,
                string computerName, 
                string user, 
                DateTime? scanDate)
        {
            this.ComputerId = computerId;
            this.ScanDate = scanDate;
            this.User = user;
            this.ComputerName = computerName;
        }

        [IsId]
        public int ComputerId { get; private set; }

        [NotNull]
        public string ComputerName { get; private set; }

        public string User { get; private set; }

        public DateTime? ScanDate { get; private set; }
    }
}