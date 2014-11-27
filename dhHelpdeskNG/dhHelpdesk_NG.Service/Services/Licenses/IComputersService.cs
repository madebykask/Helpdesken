namespace DH.Helpdesk.Services.Services.Licenses
{
    using DH.Helpdesk.BusinessData.Models.Licenses.Computers;

    public interface IComputersService
    {
        ComputerOverview[] GetComputerOverviews(int customerId, int productId);
    }
}