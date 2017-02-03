namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramHistoryFields
    {
        public ProgramHistoryFields()
        {
            Programs = new List<int>();
        }

        public ProgramHistoryFields(List<int> programs, string infoProduct)
        {
            Programs = programs;
            InfoProduct = infoProduct;
        }

        [NotNull]
        public List<int> Programs { get; private set; }  

        public string InfoProduct { get; set; }
    }
}