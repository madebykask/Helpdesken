using DH.Helpdesk.Common.Enums;
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
        private Customer customer;


        public ContractsSettingViewModel()
        {
            Languages = new List<SelectListItem>();
        }

        public int CustomerId { get; set; }
        public int CurrentLanguage { get; set; }
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

        public int VirtualOrder { get; set; }

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

        public void SetOrder()
        {
            switch (ContractField)
            {
                case EnumContractFieldSettings.Number:
                    VirtualOrder = 1;                        
                    break;

                case EnumContractFieldSettings.CaseNumber:
                    VirtualOrder = 2;
                    break;

                case EnumContractFieldSettings.Category:
                    VirtualOrder = 3;
                    break;

                case EnumContractFieldSettings.Supplier:
                    VirtualOrder = 4;
                    break;

                case EnumContractFieldSettings.Department:
                    VirtualOrder = 5;
                    break;

                case EnumContractFieldSettings.ResponsibleUser:
                    VirtualOrder = 6;
                    break;

                case EnumContractFieldSettings.StartDate:
                    VirtualOrder = 7;
                    break;

                case EnumContractFieldSettings.EndDate:
                    VirtualOrder = 8;
                    break;

                case EnumContractFieldSettings.NoticeDate:
                    VirtualOrder = 9;
                    break;

                case EnumContractFieldSettings.Filename:
                    VirtualOrder = 10;
                    break;

                case EnumContractFieldSettings.Other:
                    VirtualOrder = 11;
                    break;

                case EnumContractFieldSettings.Running:
                    VirtualOrder = 12;
                    break;

                case EnumContractFieldSettings.Finished:
                    VirtualOrder = 13;
                    break;

                case EnumContractFieldSettings.FollowUpField:
                    VirtualOrder = 14;
                    break;

                case EnumContractFieldSettings.ResponsibleFollowUpField:
                    VirtualOrder = 15;
                    break;

                default:
                    VirtualOrder = 0;
                    break;
            }
        }
    }

    public sealed class ContractsIndexColumnsModel
    {
        public ContractsIndexColumnsModel(Customer customer)
        {           
            Customer = customer;
            Columns = new List<ContractsSettingRowViewModel>();
        }
        public Customer Customer { get; private set; }
        public List<ContractsSettingRowViewModel> Columns { get; set; }
    }

    public sealed class ContractsSettingRowData
    {
        public string Id { get; set; }

        public string ContractField { get; set; }

        public bool Show { get; set; }

        public bool ShowInList { get; set; }

        public string CaptionEng { get; set; }

        public string CaptionSv { get; set; }

        public bool Required { get; set; }

        public int LanguageId { get; set; }
    }

}