namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramEditFields
    {
        public ProgramEditFields()
        {
            this.Programs = new List<OrderProgramModel>();
        }

        public ProgramEditFields(List<OrderProgramModel> programs)
        {
            this.Programs = programs;
        }

        [NotNull]
        public List<OrderProgramModel> Programs { get; private set; }  
    }
}