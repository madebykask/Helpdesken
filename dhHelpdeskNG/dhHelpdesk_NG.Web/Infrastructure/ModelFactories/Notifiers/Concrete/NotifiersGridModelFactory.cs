namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Models.Common;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public sealed class NotifiersGridModelFactory : INotifiersGridModelFactory
    {
        public NotifiersGridModel Create(SearchResult searchResult, FieldSettings settings, SortFieldModel sortField)
        {
            var notifierFieldModels = new List<GridColumnHeaderModel>();

            if (settings.UserId.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.UserId.Name, settings.UserId.Caption));
            }

            if (settings.Domain.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Domain.Name, settings.Domain.Caption));
            }

            if (settings.LoginName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.LoginName.Name, settings.LoginName.Caption));
            }

            if (settings.FirstName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.FirstName.Name, settings.FirstName.Caption));
            }

            if (settings.Initials.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Initials.Name, settings.Initials.Caption));
            }

            if (settings.LastName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.LastName.Name, settings.LastName.Caption));
            }

            if (settings.DisplayName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.DisplayName.Name, settings.DisplayName.Caption));
            }

            if (settings.Place.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Place.Name, settings.Place.Caption));
            }

            if (settings.Phone.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Phone.Name, settings.Phone.Caption));
            }

            if (settings.CellPhone.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.CellPhone.Name, settings.CellPhone.Caption));
            }

            if (settings.Email.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Email.Name, settings.Email.Caption));
            }

            if (settings.Code.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Code.Name, settings.Code.Caption));
            }

            if (settings.PostalAddress.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.PostalAddress.Name, settings.PostalAddress.Caption));
            }

            if (settings.PostalCode.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.PostalCode.Name, settings.PostalCode.Caption));
            }

            if (settings.City.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.City.Name, settings.City.Caption));
            }

            if (settings.Title.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Title.Name, settings.Title.Caption));
            }

            if (settings.Department.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Department.Name, settings.Department.Caption));
            }

            if (settings.Unit.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Unit.Name, settings.Unit.Caption));
            }

            if (settings.OrganizationUnit.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.OrganizationUnit.Name, settings.OrganizationUnit.Caption));
            }

            if (settings.Division.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Division.Name, settings.Division.Caption));
            }

            if (settings.Manager.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Manager.Name, settings.Manager.Caption));
            }

            if (settings.Group.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Group.Name, settings.Group.Caption));
            }

            if (settings.Other.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Other.Name, settings.Other.Caption));
            }

            if (settings.Ordered.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.Ordered.Name, settings.Ordered.Caption));
            }

            if (settings.CreatedDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.CreatedDate.Name, settings.CreatedDate.Caption));
            }

            if (settings.ChangedDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.ChangedDate.Name, settings.ChangedDate.Caption));
            }

            if (settings.SynchronizationDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new GridColumnHeaderModel(settings.SynchronizationDate.Name, settings.SynchronizationDate.Caption));
            }

            var notifierModels = new List<NotifierDetailedOverviewModel>(searchResult.Notifiers.Count);

            foreach (var notifier in searchResult.Notifiers)
            {
                var notifierFieldValueModels = new List<GridRowCellValueModel>();
               
                if (settings.UserId.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.UserId.Name, notifier.UserId));
                }

                if (settings.Domain.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.Domain.Name, notifier.Domain));
                }

                if (settings.LoginName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.LoginName.Name, notifier.LoginName));
                }

                if (settings.FirstName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.FirstName.Name, notifier.FirstName));
                }

                if (settings.Initials.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.Initials.Name, notifier.Initials));
                }

                if (settings.LastName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.LastName.Name, notifier.LastName));
                }

                if (settings.DisplayName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.DisplayName.Name, notifier.DisplayName));
                }

                if (settings.Place.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.Place.Name, notifier.Place));
                }

                if (settings.Phone.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.Phone.Name, notifier.Phone));
                }

                if (settings.CellPhone.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.CellPhone.Name, notifier.CellPhone));
                }

                if (settings.Email.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.Email.Name, notifier.Email));
                }

                if (settings.Code.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.Code.Name, notifier.Code));
                }

                if (settings.PostalAddress.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.PostalAddress.Name, notifier.PostalAddress));
                }

                if (settings.PostalCode.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.PostalCode.Name, notifier.PostalCode));
                }

                if (settings.City.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.City.Name, notifier.City));
                }

                if (settings.Title.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.Title.Name, notifier.Title));
                }

                if (settings.Department.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.Department.Name, notifier.Department));
                }

                if (settings.Unit.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.Unit.Name, notifier.Unit));
                }

                if (settings.OrganizationUnit.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.OrganizationUnit.Name, notifier.OrganizationUnit));
                }

                if (settings.Division.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.Division.Name, notifier.Division));
                }

                if (settings.Manager.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.Manager.Name, notifier.Manager));
                }

                if (settings.Group.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.Group.Name, notifier.Group));
                }

                if (settings.Other.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new GridRowCellValueModel(settings.Other.Name, notifier.Other));
                }

                if (settings.Ordered.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.Ordered.Name, notifier.Ordered.ToString()));
                }

                if (settings.CreatedDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(
                            settings.CreatedDate.Name, notifier.CreatedDate.ToString(CultureInfo.InvariantCulture)));
                }

                if (settings.ChangedDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(settings.ChangedDate.Name, notifier.ChangedDate.ToString(CultureInfo.InvariantCulture)));
                }

                if (settings.SynchronizationDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new GridRowCellValueModel(
                            settings.SynchronizationDate.Name,
                            notifier.SynchronizationDate.HasValue
                                ? notifier.SynchronizationDate.Value.ToString(CultureInfo.InvariantCulture)
                                : null));
                }

                var notifierModel = new NotifierDetailedOverviewModel(notifier.Id, notifierFieldValueModels);
                notifierModels.Add(notifierModel);
            }

            return new NotifiersGridModel(searchResult.NotifiersFound, notifierFieldModels, notifierModels, sortField);
        }
    }
}