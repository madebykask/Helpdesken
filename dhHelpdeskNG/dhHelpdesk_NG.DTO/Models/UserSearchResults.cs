namespace DH.Helpdesk.BusinessData.Models
{
    using System;
	using System.Collections.Generic;
	using Domain.Computers;

	[Serializable]
    public class UserSearchResults
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string CellPhone { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string UserCode { get; set; }
        public int? Region_Id { get; set; }
        public int? Department_Id { get; set; }
        public int? OU_Id { get; set; }
        public string CostCentre { get; set; }
        public string RegionName { get; set; }
        public string DepartmentName { get; set; }
        public string OUName { get; set; }
		public int? CategoryID { get; internal set; }
		public string CategoryName { get; internal set; }
		public bool IsReadOnly { get; internal set; }

	}
}
