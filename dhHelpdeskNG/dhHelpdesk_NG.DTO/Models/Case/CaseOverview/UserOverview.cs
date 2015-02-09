namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    public sealed class UserOverview
    {
        public UserOverview(
                string user, 
                string notifier, 
                string email, 
                string phone, 
                string cellPhone, 
                string customer, 
                string region, 
                string department, 
                string unit, 
                string place, 
                string ordererCode)
        {
            this.OrdererCode = ordererCode;
            this.Place = place;
            this.Unit = unit;
            this.Department = department;
            this.Region = region;
            this.Customer = customer;
            this.CellPhone = cellPhone;
            this.Phone = phone;
            this.Email = email;
            this.Notifier = notifier;
            this.User = user;
        }

        public string User { get; private set; }

        public string Notifier { get; private set; }

        public string Email { get; private set; }

        public string Phone { get; private set; }

        public string CellPhone { get; private set; }

        public string Customer { get; private set; }

        public string Region { get; private set; }

        public string Department { get; private set; }

        public string Unit { get; private set; }

        public string Place { get; private set; }

        public string OrdererCode { get; private set; }
    }
}