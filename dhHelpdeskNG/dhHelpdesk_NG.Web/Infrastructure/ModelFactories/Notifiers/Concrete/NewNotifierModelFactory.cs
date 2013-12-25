namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class NewNotifierModelFactory : INewNotifierModelFactory
    {
        private readonly INotifierInputFieldModelFactory notifierInputFieldModelFactory;

        public NewNotifierModelFactory(INotifierInputFieldModelFactory notifierInputFieldModelFactory)
        {
            this.notifierInputFieldModelFactory = notifierInputFieldModelFactory;
        }

        #region Public Methods and Operators

        public NotifierInputModel Create(
            DisplayFieldSettingsDto displaySettings, 
            List<ItemOverviewDto> domains, 
            List<ItemOverviewDto> regions,
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> organizationUnits,
            List<ItemOverviewDto> divisions, 
            List<ItemOverviewDto> managers, 
            List<ItemOverviewDto> groups)
        {
            var userId = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.UserId, null);

            NotifierInputDropDownModel domain;

            if (displaySettings.Domain.Show)
            {
                var domainItems = domains.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                domain = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Domain, domainItems, null);
            }
            else
            {
                domain = new NotifierInputDropDownModel(false);
            }

            var loginName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.LoginName, null);
            var firstName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.FirstName, null);
            var initials = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Initials, null);
            var lastName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.LastName, null);
            
            var displayName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.DisplayName, null);

            var place = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Place, null);
            var phone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Phone, null);
            var cellPhone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.CellPhone, null);
            var email = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Email, null);
            var code = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Code, null);
            
            var postalAddress =
                this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.PostalAddress, null);

            var postalCode = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                displaySettings.PostalCode, null);

            var city = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.City, null);
            var title = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Title, null);

            DropDownContent regionDropDownContent;
            NotifierInputDropDownModel department;

            if (displaySettings.Department.Show)
            {
                var regionItems = regions.Select(r => new DropDownItem(r.Name, r.Value)).ToList();
                regionDropDownContent = new DropDownContent(regionItems);

                var departmentItems =
                    departments.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                department = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Department, departmentItems, null);
            }
            else
            {
                regionDropDownContent = null;
                department = new NotifierInputDropDownModel(false);
            }

            var unit = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Unit, null);
            
            NotifierInputDropDownModel organizationUnit;

            if (displaySettings.OrganizationUnit.Show)
            {
                var organizationUnitItems =
                    organizationUnits.Select(u => new KeyValuePair<string, string>(u.Value, u.Name)).ToList();

                organizationUnit =
                    this.notifierInputFieldModelFactory.CreateDropDownModel(
                        displaySettings.OrganizationUnit, organizationUnitItems, null);
            }
            else
            {
                organizationUnit = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel division;

            if (displaySettings.Division.Show)
            {
                var divisionItems = divisions.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                division = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Division, divisionItems, null);
            }
            else
            {
                division = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel manager;

            if (displaySettings.Manager.Show)
            {
                var managerItems = managers.Select(m => new KeyValuePair<string, string>(m.Value, m.Name)).ToList();

                manager = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    displaySettings.Manager, managerItems, null);
            }
            else
            {
                manager = new NotifierInputDropDownModel(false);
            }

            NotifierInputDropDownModel group;

            if (displaySettings.Group.Show)
            {
                var groupItems = groups.Select(g => new KeyValuePair<string, string>(g.Value, g.Name)).ToList();
                group = this.notifierInputFieldModelFactory.CreateDropDownModel(displaySettings.Group, groupItems, null);
            }
            else
            {
                group = new NotifierInputDropDownModel(false);
            }
        
            var password = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Password, null);
            var other = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(displaySettings.Other, null);
            var ordered = this.notifierInputFieldModelFactory.CreateInputCheckBoxModel(displaySettings.Ordered, false);

            return new NotifierInputModel(
                true,
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
                regionDropDownContent,
                department,
                unit,
                organizationUnit,
                division,
                manager,
                group,
                password,
                other,
                ordered,
                true,
                null,
                null,
                null);
        }

        #endregion
    }
}