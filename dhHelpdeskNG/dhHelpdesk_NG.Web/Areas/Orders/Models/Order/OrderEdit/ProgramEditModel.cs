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

        public bool HasShowableFields()
        {
            return this.Program.Show;
        }
    }
}