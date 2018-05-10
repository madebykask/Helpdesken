using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.BusinessData.Models.User
{
    public class CustomerUserInfo : IUserCommon
    {
        public int Id { get; set; }
        public int IsActive { get; set; }
        public int Performer { get; set; }
        public int UserGroupId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }

        public IList<UserWorkingGroupOverview> WorkingGroups { get; set; }
    }
}