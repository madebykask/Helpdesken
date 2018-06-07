namespace DH.Helpdesk.BusinessData.Models.Shared
{
    public class EntityOverview
    {
        public EntityOverview()
        {
        }

        public EntityOverview(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}