using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;

namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
	using System.Collections.Generic;
	using DH.Helpdesk.Common.ValidationAttributes;

	public class ComputerEditViewModel : BaseViewEditWorkstationModel
    {
		public List<string> FileUploadWhiteList { get; set; }

		public ComputerEditViewModel(int id, ComputerViewModel computerViewModel)
            : base(id)
        {
            ComputerViewModel = computerViewModel;
			//FileUploadWhiteList = computerViewModel.FileUploadWhiteList;
        }

        public int CustomerId { get; set; }

        public string DocumentFileKey { get; set; }

        public int CurrentLanguageId { get; set; }

        public bool UserHasInventoryAdminPermission { get; set; }

        public override WorkstationEditTabs Tab
        {
            get { return WorkstationEditTabs.Workstation; }
        }

        [NotNull]
        public ComputerViewModel ComputerViewModel { get; set; }
    }
}