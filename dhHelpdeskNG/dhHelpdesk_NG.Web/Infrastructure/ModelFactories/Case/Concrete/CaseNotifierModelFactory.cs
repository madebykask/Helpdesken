namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Case.Input;

    internal sealed class CaseNotifierModelFactory : ICaseNotifierModelFactory
    {
        public CaseNotifier Create(
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
            int languageid)
        {
            var instance = new CaseNotifier(
                userId,
                firstName,
                sureName,
                email,
                phone,
                cellphone,
                departmentId,
                ouId,
                place,
                userCode,
                customerId,
                languageid);

            return instance;
        }
    }
}