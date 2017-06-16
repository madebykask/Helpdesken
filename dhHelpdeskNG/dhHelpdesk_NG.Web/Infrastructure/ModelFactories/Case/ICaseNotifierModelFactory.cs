namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case
{
    using DH.Helpdesk.BusinessData.Models.Case.Input;

    public interface ICaseNotifierModelFactory
    {
        CaseNotifier Create(
            string userId,
            string firstName,
            string sureName,
            string email,
            string phone,
            string cellphone,
            int? departmentId,
            int? ouId,
            string place,
            string userCode,
            int? customerId,
            int languageid);
    }
}