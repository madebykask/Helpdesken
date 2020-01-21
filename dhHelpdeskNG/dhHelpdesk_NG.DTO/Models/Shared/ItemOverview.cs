namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class ItemOverview
    {
        public ItemOverview()
        {
			Active = true;
        }

        public ItemOverview(string name, string value) : this(name, value, true)
		{
        }
		public ItemOverview(string name, string value, bool active)
		{
			Name = name;
			Value = value;
			Active = active;
		}

		public string Name { get; set; }

        public string Value { get; set; }


		public bool Active { get; set; }

		public static ItemOverview CreateEmpty()
        {
            return new ItemOverview(string.Empty, string.Empty);
        }
    }
}