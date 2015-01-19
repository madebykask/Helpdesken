namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramHistoryFields
    {
        public ProgramHistoryFields()
        {
            this.Programs = new List<OrderProgramModel>();
        }

        public ProgramHistoryFields(List<OrderProgramModel> programs)
        {
            this.Programs = programs;
        }

        [NotNull]
        public List<OrderProgramModel> Programs { get; private set; }  
    }
}