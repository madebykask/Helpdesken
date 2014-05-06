namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Case.Input;

    internal sealed class CaseNotifierModelFactory : ICaseNotifierModelFactory
    {
        public CaseNotifier Create(
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
            var instance = new CaseNotifier(
                userId,
                firstName,
                email,
                phone,
                cellphone,
                departmentId,
                ouId,
                place,
                userCode);

            return instance;
        }
    }
}