namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class NotifiersGridModelFactory : INotifiersGridModelFactory
    {
        public NotifiersGridModel Create(SearchResultDto searchResult, FieldSettingsDto displaySettings)
        {
            var notifierFieldModels = new List<NotifierFieldModel>();

            if (displaySettings.UserId.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.UserId.Name, displaySettings.UserId.Caption));
            }

            if (displaySettings.Domain.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Domain.Name, displaySettings.Domain.Caption));
            }

            if (displaySettings.LoginName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.LoginName.Name, displaySettings.LoginName.Caption));
            }

            if (displaySettings.FirstName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.FirstName.Name, displaySettings.FirstName.Caption));
            }

            if (displaySettings.Initials.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Initials.Name, displaySettings.Initials.Caption));
            }

            if (displaySettings.LastName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.LastName.Name, displaySettings.LastName.Caption));
            }

            if (displaySettings.DisplayName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.DisplayName.Name, displaySettings.DisplayName.Caption));
            }

            if (displaySettings.Place.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Place.Name, displaySettings.Place.Caption));
            }

            if (displaySettings.Phone.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Phone.Name, displaySettings.Phone.Caption));
            }

            if (displaySettings.CellPhone.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.CellPhone.Name, displaySettings.CellPhone.Caption));
            }

            if (displaySettings.Email.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Email.Name, displaySettings.Email.Caption));
            }

            if (displaySettings.Code.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Code.Name, displaySettings.Code.Caption));
            }

            if (displaySettings.PostalAddress.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.PostalAddress.Name, displaySettings.PostalAddress.Caption));
            }

            if (displaySettings.PostalCode.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.PostalCode.Name, displaySettings.PostalCode.Caption));
            }

            if (displaySettings.City.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.City.Name, displaySettings.City.Caption));
            }

            if (displaySettings.Title.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Title.Name, displaySettings.Title.Caption));
            }

            if (displaySettings.Department.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Department.Name, displaySettings.Department.Caption));
            }

            if (displaySettings.Unit.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Unit.Name, displaySettings.Unit.Caption));
            }

            if (displaySettings.OrganizationUnit.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.OrganizationUnit.Name, displaySettings.OrganizationUnit.Caption));
            }

            if (displaySettings.Division.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Division.Name, displaySettings.Division.Caption));
            }

            if (displaySettings.Manager.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Manager.Name, displaySettings.Manager.Caption));
            }

            if (displaySettings.Group.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Group.Name, displaySettings.Group.Caption));
            }

            if (displaySettings.Password.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Password.Name, displaySettings.Password.Caption));
            }

            if (displaySettings.Other.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Other.Name, displaySettings.Other.Caption));
            }

            if (displaySettings.Ordered.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.Ordered.Name, displaySettings.Ordered.Caption));
            }

            if (displaySettings.CreatedDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.CreatedDate.Name, displaySettings.CreatedDate.Caption));
            }

            if (displaySettings.ChangedDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.ChangedDate.Name, displaySettings.ChangedDate.Caption));
            }

            if (displaySettings.SynchronizationDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(displaySettings.SynchronizationDate.Name, displaySettings.SynchronizationDate.Caption));
            }

            var notifierModels = new List<NotifierDetailedOverviewModel>(searchResult.Notifiers.Count);

            foreach (var notifier in searchResult.Notifiers)
            {
                var notifierFieldValueModels = new List<NotifierFieldValueModel>();
               
                if (displaySettings.UserId.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.UserId.Name, notifier.UserId));
                }

                if (displaySettings.Domain.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.Domain.Name, notifier.Domain));
                }

                if (displaySettings.LoginName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.LoginName.Name, notifier.LoginName));
                }

                if (displaySettings.FirstName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.FirstName.Name, notifier.FirstName));
                }

                if (displaySettings.Initials.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.Initials.Name, notifier.Initials));
                }

                if (displaySettings.LastName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.LastName.Name, notifier.LastName));
                }

                if (displaySettings.DisplayName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.DisplayName.Name, notifier.DisplayName));
                }

                if (displaySettings.Place.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.Place.Name, notifier.Place));
                }

                if (displaySettings.Phone.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.Phone.Name, notifier.Phone));
                }

                if (displaySettings.CellPhone.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.CellPhone.Name, notifier.CellPhone));
                }

                if (displaySettings.Email.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.Email.Name, notifier.Email));
                }

                if (displaySettings.Code.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.Code.Name, notifier.Code));
                }

                if (displaySettings.PostalAddress.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.PostalAddress.Name, notifier.PostalAddress));
                }

                if (displaySettings.PostalCode.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.PostalCode.Name, notifier.PostalCode));
                }

                if (displaySettings.City.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.City.Name, notifier.City));
                }

                if (displaySettings.Title.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.Title.Name, notifier.Title));
                }

                if (displaySettings.Department.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.Department.Name, notifier.Department));
                }

                if (displaySettings.Unit.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.Unit.Name, notifier.Unit));
                }

                if (displaySettings.OrganizationUnit.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.OrganizationUnit.Name, notifier.OrganizationUnit));
                }

                if (displaySettings.Division.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.Division.Name, notifier.Division));
                }

                if (displaySettings.Manager.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.Manager.Name, notifier.Manager));
                }

                if (displaySettings.Group.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.Group.Name, notifier.Group));
                }

                if (displaySettings.Password.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.Password.Name, notifier.Password));
                }

                if (displaySettings.Other.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(displaySettings.Other.Name, notifier.Other));
                }

                if (displaySettings.Ordered.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.Ordered.Name, notifier.Ordered.ToString()));
                }

                if (displaySettings.CreatedDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(
                            displaySettings.CreatedDate.Name, notifier.CreatedDate.ToString(CultureInfo.InvariantCulture)));
                }

                if (displaySettings.ChangedDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(displaySettings.ChangedDate.Name, notifier.ChangedDate.ToString(CultureInfo.InvariantCulture)));
                }

                if (displaySettings.SynchronizationDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(
                            displaySettings.SynchronizationDate.Name,
                            notifier.SynchronizationDate.HasValue
                                ? notifier.SynchronizationDate.Value.ToString(CultureInfo.InvariantCulture)
                                : null));
                }

                var notifierModel = new NotifierDetailedOverviewModel(notifier.Id, notifierFieldValueModels);
                notifierModels.Add(notifierModel);
            }

            return new NotifiersGridModel(searchResult.NotifiersFound, notifierFieldModels, notifierModels);
        }
    }
}