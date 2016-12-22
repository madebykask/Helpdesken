using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.EForm.Model.Entities
{
    public class Company
    {
        public string Searchkey { get; set; }
        public string EmployerName { get; set; }
        public string CountryEmployer { get; set; }
        public string HeaderName { get; set; }
        public string footerName { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string NIP { get; set; }
        public string Regon { get; set; }
        public string KRSNo { get; set; }
        public string KapitalNo { get; set; }
        public string INGBankNo { get; set; }

        public string Name { get; set; } //Add by TAN 2015-12-22
        public int Id { get; set; } //Add by TAN 2016-02-19
    }
}
