namespace DH.Helpdesk.BusinessData.Models.Case.Input
{
    public sealed class CasePerformersSearch
    {
        public CasePerformersSearch(
            int userId, 
            string firstName,
            string sureName,            
            string workingGroupName,
            int workingGroupId)
        {
            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = sureName;
            this.WorkingGroupId = workingGroupId;
            this.WorkingGroupName = workingGroupName;
        }

        public int UserId { get;  set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }     

        public string WorkingGroupName { get; set; }

        public int WorkingGroupId { get; set; }
      
    }
}