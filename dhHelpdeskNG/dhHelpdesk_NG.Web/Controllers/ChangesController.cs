namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Enums.Changes;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.BusinessModelFactories.Changes;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes;
    using dhHelpdesk_NG.Web.Models.Changes;

    public class ChangesController : BaseController
    {
        private readonly IChangeService changeService;

        private readonly ISettingsModelFactory settingsModelFactory;

        private readonly IUpdatedFieldSettingsFactory updatedFieldSettingsFactory;

        private readonly IChangesGridModelFactory changesGridModelFactory;

        private readonly ISearchModelFactory searchModelFactory;

        private readonly IChangeModelFactory changeModelFactory;

        private readonly IUpdatedChangeAggregateFactory updatedChangeAggregateFactory;

        private readonly INewChangeModelFactory newChangeModelFactory;

        private readonly INewChangeAggregateFactory newChangeAggregateFactory;

        public ChangesController(
            IMasterDataService masterDataService,
            IChangeService changeService,
            ISettingsModelFactory settingsModelFactory,
            IUpdatedFieldSettingsFactory updatedFieldSettingsFactory,
            IChangesGridModelFactory changesGridModelFactory,
            ISearchModelFactory searchModelFactory, 
            IChangeModelFactory changeModelFactory,
            IUpdatedChangeAggregateFactory updatedChangeAggregateFactory, 
            INewChangeModelFactory newChangeModelFactory, 
            INewChangeAggregateFactory newChangeAggregateFactory)
            : base(masterDataService)
        {
            this.changeService = changeService;
            this.settingsModelFactory = settingsModelFactory;
            this.updatedFieldSettingsFactory = updatedFieldSettingsFactory;
            this.changesGridModelFactory = changesGridModelFactory;
            this.searchModelFactory = searchModelFactory;
            this.changeModelFactory = changeModelFactory;
            this.updatedChangeAggregateFactory = updatedChangeAggregateFactory;
            this.newChangeModelFactory = newChangeModelFactory;
            this.newChangeAggregateFactory = newChangeAggregateFactory;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public void Delete(int id)
        {
            this.changeService.DeleteChange(id);
        }

        [HttpGet]
        public PartialViewResult Settings()
        {
            var fieldSettings = this.changeService.FindSettings(
                SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguage);

            var model = this.settingsModelFactory.Create(fieldSettings);
            return this.PartialView(model);
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Search()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var searchFieldSettings = this.changeService.FindSearchFieldSettings(currentCustomerId);

            List<ItemOverviewDto> statuses = null;
            List<ItemOverviewDto> objects = null;
            List<ItemOverviewDto> workingGroups = null;
            List<ItemOverviewDto> administrators = null;

            if (searchFieldSettings.Statuses.Show)
            {
                statuses = this.changeService.FindStatusOverviews(currentCustomerId);
            }

            if (searchFieldSettings.Objects.Show)
            {
                objects = this.changeService.FindObjectOverviews(currentCustomerId);
            }

            if (searchFieldSettings.WorkingGroups.Show)
            {
                workingGroups = this.changeService.FindActiveWorkingGroupOverviews(currentCustomerId);
            }

            if (searchFieldSettings.Administrators.Show)
            {
                administrators = this.changeService.FindActiveAdministratorOverviews(currentCustomerId);
            }

            var model = this.searchModelFactory.Create(
                searchFieldSettings,
                statuses,
                new List<int>(),
                objects,
                new List<int>(),
                workingGroups,
                new List<int>(),
                administrators,
                new List<int>(),
                ChangeStatus.None,
                string.Empty,
                100);

            return this.PartialView(model);
        }

        [HttpPost]
        public void Settings(SettingsModel model)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var updatedFieldSettings = this.updatedFieldSettingsFactory.Create(
                model, SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguage, DateTime.Now);

            this.changeService.UpdateSettings(updatedFieldSettings);
        }

        [HttpGet]
        public ViewResult NewChange()
        {
            var optionalData = this.changeService.FindChangeOptionalData(SessionFacade.CurrentCustomer.Id);
            var model = this.newChangeModelFactory.Create(Guid.NewGuid().ToString(), optionalData);
            return this.View(model);
        }

        [HttpPost]
        public void NewChange(NewChangeModel inputModel)
        {
            var newChange = this.newChangeAggregateFactory.Create(inputModel, DateTime.Now);
            this.changeService.AddChange(newChange);
        }

        [HttpGet]
        public ViewResult Change(int id)
        {
            var change = this.changeService.FindChange(id);
            var optionalData = this.changeService.FindChangeOptionalData(SessionFacade.CurrentCustomer.Id);
            var model = this.changeModelFactory.Create(change, optionalData);
            
            return this.View(model);
        }

        [HttpPost]
        public void Change(ChangeModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var updatedChange = this.updatedChangeAggregateFactory.Create(inputModel, DateTime.Now);
            this.changeService.UpdateChange(updatedChange);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ChangesGrid(SearchModel searchModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var searchResult = this.changeService.SearchDetailedChangeOverviews(
                currentCustomerId,
                searchModel.StatusIds,
                searchModel.ObjectIds,
                searchModel.OwnerIds,
                searchModel.WorkingGroupIds,
                searchModel.AdministratorIds,
                searchModel.Pharse,
                searchModel.ShowValue, 
                searchModel.RecordsOnPage);

            var fieldSettings = this.changeService.FindFieldOverviewSettings(
                currentCustomerId, SessionFacade.CurrentLanguage);

            var model = this.changesGridModelFactory.Create(searchResult, fieldSettings);
            return this.PartialView(model);
        }
    }
}
