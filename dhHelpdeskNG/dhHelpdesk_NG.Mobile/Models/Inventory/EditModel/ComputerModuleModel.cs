namespace DH.Helpdesk.Web.Models.Inventory.EditModel
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ComputerModuleEditModel
    {
        public ComputerModuleEditModel()
        {
        }

        public ComputerModuleEditModel(ModuleTypes moduleType)
        {
            this.ModuleType = moduleType;
        }

        public ComputerModuleEditModel(int id, string name, ModuleTypes moduleType)
        {
            this.Id = id;
            this.Name = name;
            this.ModuleType = moduleType;
        }

        [IsId]
        public int Id { get; set; }

        [LocalizedRequired]
        public string Name { get; set; }

        public ModuleTypes ModuleType { get; set; }
    }
}