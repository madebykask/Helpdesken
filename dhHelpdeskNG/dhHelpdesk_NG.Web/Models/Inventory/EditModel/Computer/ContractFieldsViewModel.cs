namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFieldsViewModel
    {
        public ContractFieldsViewModel()
        {
        }

        public ContractFieldsViewModel(ContractFieldsModel contractFieldsModel, ConfigurableFieldModel<SelectList> contractStatuses)
        {
            this.ContractFieldsModel = contractFieldsModel;
            this.ContractStatuses = contractStatuses;
        }

        [NotNull]
        public ContractFieldsModel ContractFieldsModel { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> ContractStatuses { get; set; }
    }
}