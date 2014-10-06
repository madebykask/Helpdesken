namespace DH.Helpdesk.Mobile.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;

    public class ComputerUserIndexViewModel
    {
        public string SearchCompUs { get; set; }

        public IEnumerable<ComputerUserFieldSettings> ComputerUserFieldSettings { get; set; }

        public IList<ComputerUser> ComputerUsers { get; set; }

        public IList<SelectListItem> Departments { get; set; }
        public IList<SelectListItem> Divisions { get; set; }
        public IList<SelectListItem> Domains { get; set; }
        public IList<SelectListItem> OUs { get; set; }
        public IList<SelectListItem> Regions { get; set; }
        public IList<SelectListItem> StatusComputerUsers { get; set; }
    }

    public class ComputerUserInputViewModel
    {
        public ComputerUser ComputerUser { get; set; }
        public Department Department { get; set; }

        public IEnumerable<ComputerUserFieldSettings> ComputerUserFieldSettings { get; set; }

        public IList<SelectListItem> ComputerUserManagers { get; set; }
        public IList<SelectListItem> ComputerUserGroups { get; set; }
        public IList<SelectListItem> Departments { get; set; }
        public IList<SelectListItem> Divisions { get; set; }
        public IList<SelectListItem> Domains { get; set; }
        public IList<SelectListItem> OUs { get; set; }
        public IList<SelectListItem> Regions { get; set; }

        public IList<sDepartment> sDepartments { get; set; }
        public IList<sOU> sOUs { get; set; }

        public IList<SelectListItem> CUGsAvailable { get; set; }
        public IList<SelectListItem> CUGsSelected { get; set; }
    }

    public struct sDepartment
    {
        public int Id { get; set; }
        public int Customer_Id { get; set; }
        public int? Region_Id { get; set; }
        public string Name { get; set; }
    }

    public struct sOU
    {
        public int Id { get; set; }
        public int Customer_Id { get; set; }
        public int? Department_Id { get; set; }
        public string Name { get; set; }
    }
}