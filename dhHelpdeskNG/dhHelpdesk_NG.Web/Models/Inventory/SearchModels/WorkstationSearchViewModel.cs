namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.Response.Inventory;

    public class WorkstationSearchViewModel
    {
        private WorkstationSearchViewModel(
            SelectList regions,
            SelectList computerTypes,
            SelectList contractStatuses,
            WorkstationsSearchFilter filter,
            ComputerFieldsSettingsOverviewForFilter settings)
        {
            this.Regions = regions;
            this.ComputerTypes = computerTypes;
            this.ContractStatuses = contractStatuses;
            this.Filter = filter;
            this.Settings = settings;
        }

        [NotNull]
        public SelectList Regions { get; private set; }

        [NotNull]
        public SelectList ComputerTypes { get; private set; }

        [NotNull]
        public SelectList ContractStatuses { get; private set; }

        [NotNull]
        public WorkstationsSearchFilter Filter { get; private set; }

        public ComputerFieldsSettingsOverviewForFilter Settings { get; private set; }

        public static WorkstationSearchViewModel BuildViewModel(
            WorkstationsSearchFilter currentFilter,
            ComputerFiltersResponse additionalData,
            ComputerFieldsSettingsOverviewForFilter settings)
        {
            var regions = new SelectList(additionalData.Regions, "Value", "Name");
            var computerTypes = new SelectList(additionalData.ComputerTypes, "Value", "Name");
            var contractStatuses = new SelectList(Enum.GetValues(typeof(ContractStatuses)));

            var viewModel = new WorkstationSearchViewModel(
                regions,
                computerTypes,
                contractStatuses,
                currentFilter,
                settings);

            return viewModel;
        }
    }
}