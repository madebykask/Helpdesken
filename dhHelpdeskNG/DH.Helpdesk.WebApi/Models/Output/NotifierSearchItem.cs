namespace DH.Helpdesk.WebApi.Models.Output
{
    public class NotifierSearchItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string UserCode { get; set; }
        
    }
}