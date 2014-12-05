namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ContractFieldsViewModel
    {
        public ContractFieldsViewModel()
        {
        }

        public ContractFieldsViewModel(ContractFieldsModel contractFieldsModel, SelectList contractStatuses)
        {
            this.ContractFieldsModel = contractFieldsModel;
            this.ContractStatuses = contractStatuses;
        }

        [NotNull]
        public ContractFieldsModel ContractFieldsModel { get; set; }

        [NotNull]
        public SelectList ContractStatuses { get; set; }
    }
}