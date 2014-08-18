namespace DH.Helpdesk.BusinessData.Models.Operation
{
    public abstract class OperationObject
    {
        protected OperationObject(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
