using System.Collections.Generic;

namespace ECT.Model.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int CustomerId { get; set; }
        public string Language { get; set; }
        public string RegUserId { get; set; }
        public string RegUserDomain { get; set; }
        public IList<WorkingGroup> WorkingGroups { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + Surname;
            }
        }
    }
}
