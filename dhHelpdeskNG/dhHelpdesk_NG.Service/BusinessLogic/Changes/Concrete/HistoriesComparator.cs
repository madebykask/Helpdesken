﻿namespace DH.Helpdesk.Services.BusinessLogic.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;

    public sealed class HistoriesComparator : IHistoriesComparator
    {
        public HistoriesDifference Compare(
            HistoryOverview previousHistory,
            HistoryOverview currentHistory,
            List<LogOverview> currentHistoryLog,
            List<EmailLogOverview> currentHistoryEmailLogs)
        {
            return previousHistory == null
                ? CreateDifferenceForFirstHistory(currentHistory, currentHistoryLog, currentHistoryEmailLogs)
                : CompareHistories(previousHistory, currentHistory, currentHistoryLog, currentHistoryEmailLogs);
        }

        private static HistoriesDifference CreateDifferenceForFirstHistory(
            HistoryOverview firstHistory,
            List<LogOverview> log,
            List<EmailLogOverview> emailLogs)
        {
            var history = new List<FieldDifference>();

            if (firstHistory.StatusId.HasValue)
            {
                var difference = new FieldDifference("Status", null, firstHistory.Status);
                history.Add(difference);
            }

            if (firstHistory.SystemId.HasValue)
            {
                var difference = new FieldDifference("System", null, firstHistory.System);
                history.Add(difference);
            }

            if (firstHistory.ObjectId.HasValue)
            {
                var difference = new FieldDifference("Object", null, firstHistory.Object);
                history.Add(difference);
            }

            if (firstHistory.WorkingGroupId.HasValue)
            {
                var difference = new FieldDifference("WorkingGroup", null, firstHistory.WorkingGroup);
                history.Add(difference);
            }

            if (firstHistory.AdministratorId.HasValue)
            {
                var dirrefence = new FieldDifference(
                                "Administrator", 
                                null, 
                                firstHistory.Administrator != null ? firstHistory.Administrator.GetFullName() : string.Empty);
                history.Add(dirrefence);
            }

            if (firstHistory.CategoryId.HasValue)
            {
                var difference = new FieldDifference("Category", null, firstHistory.Category);
                history.Add(difference);
            }

            if (firstHistory.PriorityId.HasValue)
            {
                var difference = new FieldDifference("Priority", null, firstHistory.Priority);
                history.Add(difference);
            }

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
            List<EmailLogOverview> currentHistoryEmailLogs)
        {
            var history = new List<FieldDifference>();

            if (previousHistory.StatusId != currentHistory.StatusId)
            {
                var difference = new FieldDifference("Status", previousHistory.Status, currentHistory.Status);
                history.Add(difference);
            }

            if (previousHistory.SystemId != currentHistory.SystemId)
            {
                var difference = new FieldDifference("System", previousHistory.System, currentHistory.System);
                history.Add(difference);
            }

            if (previousHistory.ObjectId != currentHistory.ObjectId)
            {
                var difference = new FieldDifference("Object", previousHistory.Object, currentHistory.Object);
                history.Add(difference);
            }

            if (previousHistory.WorkingGroupId != currentHistory.WorkingGroupId)
            {
                var difference = new FieldDifference(
                    "WorkingGroup",
                    previousHistory.WorkingGroup,
                    currentHistory.WorkingGroup);

                history.Add(difference);
            }

            if (previousHistory.AdministratorId != currentHistory.AdministratorId)
            {
                var difference = new FieldDifference(
                    "Administrator",
                    previousHistory.Administrator != null ? previousHistory.Administrator.GetFullName() : string.Empty,
                    currentHistory.Administrator != null ? currentHistory.Administrator.GetFullName() : string.Empty);

                history.Add(difference);
            }

            if (previousHistory.CategoryId != currentHistory.CategoryId)
            {
                var difference = new FieldDifference("Category", previousHistory.Category, currentHistory.Category);
                history.Add(difference);
            }

            if (previousHistory.PriorityId != currentHistory.PriorityId)
            {
                var difference = new FieldDifference("Priority", previousHistory.Priority, currentHistory.Priority);
                history.Add(difference);
            }

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
    }
}