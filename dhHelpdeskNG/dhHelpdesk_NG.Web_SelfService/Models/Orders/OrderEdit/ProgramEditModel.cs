using System.Collections.Generic;
using System.Web.Mvc;
using DH.Helpdesk.Common.ValidationAttributes;
using DH.Helpdesk.SelfService.Models.Orders.FieldModels;

namespace DH.Helpdesk.SelfService.Models.Orders.OrderEdit
{
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

        public string Header { get; set; }


        [NotNull]
        public MultiSelectList AllPrograms { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> InfoProduct { get; set; }
        public ConfigurableFieldModel<List<CheckBoxListItem>> Programs { get; set; }

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