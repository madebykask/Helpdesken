namespace DH.Helpdesk.BusinessData.Models.Customer.Input
{
    public class CustomerOverview
    {
		public CustomerOverview()
		{
			Active = true;
		}
        public int Id { get; set; }
        public string Name { get; set; }

		public bool Active { get; set; }
	}
}
