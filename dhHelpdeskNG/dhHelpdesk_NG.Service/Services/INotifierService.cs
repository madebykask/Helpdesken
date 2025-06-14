﻿namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.Case.Input;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;

    public interface INotifierService
    {
        void AddNotifier(Notifier notifier);

        void UpdateNotifier(Notifier notifier, int customerId);

        void DeleteNotifier(int notifierId);

        void UpdateSettings(FieldSettings settings);

        void UpdateCaseNotifier(CaseNotifier caseNotifier);
    }
}