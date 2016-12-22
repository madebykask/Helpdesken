using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ECT.Model.Entities
{
    public class Department
    {
        public string Searchkey { get; set; }
        public string Name { get; set; }
        public string HeadOfDepartmentName { get; set; }
        public string HeadOfDepartmentTitle { get; set; }
        public string HeadOfDepartmentCity { get; set; }
        public string HeadOfDepartmentSignature { get; set; }
        public string Unit { get; set; }
        public string StrAddr { get; set; }
        public string CloseDay { get; set; }
        public string TelNbr { get; set; }

        public string HrManager { get; set; }
        public string StoreManager { get; set; }
        public string Extra { get; set; }


        public string StrAddr2 { get; set; }
        public string StrAddr3 { get; set; }
        public string StrAddr4 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

    }
}
