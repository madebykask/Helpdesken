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
            ConfigurableFieldModel<ProgramsModel> program)
        {
            this.Program = program;
        }

        [NotNull]
        public ConfigurableFieldModel<ProgramsModel> Program { get; set; }

        public static ProgramEditModel CreateEmpty()
        {
            var program = ConfigurableFieldModel<ProgramsModel>.CreateUnshowable();
            program.Value = new ProgramsModel();

            return new ProgramEditModel(program);
        }

        public bool HasShowableFields()
        {
            return this.Program.Show;
        }
    }
}