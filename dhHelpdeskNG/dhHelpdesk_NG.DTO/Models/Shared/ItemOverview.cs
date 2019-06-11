namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class ItemOverview
    {
        public ItemOverview()
        {
        }

        public ItemOverview(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public static ItemOverview CreateEmpty()
        {
            return new ItemOverview(string.Empty, string.Empty);
        }
    }
}