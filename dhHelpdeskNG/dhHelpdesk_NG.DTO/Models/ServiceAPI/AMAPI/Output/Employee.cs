using System;
using System.Collections.Generic;
namespace DH.Helpdesk.BusinessData.Models.ServiceAPI.AMAPI.Output
{
    public class APIEmployee
    {
        public bool IsManager { get; set; }
        public List<SubordinateResponseItem> Subordinates { get; set; }
    }

    public class SubordinateResponseItem
    {
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public Dictionary<string, Dictionary<string, string>> ExtraInfo { get; set; }
    }

}