namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.Edit
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels;

    public class Program
    {
        public Program()
        {
        }

        public Program(
            ConfigurableFieldModel<string> infoProduct,
            ConfigurableFieldModel<List<int>> programs,
            MultiSelectList allPrograms)
        {
            this.InfoProduct = infoProduct;
            this.Programs = programs;
            this.AllPrograms = allPrograms;
        }

        public ConfigurableFieldModel<string> InfoProduct { get; set; }

        public ConfigurableFieldModel<List<int>> Programs { get; set; }

        public MultiSelectList AllPrograms { get; set; }
    }
}