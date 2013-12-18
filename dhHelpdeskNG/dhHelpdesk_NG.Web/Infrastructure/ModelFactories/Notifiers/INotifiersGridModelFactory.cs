namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public interface INotifiersGridModelFactory
    {
        NotifiersGridModel Create(List<NotifierDetailedOverviewDto> notifiers, FieldSettingsDto displaySettings);
    }
}