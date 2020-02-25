namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
	using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
	using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;
	using DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates;
	using System.Collections.Generic;

	public interface IComputerViewModelBuilder
    {
        ComputerViewModel BuildViewModel(
            BusinessData.Models.Inventory.Edit.Computer.ComputerForRead model,
            ComputerEditOptions options,
            ComputerFieldsSettingsForModelEdit settings,
			List<string> fileUploadWhiteList);

        ComputerViewModel BuildViewModel(
            ComputerEditOptions options,
            ComputerFieldsSettingsForModelEdit settings,
            int currentCustomerId,
			List<string> fileUploadWhiteList);
    }
}