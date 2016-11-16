namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class ItemOverview
    {
        public ItemOverview(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }

        public string Value { get; private set; }

        public static ItemOverview CreateEmpty()
        {
            return new ItemOverview(string.Empty, string.Empty);
        }
    }
}