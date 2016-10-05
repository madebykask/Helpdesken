namespace DH.Helpdesk.BusinessData.Models.Contract
{
    using DH.Helpdesk.BusinessData.Models.Shared;
    using System;
    using System.Collections.Generic;


    public class ContractsSettingRowModel
    {
        public ContractsSettingRowModel() { }

        public ContractsSettingRowModel(
                int id,
                int customerId,
                string contractField,
                bool show,
                bool showInList,
                string contractFieldLable,
                string contractFieldLable_Eng,                
                bool reguired,
                DateTime createdDate,
                DateTime changedDate
            )
        {
            this.CustomerId = customerId;
            this.Id = id;
            this.ContractField = contractField;
            this.show = show;
            this.showInList = showInList;
            this.ContractFieldLable = contractFieldLable;
            this.ContractFieldLable_Eng = contractFieldLable_Eng;
            this.reguired = reguired;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string ContractField { get; set; }

        public bool show { get; set; }

        public bool showInList { get; set; }

        public bool ShowExternal { get; set; }

        public string ContractFieldLable { get; set; }

        public string ContractFieldLable_Eng { get; set; }

        public string FieldHelp { get; set; }

        public bool reguired { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

    }
}