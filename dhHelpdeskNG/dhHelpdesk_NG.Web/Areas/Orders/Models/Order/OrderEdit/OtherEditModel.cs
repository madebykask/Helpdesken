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
            ConfigurableFieldModel<string> info)
        {
            this.FileName = fileName;
            this.CaseNumber = caseNumber;
            this.Info = info;
        }

        [NotNull]
        public ConfigurableFieldModel<AttachedFilesModel> FileName { get; set; }

        [NotNull]
        public ConfigurableFieldModel<decimal?> CaseNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Info { get; set; }

        public static OtherEditModel CreateEmpty()
        {
            var files = ConfigurableFieldModel<AttachedFilesModel>.CreateUnshowable();
            files.Value = new AttachedFilesModel();

            return new OtherEditModel(
                files,
                ConfigurableFieldModel<decimal?>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return this.FileName.Show ||
                this.CaseNumber.Show ||
                this.Info.Show;
        }
    }
}