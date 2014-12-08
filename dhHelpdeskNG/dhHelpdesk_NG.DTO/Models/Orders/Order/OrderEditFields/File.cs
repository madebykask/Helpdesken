namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class File
    {
        public File(Subtopic subtopic, string name)
        {
            this.Subtopic = subtopic;
            this.Name = name;
        }

        public Subtopic Subtopic { get; private set; }

        [NotNullAndEmpty]
        public string Name { get; private set; }
    }
}