namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramHistoryFields
    {
        public ProgramHistoryFields()
        {
            Programs = new List<OrderProgramModel>();
        }

        public ProgramHistoryFields(List<OrderProgramModel> programs, string infoProduct)
        {
            Programs = programs;
            InfoProduct = infoProduct;
        }

        [NotNull]
        public List<OrderProgramModel> Programs { get; private set; }  

        public string InfoProduct { get; set; }
    }
}