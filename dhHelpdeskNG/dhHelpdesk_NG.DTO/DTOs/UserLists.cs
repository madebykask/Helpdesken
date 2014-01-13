
using System;
namespace dhHelpdesk_NG.DTO.DTOs
{
    public class UserLists
    {
        public int? User_Id { get; set; }
        public int? UserRole_Id { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public class CustomerWorkingGroupForUser
    {
        public int? IsStandard { get; set; }
        public int RoleToUWG { get; set; }
        public int User_Id { get; set; }
        public int WorkingGroup_Id { get; set; }
        public string CustomerName { get; set; }
        public string WorkingGroupName { get; set; }
    }

    public class LoggedOnUsersOnIndexPage
    {
        public decimal CaseNumber { get; set; }
        public int Customer_Id { get; set; }
        //public int User_Id { get; set; }
        public string CustomerName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public DateTime LatestActivity { get; set; }
        public DateTime LoggedOnLastTime { get; set; }
    }

}