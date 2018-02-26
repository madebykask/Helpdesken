using System;

namespace DH.Helpdesk.Common.Types
{
    public interface IUserIdentity
    {
        string Domain { get; }
        string UserId { get; }
        string EmployeeNumber { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string Phone { get; }

        string GetUserIdWithDomain();
    }

    public sealed class UserIdentity : IUserIdentity
    {
        public UserIdentity()
        {
        }

        public UserIdentity(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }

        public string Domain { get; set; }

        public string EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Email { get; set; }

        public string Phone { get; set; }
        public string GetUserIdWithDomain()
        {
            if (!string.IsNullOrEmpty(UserId))
            {
                //check if userId already has domain name
                if (UserId.IndexOf(@"\\", StringComparison.Ordinal) != -1)
                    return UserId;

                if (!string.IsNullOrEmpty(Domain))
                    return $@"{Domain}\\{UserId}";
            }

            return UserId;
        }
    }
}
