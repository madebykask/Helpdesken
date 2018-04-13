namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ComputerModuleEditModel
    {
        public ComputerModuleEditModel()
        {
        }

        public ComputerModuleEditModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        [IsId]
        public int Id { get; set; }

        [LocalizedRequired]
        public string Name { get; set; }

        [LocalizedStringLengthAttribute(50)]
        public string Description { get; set; }

        public bool UserHasInventoryAdminPermission { get; set; }
    }
}