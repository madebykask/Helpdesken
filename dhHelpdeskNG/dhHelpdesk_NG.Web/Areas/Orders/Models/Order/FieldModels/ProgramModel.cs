namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels
{
    using DH.Helpdesk.Common.ValidationAttributes;

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