
namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer
{
    public class ComputerTypeOverview 
    {
        public ComputerTypeOverview(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}