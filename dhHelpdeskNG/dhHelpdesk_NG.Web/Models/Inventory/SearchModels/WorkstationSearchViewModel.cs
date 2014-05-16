namespace DH.Helpdesk.Web.Models.Inventory.SearchModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationSearchViewModel
    {
        private WorkstationSearchViewModel(
            SelectList regions,
            SelectList departments,
            SelectList computerTypes,
            SelectList contractStatuses,
            WorkstationsSearchFilter filter,
            ComputerFieldsSettingsOverviewForFilter settings)
        {
            this.Departments = departments;
            this.Regions = regions;
            this.ComputerTypes = computerTypes;
            this.ContractStatuses = contractStatuses;
            this.Filter = filter;
            this.Settings = settings;
        }

        [NotNull]
        public SelectList Regions { get; private set; }

        [NotNull]
        public SelectList Departments { get; private set; }

        [NotNull]
        public SelectList ComputerTypes { get; private set; }

        [NotNull]
        public SelectList ContractStatuses { get; private set; }

        [NotNull]
        public WorkstationsSearchFilter Filter { get; private set; }

        public ComputerFieldsSettingsOverviewForFilter Settings { get; private set; }

        public static WorkstationSearchViewModel BuildViewModel(
            WorkstationsSearchFilter currentFilter,
            List<ItemOverview> regionsItemOverviews,
            List<ItemOverview> departmentsItemOverviews, 
            List<ItemOverview> computerTypesItemOverviews,
            ComputerFieldsSettingsOverviewForFilter settings)
        {
            var regions = new SelectList(regionsItemOverviews, "Value", "Name");
            var departments = new SelectList(departmentsItemOverviews, "Value", "Name");
            var computerTypes = new SelectList(computerTypesItemOverviews, "Value", "Name");
            var contractStatuses = new SelectList(Enum.GetValues(typeof(ContractStatuses)));

            var viewModel = new WorkstationSearchViewModel(
                regions,
                departments,
                computerTypes,
                contractStatuses,
                currentFilter,
                settings);

            return viewModel;
        }
    }
}