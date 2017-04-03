using DH.Helpdesk.Common.ValidationAttributes;

namespace DH.Helpdesk.SelfService.Models.Orders.FieldModels
{
    public sealed class ProgramModel
    {
        public ProgramModel(
                int id, 
                string name)
        {
            this.Name = name;
            this.Id = id;
        }

        public int Id { get; private set; }

        [NotNull]
        public string Name { get; private set; }
    }
}