namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public sealed class NotifierModelFactory : INotifierModelFactory
    {
        private readonly INotifierInputFieldModelFactory notifierInputFieldModelFactory;

        public NotifierModelFactory(INotifierInputFieldModelFactory notifierInputFieldModelFactory)
        {
            this.notifierInputFieldModelFactory = notifierInputFieldModelFactory;
        }

        public NotifierModel Create(
            DisplayFieldSettingsDto displaySettings,
            NotifierDetailsDto notifier,
            List<ItemOverviewDto> domains,
            List<ItemOverviewDto> regions,
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> organizationUnits,
            List<ItemOverviewDto> divisions,
            List<ItemOverviewDto> managers,
            List<ItemOverviewDto> groups)
        {
            var userId = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.UserId, notifier.UserId);

            NotifierInputDropDownModel domain;

            if (domains != null)
            {
                var domainItems = domains.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                var domainValue = notifier.DomainId.HasValue
                                      ? notifier.DomainId.Value.ToString(CultureInfo.InvariantCulture)
                                      : null;

                domain = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Domain, domainItems, domainValue);
            }
            else
            {
                domain = new NotifierInputDropDownModel(false);
            }

            var loginName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.LoginName, notifier.LoginName);

            var firstName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.FirstName, notifier.FirstName);

            var initials = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Initials, notifier.Initials);

            var lastName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.LastName, notifier.LastName);

            var displayName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.DisplayName, notifier.DisplayName);

            var place = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Place, notifier.Place);

            var phone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Phone, notifier.Phone);

            var cellPhone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.CellPhone, notifier.CellPhone);

            var email = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Email, notifier.Email);

            var code = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Code, notifier.Code);

            var postalAddress =
                this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                    displaySettings.PostalAddress, notifier.PostalAddress);

            var postalCode = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.PostalCode, notifier.PostalCode);

            var city = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.City, notifier.City);

            var title = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Title, notifier.Title);

            DropDownContent regionContent;
            NotifierInputDropDownModel department;

            if (departments != null)
            {
                var regionItems = regions.Select(r => new DropDownItem(r.Name, r.Value)).ToList();
                regionContent = new DropDownContent(regionItems);

                var departmentItems =
                    departments.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                var departmentValue = notifier.DepartmentId.HasValue
                                          ? notifier.DepartmentId.Value.ToString(CultureInfo.InvariantCulture)
                                          : null;

                department = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Department, departmentItems, departmentValue);
            }
            else
            {
                regionContent = null;
                department = new NotifierInputDropDownModel(false);
            }

            var unit = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Unit, notifier.Unit);

            NotifierInputDropDownModel organizationUnit;

            if (organizationUnits != null)
            {
                var organizationUnitItems =
                    organizationUnits.Select(u => new KeyValuePair<string, string>(u.Value, u.Name)).ToList();

                var organizationUnitValue = notifier.OrganizationUnitId.HasValue
                                                ? notifier.OrganizationUnitId.Value.ToString(
                                                    CultureInfo.InvariantCulture)
                                                : null;

                organizationUnit =
                    this.notifierInputFieldModelFactory.CreateDropDownModel(
                        displaySettings.OrganizationUnit, organizationUnitItems, organizationUnitValue);
            }
            else
            {
                organizationUnit = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel division;

            if (divisions != null)
            {
                var divisionItems = divisions.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                var divisionValue = notifier.DivisionId.HasValue
                                        ? notifier.DivisionId.Value.ToString(CultureInfo.InvariantCulture)
                                        : null;

                division = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Division, divisionItems, divisionValue);
            }
            else
            {
                division = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel manager;

            if (managers != null)
            {
                var managerItems = managers.Select(m => new KeyValuePair<string, string>(m.Value, m.Name)).ToList();

                var managerValue = notifier.ManagerId.HasValue
                                       ? notifier.ManagerId.Value.ToString(CultureInfo.InvariantCulture)
                                       : null;

                manager = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Manager, managerItems, managerValue);
            }
            else
            {
                manager = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel group;

            if (groups != null)
            {
                var groupItems = groups.Select(g => new KeyValuePair<string, string>(g.Value, g.Name)).ToList();

                var groupValue = notifier.GroupId.HasValue
                                     ? notifier.GroupId.Value.ToString(CultureInfo.InvariantCulture)
                                     : null;

                group = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Group, groupItems, groupValue);
            }
            else
            {
                group = new NotifierInputDropDownModel(false);
            }

            var password = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Password, notifier.Password);

            var other = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.Other, notifier.Other);

            var ordered = this.notifierInputFieldModelFactory.CreateInputCheckBoxModel(
                displaySettings.Ordered, notifier.Ordered);

            var createdDate = this.notifierInputFieldModelFactory.CreateLabelModel(
                displaySettings.CreatedDate, notifier.CreatedDate.ToString(CultureInfo.InvariantCulture));

            var changedDate = this.notifierInputFieldModelFactory.CreateLabelModel(
                displaySettings.ChangedDate, notifier.ChangedDate.ToString(CultureInfo.InvariantCulture));

            string synchronizationDateValue;

            if (notifier.SynchronizationDate.HasValue)
            {
                synchronizationDateValue = notifier.SynchronizationDate.Value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                synchronizationDateValue = string.Empty;
            }

            var synchronizationDate =
                this.notifierInputFieldModelFactory.CreateLabelModel(
                    displaySettings.SynchronizationDate, synchronizationDateValue);

            var inputModel = new NotifierInputModel(
                false,
                userId,
                domain,
                loginName,
                firstName,
                initials,
                lastName,
                displayName,
                place,
                phone,
                cellPhone,
                email,
                code,
                postalAddress,
                postalCode,
                city,
                title,
                regionContent,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                password,
                other,
                ordered,
                notifier.IsActive,
                createdDate,
                changedDate,
                synchronizationDate);

            return new NotifierModel(notifier.Id, inputModel);
        }
    }
}