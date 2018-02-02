namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Models.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.ConfigurableFields;

    public sealed class NewNotifierModelFactory : INewNotifierModelFactory
    {
        private readonly INotifierInputFieldModelFactory notifierInputFieldModelFactory;

        public NewNotifierModelFactory(INotifierInputFieldModelFactory notifierInputFieldModelFactory)
        {
            this.notifierInputFieldModelFactory = notifierInputFieldModelFactory;
        }

        #region Public Methods and Operators

        public InputModel Create(
            NotifierOverviewSettings settings,
            List<ItemOverview> domains,
            List<ItemOverview> regions,
            List<ItemOverview> departments,
            List<ItemOverview> organizationUnits,
            List<ItemOverview> divisions,
            List<ItemOverview> managers,
            List<ItemOverview> groups,
            Dictionary<string, string> inputParams,
            List<ItemOverview> languages,
			ComputerUserCategoryModel model)
        {
            var userId = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.UserId, (inputParams.ContainsKey("UserId") ? inputParams["UserId"] : null));

            DropDownFieldModel domain;

            if (settings.Domain.Show)
            {
                var domainItems = domains.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                domain = this.notifierInputFieldModelFactory.CreateDropDownModel(settings.Domain, domainItems, null);
            }
            else
            {
                domain = new DropDownFieldModel(false);
            }


            var loginName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.LoginName, null);
            var firstName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.FirstName, (inputParams.ContainsKey("FirstName") ? inputParams["FirstName"] : null));
            var initials = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Initials, null, 10);
            var lastName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.LastName, (inputParams.ContainsKey("LastName") ? inputParams["LastName"] : null));

            var displayName = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.DisplayName, (inputParams.ContainsKey("FName") ? inputParams["FName"] : null));

            var place = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Place, (inputParams.ContainsKey("Placement") ? inputParams["Placement"] : null), 100);
            var phone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Phone, (inputParams.ContainsKey("Phone") ? inputParams["Phone"] : null));
            var cellPhone = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.CellPhone, (inputParams.ContainsKey("CellPhone") ? inputParams["CellPhone"] : null));
            var email = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Email, (inputParams.ContainsKey("Email") ? inputParams["Email"] : null), 100);            
            var code = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Code, null);

            var postalAddress = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(
                settings.PostalAddress,
                null);

            var postalCode = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.PostalCode, null);

            var city = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.City, null);
            var title = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Title, null);

            DropDownFieldModel region;
            DropDownFieldModel department;
            DropDownFieldModel organizationUnit;
            DropDownFieldModel language;

            if (settings.Region.Show)
            {
                var regionItems = regions.Select(r => new KeyValuePair<string, string>(r.Value, r.Name)).ToList();

                region = this.notifierInputFieldModelFactory.CreateDropDownModel(settings.Region, regionItems, (inputParams.ContainsKey("RegionId") ? inputParams["RegionId"] : null));
            }
            else
            {
                region = new DropDownFieldModel(false);
            }


            if (settings.Language.Show)
            {
                var languageItems =
                    languages.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                language = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.Language,
                    languageItems,
                    (inputParams.ContainsKey("LanguageId") ? inputParams["LanguageId"] : null));
            }
            else
            {
                language = new DropDownFieldModel(false);
            }

            if (settings.Department.Show)
            {
                var departmentItems =
                    departments.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                department = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.Department,
                    departmentItems,
                    (inputParams.ContainsKey("DepartmentId") ? inputParams["DepartmentId"] : null));
            }
            else
            {
                department = new DropDownFieldModel(false);
            }

            if (settings.OrganizationUnit.Show)
            {
                var organizationUnitItems =
                    organizationUnits.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                organizationUnit = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.OrganizationUnit,
                    organizationUnitItems,
                    (inputParams.ContainsKey("OrganizationUnitId") ? inputParams["OrganizationUnitId"] : null));
            }
            else
            {
                organizationUnit = new DropDownFieldModel(false);
            }

            var costCentre = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.CostCentre, (inputParams.ContainsKey("CostCentre") ? inputParams["CostCentre"] : null));
            var unit = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Unit, null);

            DropDownFieldModel division;

            if (settings.Division.Show)
            {
                var divisionItems = divisions.Select(d => new KeyValuePair<string, string>(d.Value, d.Name)).ToList();

                division = this.notifierInputFieldModelFactory.CreateDropDownModel(
                    settings.Division,
                    divisionItems,
                    null);
            }
            else
            {
                division = new DropDownFieldModel(false);
            }

            DropDownFieldModel manager;

            if (settings.Manager.Show)
            {
                var managerItems = managers.Select(m => new KeyValuePair<string, string>(m.Value, m.Name)).ToList();

                manager = this.notifierInputFieldModelFactory.CreateDropDownModel(settings.Manager, managerItems, null);
            }
            else
            {
                manager = new DropDownFieldModel(false);
            }

            DropDownFieldModel group;

            if (settings.Group.Show)
            {
                var groupItems = groups.Select(g => new KeyValuePair<string, string>(g.Value, g.Name)).ToList();
                group = this.notifierInputFieldModelFactory.CreateDropDownModel(settings.Group, groupItems, null);
            }
            else
            {
                group = new DropDownFieldModel(false);
            }

            var other = this.notifierInputFieldModelFactory.CreateInputTextBoxModel(settings.Other, null, 500);
            var ordered = this.notifierInputFieldModelFactory.CreateInputCheckBoxModel(settings.Ordered, false);

            return new InputModel(
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
                region,
                department,
                organizationUnit,
                costCentre,
                unit,
                division,
                manager,
                group,
                other,
                ordered,
                true,
                language,
				model);
        }

        #endregion
    }
}