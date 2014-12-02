namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProgramsModel
    {
        public ProgramsModel()
        {
            this.Programs = new List<ProgramModel>();
        }

        public ProgramsModel(int orderId, List<ProgramModel> programs)
        {
            this.OrderId = orderId;
            this.Programs = programs;
        }

        [IsId]
        public int OrderId { get; set; }

        [NotNull]
        public List<ProgramModel> Programs { get; set; }
    }
}