using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DH.Helpdesk.Web.Models.Contract
{    

    public class ContractsSettingViewModel
    {

        public ContractsSettingViewModel()
        {
            Languages = new List<SelectListItem>();
        }

        public int customer_id { get; set; }
        public int language_id { get; set; }
        public List<SelectListItem> Languages { get; set; }
        public List<ContractsSettingRowViewModel> SettingRows { get; set; }
    }

    public sealed class ContractsSettingRowViewModel
    {
        public int Id { get; set; }

        public string ContractField { get; set; }

        public bool Show { get; set; }

        public bool ShowInList { get; set; }

        public string ContractFieldLable { get; set; }

        public string ContractFieldLable_Eng { get; set; }

        public bool Required { get; set; }

        public static ContractsSettingRowViewModel CreateEmpty(string contractFieldId)
        {
            var ret = new ContractsSettingRowViewModel();
            ret.Id = 0;
            ret.ContractField = contractFieldId;
            ret.Show = true;
            ret.ShowInList = true;
            ret.Required = true;
            ret.ContractFieldLable = "";
            ret.ContractFieldLable_Eng = "";

            return ret;
        }       
    }

    public sealed class ContractFieldsViewModel
    {
        public ContractFieldsViewModel(Customer customer)
        {
            Data = new List<ContractsSettingRowViewModel>();
            Customer = customer;
        }
        public Customer Customer { get; private set; }
        public List<ContractsSettingRowViewModel> Data { get; set; }
    }

    public sealed class JSContractsSettingRowViewModel
    {

        public JSContractsSettingRowViewModel()
        { }
        public string Id { get; set; }

        public string ContractField { get; set; }

        public string Show { get; set; }

        public string ShowInList { get; set; }

        public string Caption_Eng { get; set; }

        public string Caption_Sv { get; set; }

        public string Required { get; set; }

        public string LanguageId { get; set; }
    }
}