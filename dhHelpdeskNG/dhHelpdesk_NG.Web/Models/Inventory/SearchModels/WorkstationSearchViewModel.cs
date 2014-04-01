namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Requests.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Common;

    public class WorkstationSearchViewModel
    {
        public WorkstationSearchViewModel(
            ConfigurableSearchFieldModel<List<SelectListItem>> regions,
            ConfigurableSearchFieldModel<List<SelectListItem>> departments,
            ConfigurableSearchFieldModel<List<SelectListItem>> computerTypes,
            ConfigurableSearchFieldModel<List<SelectListItem>> contractStatuses,
            WorkstationsSearchFilter filter)
        {
            this.Regions = regions;
            this.Departments = departments;
            this.ComputerTypes = computerTypes;
            this.ContractStatuses = contractStatuses;
            this.Filter = filter;
        }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> Regions { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> Departments { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> ComputerTypes { get; private set; }

        [NotNull]
        public ConfigurableSearchFieldModel<List<SelectListItem>> ContractStatuses { get; private set; }

        [NotNull]
        public WorkstationsSearchFilter Filter { get; private set; }

        public static WorkstationSearchViewModel BuildViewModel(
            ComputersFilter currentFilter,
            ComputerFiltersRequest additionalData,
            ComputerFieldsSettingsOverview settings)
        {
            throw new NotImplementedException();
        }
    }
}