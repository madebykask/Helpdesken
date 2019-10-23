namespace DH.Helpdesk.BusinessData.Models.Inventory.Output
{
    public class ComputerUserOverview
    {
        public ComputerUserOverview(
            int id, 
            string userId,
            string firstName,
            string surName,
            string email,
            string phone,
            string mobilePhone,
            string region, 
            string department, 
            string unit)
        {
            this.Unit = unit;
            this.Id = id;
            this.UserId = userId;
            this.FirstName = firstName;
            this.SurName = surName;
            this.Email = email;
            this.Phone = phone;
            this.MobilePhone = mobilePhone;
            this.Region = region;
            this.Department = department;
        }

        public int Id { get; private set; }

        public string UserId { get; private set; }

        public string FirstName { get; private set; }
       
        public string SurName { get; private set; }

        public string Email { get; private set; }

        public string Phone { get; private set; }

        public string MobilePhone { get; private set; }

        public string Region { get; private set; }
        public string Department { get; private set; }

        public string Unit { get; private set; }
    }
}