namespace dhHelpdesk_NG.Data.Repositories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Notifiers;

    public interface INotifiersRepository : IRepository<ComputerUser>
    {
        DisplayNotifierDto FindById(int notifierId);

        void AddNotifier(NewNotifierDto notifier);

        void UpdateNotifier(UpdatedNotifierDto notifier);

        List<NotifierDetailedOverviewDto> FindDetailedOverviewsByCustomerId(int customerId);

        List<ItemOverviewDto> FindOverviewsByCustomerId(int customerId);

        IList<UserSearchResults> Search(int customerId, string searchFor);

        List<NotifierDetailedOverviewDto> SearchDetailedOverviews(
            int customerId,
            int? domainId,
            int? departmentId,
            int? divisionId,
            string pharse,
            EntityStatus status,
            int selectCount);
    }
}
