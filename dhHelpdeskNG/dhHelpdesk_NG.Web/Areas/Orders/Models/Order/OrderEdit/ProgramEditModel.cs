using System.Collections.Generic;
using System.Web.Mvc;
using DH.Helpdesk.Web.Models;

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
            ConfigurableFieldModel<string> infoProduct,
            ConfigurableFieldModel<List<CheckBoxListItem>> programs)
        {
            InfoProduct = infoProduct;
            Programs = programs;
        }

        public ConfigurableFieldModel<List<CheckBoxListItem>> Programs { get; set; }

        [NotNull]
        public MultiSelectList AllPrograms { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> InfoProduct { get; set; }

        public static ProgramEditModel CreateEmpty()
        {
            var programs = ConfigurableFieldModel<List<CheckBoxListItem>>.CreateUnshowable();
            return new ProgramEditModel(ConfigurableFieldModel<string>.CreateUnshowable(), programs);
        }

        public bool HasShowableFields()
        {
            return Programs.Show ||
                InfoProduct.Show;
        }
    }
}