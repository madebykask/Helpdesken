namespace DH.Helpdesk.Web.Models.Shared
{
    public class ChangePasswordModel
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public bool UseComplexPassword { get; set; }
        public int MinPasswordLength { get; set; }
    }
}