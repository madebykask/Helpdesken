using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.Contract
{   
    public sealed class ContractViewInputModel
    {
        public ContractViewInputModel()
        {
            NoticeTimes = new List<SelectListItem>();
            FollowUpIntervals = new List<SelectListItem>();
        }

        public List<ContractsSettingRowViewModel> SettingsModel { get; set; }

        public List<ContractFileViewModel> ContractFiles { get; set; }

        public string ContractFileKey { get; set; }

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }

        public int DepartmentId { get; set; }

        public int ResponsibleUserId { get; set; }

        public int FollowUpResponsibleUserId { get; set; }

        public int NoticeTimeId { get; set; }

        public int FollowUpIntervalId { get; set; }

        public List<SelectListItem> ContractCategories { get; set; }

        public List<SelectListItem> Suppliers { get; set; }

        public int ContractId { get; set; }

        public string ContractNumber { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public List<SelectListItem> NoticeTimes { get; set; }

        public bool Finished { get; set; }

        public bool Running { get; set; }

        public List<SelectListItem> FollowUpIntervals { get; set; }

        public string Other { get; set; }

        public DateTime? NoticeDate { get; set; }

        public List<SelectListItem> Departments { get; set; }

        public List<SelectListItem> ResponsibleUsers { get; set; }

        public List<SelectListItem> FollowUpResponsibleUsers { get; set; }

        public string ChangedByUser { get; set; }

        public string CreatedDate { get; set; }

        public string ChangedDate { get; set; }

		public List<string> FileUploadWhiteList { get; set; }

	}
}