﻿using DH.Helpdesk.BusinessData.Models.Shared;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    public sealed class CaseInvoiceSettings
    {
        public CaseInvoiceSettings(
                int id, 
                int customerId, 
                string exportPath,
                string currency,
                string orderNoPrefix,
                string issuer,
                string ourReference,
                string docTemplate,
                string filter)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.ExportPath = exportPath;
            this.Currency = currency;
            this.OrderNoPrefix = orderNoPrefix;
            this.Issuer = issuer;
            this.OurReference = ourReference;
            this.DocTemplate = docTemplate;
            this.Filter = filter;
        }

        public CaseInvoiceSettings()
        {
            AvailableDepartments = new CustomSelectList();
            DisabledDepartments = new CustomSelectList();
        }

        public CaseInvoiceSettings(int customerId)
        {
            this.CustomerId = customerId;
            AvailableDepartments = new CustomSelectList();
            DisabledDepartments = new CustomSelectList();
        }


        public int Id { get; set; } 

        public int CustomerId { get; set; }

        public string ExportPath { get; set; }

        public string Currency { get; set; }

        public string OrderNoPrefix { get; set; }

        public string Issuer { get; set; }

        public string OurReference { get; set; }

        public string DocTemplate { get; set; }

        public string Filter { get; set; }

        public CustomSelectList AvailableDepartments { get; set; }
        public CustomSelectList DisabledDepartments { get; set; }

        public int[] DisabledDepartmentIds { get; set; }

    }

    public class MultiSelectListItem
    {
        public int Value { get; set; }

        public string Text { get; set; }

        public bool Selected { get; set; }

        public bool Disabled { get; set; }        
    }
}