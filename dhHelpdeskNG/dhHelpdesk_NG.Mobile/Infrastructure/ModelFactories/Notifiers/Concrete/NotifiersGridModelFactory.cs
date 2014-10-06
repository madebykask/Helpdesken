namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.SettingsEdit;
    using DH.Helpdesk.Common.Extensions.DateTime;
    using DH.Helpdesk.Mobile.Models.Notifiers;
    using DH.Helpdesk.Mobile.Models.Shared;

    public sealed class NotifiersGridModelFactory : INotifiersGridModelFactory
    {
        public NotifiersGridModel Create(SearchResult searchResult, FieldSettings settings, SortFieldModel sortField)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(settings.UserId, GeneralField.UserId, headers);
            CreateHeaderIfNeeded(settings.Domain, GeneralField.Domain, headers);
            CreateHeaderIfNeeded(settings.LoginName, GeneralField.LoginName, headers);
            CreateHeaderIfNeeded(settings.FirstName, GeneralField.FirstName, headers);
            CreateHeaderIfNeeded(settings.Initials, GeneralField.Initials, headers);
            CreateHeaderIfNeeded(settings.LastName, GeneralField.LastName, headers);
            CreateHeaderIfNeeded(settings.DisplayName, GeneralField.DisplayName, headers);
            CreateHeaderIfNeeded(settings.Place, GeneralField.Place, headers);
            CreateHeaderIfNeeded(settings.Phone, GeneralField.Phone, headers);
            CreateHeaderIfNeeded(settings.CellPhone, GeneralField.CellPhone, headers);
            CreateHeaderIfNeeded(settings.Email, GeneralField.Email, headers);
            CreateHeaderIfNeeded(settings.Code, GeneralField.Code, headers);
            CreateHeaderIfNeeded(settings.PostalAddress, AddressField.PostalAddress, headers);
            CreateHeaderIfNeeded(settings.PostalCode, AddressField.PostalCode, headers);
            CreateHeaderIfNeeded(settings.City, AddressField.City, headers);
            CreateHeaderIfNeeded(settings.Title, OrganizationField.Title, headers);
            CreateHeaderIfNeeded(settings.Department, OrganizationField.Department, headers);
            CreateHeaderIfNeeded(settings.Unit, OrganizationField.Unit, headers);
            CreateHeaderIfNeeded(settings.OrganizationUnit, OrganizationField.OrganizationUnit, headers);
            CreateHeaderIfNeeded(settings.Division, OrganizationField.Division, headers);
            CreateHeaderIfNeeded(settings.Manager, OrganizationField.Manager, headers);
            CreateHeaderIfNeeded(settings.Group, OrganizationField.Group, headers);
            CreateHeaderIfNeeded(settings.Other, OrganizationField.Other, headers);
            CreateHeaderIfNeeded(settings.Ordered, OrdererField.Orderer, headers);
            CreateHeaderIfNeeded(settings.CreatedDate, StateField.CreatedDate, headers);
            CreateHeaderIfNeeded(settings.ChangedDate, StateField.ChangedDate, headers);
            CreateHeaderIfNeeded(settings.SynchronizationDate, StateField.SynchronizationDate, headers);

            var notifierRows = new List<NotifierDetailedOverviewModel>(searchResult.Notifiers.Count);

            foreach (var notifier in searchResult.Notifiers)
            {
                var cellValues = new List<GridRowCellValueModel>();

                CreateValueIfNeeded(settings.UserId, GeneralField.UserId, notifier.UserId, cellValues);
                CreateValueIfNeeded(settings.Domain, GeneralField.Domain, notifier.Domain, cellValues);
                CreateValueIfNeeded(settings.LoginName, GeneralField.LoginName, notifier.LoginName, cellValues);
                CreateValueIfNeeded(settings.FirstName, GeneralField.FirstName, notifier.FirstName, cellValues);
                CreateValueIfNeeded(settings.Initials, GeneralField.Initials, notifier.Initials, cellValues);
                CreateValueIfNeeded(settings.LastName, GeneralField.LastName, notifier.LastName, cellValues);
                CreateValueIfNeeded(settings.DisplayName, GeneralField.DisplayName, notifier.DisplayName, cellValues);
                CreateValueIfNeeded(settings.Place, GeneralField.Place, notifier.Place, cellValues);
                CreateValueIfNeeded(settings.Phone, GeneralField.Phone, notifier.Phone, cellValues);
                CreateValueIfNeeded(settings.CellPhone, GeneralField.CellPhone, notifier.CellPhone, cellValues);
                CreateValueIfNeeded(settings.Email, GeneralField.Email, notifier.Email, cellValues);
                CreateValueIfNeeded(settings.Code, GeneralField.Code, notifier.Code, cellValues);

                CreateValueIfNeeded(
                    settings.PostalAddress,
                    AddressField.PostalAddress,
                    notifier.PostalAddress,
                    cellValues);

                CreateValueIfNeeded(settings.PostalCode, AddressField.PostalCode, notifier.PostalCode, cellValues);
                CreateValueIfNeeded(settings.City, AddressField.City, notifier.City, cellValues);
                CreateValueIfNeeded(settings.Title, OrganizationField.Title, notifier.Title, cellValues);
                CreateValueIfNeeded(settings.Department, OrganizationField.Department, notifier.Department, cellValues);
                CreateValueIfNeeded(settings.Unit, OrganizationField.Unit, notifier.Unit, cellValues);

                CreateValueIfNeeded(
                    settings.OrganizationUnit,
                    OrganizationField.OrganizationUnit,
                    notifier.OrganizationUnit,
                    cellValues);

                CreateValueIfNeeded(settings.Division, OrganizationField.Division, notifier.Division, cellValues);
                CreateValueIfNeeded(settings.Manager, OrganizationField.Manager, notifier.Manager, cellValues);
                CreateValueIfNeeded(settings.Group, OrganizationField.Group, notifier.Group, cellValues);
                CreateValueIfNeeded(settings.Other, OrganizationField.Other, notifier.Other, cellValues);
                CreateValueIfNeeded(settings.Ordered, OrdererField.Orderer, notifier.Ordered.ToString(), cellValues);

                CreateValueIfNeeded(
                    settings.CreatedDate,
                    StateField.CreatedDate,
                    notifier.CreatedDate.ToString(CultureInfo.InvariantCulture),
                    cellValues);

                CreateValueIfNeeded(
                    settings.ChangedDate,
                    StateField.ChangedDate,
                    notifier.ChangedDate.ToFormattedDateTime(),
                    cellValues);

                CreateValueIfNeeded(
                    settings.SynchronizationDate,
                    StateField.SynchronizationDate,
                    notifier.SynchronizationDate.HasValue 
                        ? notifier.SynchronizationDate.Value.ToString(CultureInfo.InvariantCulture)
                        : null,
                    cellValues);

                var notifierRow = new NotifierDetailedOverviewModel(notifier.Id, cellValues);
                notifierRows.Add(notifierRow);
            }

            return new NotifiersGridModel(searchResult.NotifiersFound, headers, notifierRows, sortField);
        }

        private static void CreateHeaderIfNeeded(
            FieldSetting setting,
            string fieldName,
            List<GridColumnHeaderModel> headers)
        {
            if (setting.ShowInNotifiers)
            {
                var header = new GridColumnHeaderModel(fieldName, setting.Caption);
                headers.Add(header);
            }
        }

        private static void CreateValueIfNeeded(
            FieldSetting setting,
            string fieldName,
            string value,
            List<GridRowCellValueModel> values)
        {
            if (setting.ShowInNotifiers)
            {
                values.Add(new GridRowCellValueModel(fieldName, value));
            }
        }
    }
}