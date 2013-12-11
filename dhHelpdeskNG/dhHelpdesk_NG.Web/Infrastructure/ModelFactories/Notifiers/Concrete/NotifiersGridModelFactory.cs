namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class NotifiersGridModelFactory : INotifiersGridModelFactory
    {
        public NotifiersGridModel Create(List<NotifierDetailedOverviewDto> notifiers, FieldsSettingsDto fieldsSettings)
        {
            var notifierFieldModels = new List<NotifierFieldModel>();

            if (fieldsSettings.UserId.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.UserId.Name));
            }

            if (fieldsSettings.Domain.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Domain.Name));
            }

            if (fieldsSettings.LoginName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.LoginName.Name));
            }

            if (fieldsSettings.FirstName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.FirstName.Name));
            }

            if (fieldsSettings.Initials.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Initials.Name));
            }

            if (fieldsSettings.LastName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.LastName.Name));
            }

            if (fieldsSettings.DisplayName.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.DisplayName.Name));
            }

            if (fieldsSettings.Place.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Place.Name));
            }

            if (fieldsSettings.Phone.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Phone.Name));
            }

            if (fieldsSettings.CellPhone.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.CellPhone.Name));
            }

            if (fieldsSettings.Email.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Email.Name));
            }

            if (fieldsSettings.Code.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Code.Name));
            }

            if (fieldsSettings.PostalAddress.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.PostalAddress.Name));
            }

            if (fieldsSettings.PostalCode.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.PostalCode.Name));
            }

            if (fieldsSettings.City.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.City.Name));
            }

            if (fieldsSettings.Title.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Title.Name));
            }

            if (fieldsSettings.Department.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Department.Name));
            }

            if (fieldsSettings.Unit.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Unit.Name));
            }

            if (fieldsSettings.OrganizationUnit.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.OrganizationUnit.Name));
            }

            if (fieldsSettings.Division.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Division.Name));
            }

            if (fieldsSettings.Manager.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Manager.Name));
            }

            if (fieldsSettings.Group.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Group.Name));
            }

            if (fieldsSettings.Password.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Password.Name));
            }

            if (fieldsSettings.Other.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Other.Name));
            }

            if (fieldsSettings.Ordered.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.Ordered.Name));
            }

            if (fieldsSettings.CreatedDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.CreatedDate.Name));
            }

            if (fieldsSettings.ChangedDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.ChangedDate.Name));
            }

            if (fieldsSettings.SynchronizationDate.ShowInNotifiers)
            {
                notifierFieldModels.Add(new NotifierFieldModel(fieldsSettings.SynchronizationDate.Name));
            }

            var notifierModels = new List<NotifierDetailedOverviewModel>(notifiers.Count);

            foreach (var notifier in notifiers)
            {
                var notifierFieldValueModels = new List<NotifierFieldValueModel>();
               
                if (fieldsSettings.UserId.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.UserId.Name, notifier.UserId));
                }

                if (fieldsSettings.Domain.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.Domain.Name, notifier.Domain));
                }

                if (fieldsSettings.LoginName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.LoginName.Name, notifier.LoginName));
                }

                if (fieldsSettings.FirstName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.FirstName.Name, notifier.FirstName));
                }

                if (fieldsSettings.Initials.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.Initials.Name, notifier.Initials));
                }

                if (fieldsSettings.LastName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.LastName.Name, notifier.LastName));
                }

                if (fieldsSettings.DisplayName.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.DisplayName.Name, notifier.DisplayName));
                }

                if (fieldsSettings.Place.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.Place.Name, notifier.Place));
                }

                if (fieldsSettings.Phone.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.Phone.Name, notifier.Phone));
                }

                if (fieldsSettings.CellPhone.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.CellPhone.Name, notifier.CellPhone));
                }

                if (fieldsSettings.Email.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.Email.Name, notifier.Email));
                }

                if (fieldsSettings.Code.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.Code.Name, notifier.Code));
                }

                if (fieldsSettings.PostalAddress.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.PostalAddress.Name, notifier.PostalAddress));
                }

                if (fieldsSettings.PostalCode.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.PostalCode.Name, notifier.PostalCode));
                }

                if (fieldsSettings.City.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.City.Name, notifier.City));
                }

                if (fieldsSettings.Title.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.Title.Name, notifier.Title));
                }

                if (fieldsSettings.Department.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.Department.Name, notifier.Department));
                }

                if (fieldsSettings.Unit.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.Unit.Name, notifier.Unit));
                }

                if (fieldsSettings.OrganizationUnit.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.OrganizationUnit.Name, notifier.OrganizationUnit));
                }

                if (fieldsSettings.Division.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.Division.Name, notifier.Division));
                }

                if (fieldsSettings.Manager.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.Manager.Name, notifier.Manager));
                }

                if (fieldsSettings.Group.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.Group.Name, notifier.Group));
                }

                if (fieldsSettings.Password.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.Password.Name, notifier.Password));
                }

                if (fieldsSettings.Other.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(new NotifierFieldValueModel(fieldsSettings.Other.Name, notifier.Other));
                }

                if (fieldsSettings.Ordered.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.Ordered.Name, notifier.Ordered.ToString()));
                }

                if (fieldsSettings.CreatedDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(
                            fieldsSettings.CreatedDate.Name, notifier.CreatedDate.ToString(CultureInfo.InvariantCulture)));
                }

                if (fieldsSettings.ChangedDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(fieldsSettings.ChangedDate.Name, notifier.ChangedDate.ToString(CultureInfo.InvariantCulture)));
                }

                if (fieldsSettings.SynchronizationDate.ShowInNotifiers)
                {
                    notifierFieldValueModels.Add(
                        new NotifierFieldValueModel(
                            fieldsSettings.SynchronizationDate.Name,
                            notifier.SynchronizationDate.HasValue
                                ? notifier.SynchronizationDate.Value.ToString(CultureInfo.InvariantCulture)
                                : null));
                }

                var notifierModel = new NotifierDetailedOverviewModel(notifier.Id, notifierFieldValueModels);
                notifierModels.Add(notifierModel);
            }

            return new NotifiersGridModel(notifierFieldModels, notifierModels);
        }
    }
}