namespace DH.Helpdesk.BusinessData.Models.Case.Input
{
    using DH.Helpdesk.Common.Constraints;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseNotifier
    {
        public CaseNotifier(
            string userId, 
            string firstName, 
            string email, 
            string phone, 
            string cellphone, 
            int? departmentId, 
            int? ouId, 
            string place, 
            string userCode)
        {
            this.UserCode = userCode;
            this.Place = place;
            this.OuId = ouId;
            this.DepartmentId = departmentId;
            this.Cellphone = cellphone;
            this.Phone = phone;
            this.Email = email;
            this.UserId = userId;
            this.FirstName = firstName;
        }

        [MaxLength(NotifierConstraint.UserIdMaxLength)]
        public string UserId { get; private set; }

        [MaxLength(NotifierConstraint.FirstNameMaxLength)]
        public string FirstName { get; private set; }

        [MaxLength(NotifierConstraint.EmailMaxLength)]
        public string Email { get; private set; }

        [MaxLength(NotifierConstraint.PhoneMaxLength)]
        public string Phone { get; private set; }

        [MaxLength(NotifierConstraint.CellPhoneMaxLength)]
        public string Cellphone { get; private set; }

        public int? DepartmentId { get; private set; }

        public int? OuId { get; private set; }

        [MaxLength(NotifierConstraint.PlaceMaxLenght)]
        public string Place { get; private set; }

        [MaxLength(NotifierConstraint.UserCodeMaxLength)]
        public string UserCode { get; private set; }
    }
}