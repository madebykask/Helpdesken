namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramEditFields
    {
        public ProgramEditFields()
        {
            this.Programs = new List<int>();
        }

        public ProgramEditFields(List<int> programs, string infoProduct)
        {
            Programs = programs;
            InfoProduct = infoProduct;
        }

        [NotNull]
        public List<int> Programs { get; private set; }

        public string InfoProduct { get; private set; }

    }
}