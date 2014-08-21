namespace DH.Helpdesk.BusinessData.Models.Operation
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class OperationObject : INewBusinessModel
    {
        protected OperationObject(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        [IsId]
        public int Id { get; set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
