namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class OtherEditModel
    {
        public OtherEditModel()
        {            
        }

        public OtherEditModel(
            ConfigurableFieldModel<AttachedFilesModel> fileName,
            ConfigurableFieldModel<decimal?> caseNumber,
            ConfigurableFieldModel<string> info,
            ConfigurableFieldModel<SelectList> status)
        {
            this.FileName = fileName;
            this.CaseNumber = caseNumber;
            this.Info = info;
            this.Status = status;
        }

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> FileName { get; set; }

        [NotNull]
        public ConfigurableFieldModel<decimal?> CaseNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Info { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Status { get; set; }

        [IsId]
        public int? StatusId { get; set; }
    }
}