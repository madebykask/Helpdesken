namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AttachedFilesModel
    {
        public AttachedFilesModel()
        {
            this.Files = new List<string>();    
        }

        public AttachedFilesModel(string orderId, Subtopic area, List<string> files)
        {
            this.OrderId = orderId;
            this.Area = area;
            this.Files = files;
        }

        public string OrderId { get; set; }

        public Subtopic Area { get; set; }

        [NotNull]
        public List<string> Files { get; set; }
    }
}