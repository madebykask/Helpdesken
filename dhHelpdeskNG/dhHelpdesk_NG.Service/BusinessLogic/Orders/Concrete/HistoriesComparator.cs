namespace DH.Helpdesk.Services.BusinessLogic.Orders.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.Common.Extensions.String;
    using DH.Helpdesk.Common.Types;

    public sealed class HistoriesComparator : IHistoriesComparator
    {
        public HistoriesDifference Compare(
            HistoryOverview previousHistory,
            HistoryOverview currentHistory,
            List<LogOverview> currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs,
            FullOrderEditSettings settings)
        {
            return previousHistory == null
                ? CreateDifferenceForFirstHistory(currentHistory, currentHistoryLog, currentHistoryEmailLogs, settings)
                : CompareHistories(previousHistory, currentHistory, currentHistoryLog, currentHistoryEmailLogs, settings);
        }

        private static HistoriesDifference CreateDifferenceForFirstHistory(
            HistoryOverview firstHistory,
            List<LogOverview> log,
            List<EmailLogOverview> emailLogs,
            FullOrderEditSettings settings)
        {
            var history = new List<FieldDifference>();

            AddFirstDifference(history, settings.Delivery.DeliveryDate, firstHistory.Order.Delivery.DeliveryDate);
            AddFirstDifference(history, settings.Delivery.InstallDate, firstHistory.Order.Delivery.InstallDate);
            AddFirstDifference(history, settings.Delivery.DeliveryDepartment, firstHistory.Order.Delivery.DeliveryDepartmentId, firstHistory.Order.Delivery.DeliveryDepartment);
            AddFirstDifference(history, settings.Delivery.DeliveryOu, firstHistory.Order.Delivery.DeliveryOu);
            AddFirstDifference(history, settings.Delivery.DeliveryAddress, firstHistory.Order.Delivery.DeliveryAddress);
            AddFirstDifference(history, settings.Delivery.DeliveryPostalCode, firstHistory.Order.Delivery.DeliveryPostalCode);
            AddFirstDifference(history, settings.Delivery.DeliveryPostalAddress, firstHistory.Order.Delivery.DeliveryPostalAddress);
            AddFirstDifference(history, settings.Delivery.DeliveryLocation, firstHistory.Order.Delivery.DeliveryLocation);
            AddFirstDifference(history, settings.Delivery.DeliveryInfo1, firstHistory.Order.Delivery.DeliveryInfo1);
            AddFirstDifference(history, settings.Delivery.DeliveryInfo2, firstHistory.Order.Delivery.DeliveryInfo2);
            AddFirstDifference(history, settings.Delivery.DeliveryInfo3, firstHistory.Order.Delivery.DeliveryInfo3);

            var emails = emailLogs.Select(l => l.Email).ToList();
            var logs = log.Select(l => l.Text).ToList();

            return new HistoriesDifference(
                firstHistory.DateAndTime,
                firstHistory.RegisteredBy,
                logs,
                history,
                emails);
        }

        private static HistoriesDifference CompareHistories(
            HistoryOverview previousHistory,
            HistoryOverview currentHistory,
            List<LogOverview> currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs,
            FullOrderEditSettings settings)
        {
            var history = new List<FieldDifference>();

            var emails = currentHistoryEmailLogs.Select(l => l.Email).ToList();
            var logs = currentHistoryLog.Select(l => l.Text).ToList();

            if (logs.Any() || history.Any() || emails.Any())
            {
                return new HistoriesDifference(
                currentHistory.DateAndTime,
                currentHistory.RegisteredBy,
                logs,
                history,
                emails);
            }

            return null;
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, int? firstValue, string firstText)
        {
            if (firstValue.HasValue)
            {
                var difference = new FieldDifference(settings.Caption, null, firstText);
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, string firstText)
        {
            if (!string.IsNullOrEmpty(firstText))
            {
                var difference = new FieldDifference(settings.Caption, null, firstText);
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, UserName firstUserName)
        {
            if (firstUserName != null)
            {
                var difference = new FieldDifference(settings.Caption, null, firstUserName.GetFullName());
                differencies.Add(difference);
            }
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, DateTime? firstValue)
        {
            if (firstValue.HasValue)
            {
                var difference = new FieldDifference(settings.Caption, null, firstValue.Value.ToString(CultureInfo.InvariantCulture));
                differencies.Add(difference);
            }
        }

        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, int? oldValue, string oldText, int? newValue, string newText)
        {
            if (oldValue != newValue)
            {
                var difference = new FieldDifference(settings.Caption, oldText, newText);
                differencies.Add(difference);
            }
        }

        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, string oldText, string newText)
        {
            if (!oldText.EqualWith(newText))
            {
                var difference = new FieldDifference(settings.Caption, oldText, newText);
                differencies.Add(difference);
            }
        }

        private static void AddDifference(List<FieldDifference> differencies, FieldEditSettings settings, UserName oldUserName, UserName newUserName)
        {
            var oldText = oldUserName != null ? oldUserName.GetFullName() : string.Empty;
            var newText = newUserName != null ? newUserName.GetFullName() : string.Empty;

            AddDifference(differencies, settings, oldText, newText);
        }

        private static void AddFirstDifference(List<FieldDifference> differencies, FieldEditSettings settings, DateTime? oldValue, DateTime? newValue)
        {
            if (oldValue != newValue)
            {
                var difference = new FieldDifference(                            
                            settings.Caption, 
                            oldValue != null ? oldValue.Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
                            newValue != null ? newValue.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
                differencies.Add(difference);
            }
        }
    }
}