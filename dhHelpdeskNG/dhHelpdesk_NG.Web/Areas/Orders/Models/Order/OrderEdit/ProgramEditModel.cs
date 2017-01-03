namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class ProgramEditModel
    {
        public ProgramEditModel()
        {            
        }

        public ProgramEditModel(
            ConfigurableFieldModel<ProgramsModel> program,
            ConfigurableFieldModel<string> infoProduct)
        {
            Program = program;
            InfoProduct = infoProduct;
        }

        [NotNull]
        public ConfigurableFieldModel<ProgramsModel> Program { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> InfoProduct { get; set; }

        public static ProgramEditModel CreateEmpty()
        {
            var program = ConfigurableFieldModel<ProgramsModel>.CreateUnshowable();
            program.Value = new ProgramsModel();
            

            return new ProgramEditModel(program,
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return Program.Show ||
                InfoProduct.Show;
        }
    }
}