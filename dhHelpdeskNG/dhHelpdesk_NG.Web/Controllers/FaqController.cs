namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Common.Tools;
    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Faq;
    using dhHelpdesk_NG.DTO.DTOs.Faq.Input;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Service.WorkflowModels.Faq;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Faq;
    using dhHelpdesk_NG.Web.Infrastructure.Tools;
    using dhHelpdesk_NG.Web.Models.Faq.Input;
    using dhHelpdesk_NG.Web.Models.Faq.Output;

    public sealed class FaqController : BaseController
    {
        #region Fields

        private readonly IEditFaqModelFactory editFaqModelFactory;

        private readonly IFaqCategoryRepository faqCategoryRepository;

        private readonly IFaqCategoryService faqCategoryService;

        private readonly IFaqFileRepository faqFileRepository;

        private readonly IFaqRepository faqRepository;

        private readonly IFaqService faqService;

        private readonly IIndexModelFactory indexModelFactory;

        private readonly INewFaqModelFactory newFaqModelFactory;

        private readonly IWebTemporaryStorage webTemporaryStorage;

        private readonly IWorkingGroupRepository workingGroupRepository;

        #endregion

        #region Public Methods and Operators

        public FaqController(IMasterDataService masterDataService, IEditFaqModelFactory editFaqModelFactory, IFaqCategoryRepository faqCategoryRepository, IFaqCategoryService faqCategoryService, IFaqFileRepository faqFileRepository, IFaqRepository faqRepository, IFaqService faqService, IIndexModelFactory indexModelFactory, INewFaqModelFactory newFaqModelFactory, IWebTemporaryStorage webTemporaryStorage, IWorkingGroupRepository workingGroupRepository)
            : base(masterDataService)
        {
            this.editFaqModelFactory = editFaqModelFactory;
            this.faqCategoryRepository = faqCategoryRepository;
            this.faqCategoryService = faqCategoryService;
            this.faqFileRepository = faqFileRepository;
            this.faqRepository = faqRepository;
            this.faqService = faqService;
            this.indexModelFactory = indexModelFactory;
            this.newFaqModelFactory = newFaqModelFactory;
            this.webTemporaryStorage = webTemporaryStorage;
            this.workingGroupRepository = workingGroupRepository;
        }

        [HttpPost]
        public void DeleteCategory(int id)
        {
            this.faqCategoryService.DeleteCategory(id);
        }

        [HttpPost]
        public void DeleteFaq(int id)
        {
            this.faqService.DeleteFaq(id);
        }

        [HttpPost]
        public void DeleteFile(string faqId, string fileName)
        {
            if (GuidHelper.IsGuid(faqId))
            {
                this.webTemporaryStorage.DeleteFile(Topic.Faq, faqId, fileName);
            }
            else
            {
                this.faqFileRepository.DeleteByFaqIdAndFileName(int.Parse(faqId), fileName);
                this.faqFileRepository.Commit();
            }
        }

        [HttpGet]
        public FileContentResult DownloadFile(string faqId, string fileName)
        {
            var fileContent = GuidHelper.IsGuid(faqId)
                                  ? this.webTemporaryStorage.GetFileContent(Topic.Faq, faqId, fileName)
                                  : this.faqFileRepository.GetFileContentByFaqIdAndFileName(int.Parse(faqId), fileName);

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public ViewResult EditCategory(int id)
        {
            var category = this.faqCategoryRepository.FindById(id);
            if (category == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var hasFaqs = this.faqRepository.AnyFaqWithCategoryId(id);
            var hasSubcategories = this.faqCategoryRepository.CategoryHasSubcategories(id);
            var model = new EditCategoryModel(category.Id, category.Name, hasFaqs, hasSubcategories);
            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult EditCategory(EditCategoryInputModel model)
        {
            this.faqCategoryRepository.UpdateNameById(model.Id, model.Name);
            this.faqCategoryRepository.Commit();
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult EditFaq(int id)
        {
            var faq = this.faqRepository.FindById(id);
            if (faq == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var categoriesWithSubcategories =
                this.faqCategoryRepository.FindCategoriesWithSubcategoriesByCustomerId(SessionFacade.CurrentCustomer.Id);

            var fileNames = this.faqFileRepository.FindFileNamesByFaqId(id);
            List<WorkingGroupOverview> workingGroups;

            if (faq.WorkingGroupId.HasValue)
            {
                workingGroups =
                    this.workingGroupRepository.FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
                        faq.CustomerId, faq.WorkingGroupId.Value);
            }
            else
            {
                workingGroups = this.workingGroupRepository.FindActiveByCustomerId(faq.CustomerId);
            }

            var model = this.editFaqModelFactory.Create(faq, categoriesWithSubcategories, fileNames, workingGroups);
            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult EditFaq(EditFaqInputModel model)
        {
            var existingFaq = new ExistingFaqDto(
                model.Id,
                model.CategoryId,
                model.Question,
                model.Answer,
                model.InternalAnswer,
                model.UrlOne,
                model.UrlTwo,
                model.WorkingGroupId,
                model.InformationIsAvailableForNotifiers,
                model.ShowOnStartPage,
                DateTime.Now);

            this.faqRepository.Update(existingFaq);
            this.faqRepository.Commit();
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Faqs(int categoryId)
        {
            var faqOverviews = this.faqRepository.FindOverviewsByCategoryId(categoryId);

            var faqModels =
                faqOverviews.Select(
                    f => new FaqOverviewModel(f.Id, f.CreatedDate.ToString(CultureInfo.InvariantCulture), f.Text)).ToList();

            return this.Json(faqModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FaqsDetailed(int categoryId)
        {
            var faqDetailedOverviews = this.faqRepository.FindDetailedOverviewsByCategoryId(categoryId);
            var faqIds = faqDetailedOverviews.Select(f => f.Id).ToList();
            var faqFiles = this.faqFileRepository.FindFileOverviewsByFaqIds(faqIds);
            var faqModels = new List<FaqDetailedOverviewModel>(faqFiles.Count);

            foreach (var faqDetailedOverview in faqDetailedOverviews)
            {
                var faqModel = new FaqDetailedOverviewModel(
                    faqDetailedOverview.Id,
                    faqDetailedOverview.CreatedDate.ToString(CultureInfo.InvariantCulture),
                    faqDetailedOverview.Text,
                    faqDetailedOverview.Answer,
                    faqDetailedOverview.InternalAnswer,
                    faqFiles.Where(f => f.FaqId == faqDetailedOverview.Id).Select(f => f.Name).ToList(),
                    faqDetailedOverview.UrlOne,
                    faqDetailedOverview.UrlTwo);

                faqModels.Add(faqModel);
            }

            return this.Json(faqModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Files(string faqId)
        {
            var fileNames = GuidHelper.IsGuid(faqId)
                                ? this.webTemporaryStorage.GetFileNames(Topic.Faq, faqId)
                                : this.faqFileRepository.FindFileNamesByFaqId(int.Parse(faqId));

            return this.Json(fileNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ViewResult Index()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categoriesWithSubcategories =
                this.faqCategoryRepository.FindCategoriesWithSubcategoriesByCustomerId(currentCustomerId);

            var firstCategoryId = categoriesWithSubcategories.First().Id;
            var faqs = this.faqRepository.FindOverviewsByCategoryId(firstCategoryId);
            var model = this.indexModelFactory.Create(categoriesWithSubcategories, firstCategoryId, faqs);

            return this.View(model);
        }

        [HttpGet]
        public ViewResult NewCategory(int? parentCategoryId)
        {
            var model = new NewCategoryModel(parentCategoryId);
            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult NewCategory(NewCategoryInputModel model)
        {
            var newCategory = new NewCategoryDto(
                model.Name, DateTime.Now, SessionFacade.CurrentCustomer.Id, model.ParentCategoryId);

            this.faqCategoryRepository.Add(newCategory);
            this.faqCategoryRepository.Commit();
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewFaq(int categoryId)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categoriesWithSubcategories =
                this.faqCategoryRepository.FindCategoriesWithSubcategoriesByCustomerId(currentCustomerId);

            var workingGroups = this.workingGroupRepository.FindActiveByCustomerId(currentCustomerId);

            var model = this.newFaqModelFactory.Create(Guid.NewGuid().ToString(), categoriesWithSubcategories, categoryId, workingGroups);
            
            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult NewFaq(NewFaqInputModel model)
        {
            var currentDateTime = DateTime.Now;

            var newFaq = new NewFaq(
                model.CategoryId,
                model.Question,
                model.Answer,
                model.InternalAnswer,
                model.UrlOne,
                model.UrlTwo,
                model.WorkingGroupId,
                model.InformationIsAvailableForNotifiers,
                model.ShowOnStartPage,
                SessionFacade.CurrentCustomer.Id,
                currentDateTime);

            var temporaryFiles = this.webTemporaryStorage.GetFiles(Topic.Faq, model.Id);
            var newFaqFiles = temporaryFiles.Select(f => new NewFaqFile(f.Content, f.Name, currentDateTime)).ToList();
            this.faqService.AddFaq(newFaq, newFaqFiles);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Search(string pharse)
        {
            var faqOverviews = this.faqRepository.SearchOverviewsByPharse(pharse);

            var faqModels =
                faqOverviews.Select(
                    f => new FaqOverviewModel(f.Id, f.CreatedDate.ToString(CultureInfo.InvariantCulture), f.Text)).ToList();

            return this.Json(faqModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchDetailed(string pharse)
        {
            var faqDetailedOverviews = this.faqRepository.SearchDetailedOverviewsByPharse(pharse);
            var faqIds = faqDetailedOverviews.Select(f => f.Id).ToList();
            var faqFiles = this.faqFileRepository.FindFileOverviewsByFaqIds(faqIds);
            var faqModels = new List<FaqDetailedOverviewModel>(faqDetailedOverviews.Count);

            foreach (var faqDetailedOverview in faqDetailedOverviews)
            {
                var faqModel = new FaqDetailedOverviewModel(
                    faqDetailedOverview.Id,
                    faqDetailedOverview.CreatedDate.ToString(CultureInfo.InvariantCulture),
                    faqDetailedOverview.Text,
                    faqDetailedOverview.Answer,
                    faqDetailedOverview.InternalAnswer,
                    faqFiles.Where(f => f.FaqId == faqDetailedOverview.Id).Select(f => f.Name).ToList(),
                    faqDetailedOverview.UrlOne,
                    faqDetailedOverview.UrlTwo);

                faqModels.Add(faqModel);
            }

            return this.Json(faqModels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void UploadFile(string faqId, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(faqId))
            {
                if (this.webTemporaryStorage.FileExists(Topic.Faq, faqId, name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                this.webTemporaryStorage.Save(uploadedData, Topic.Faq, faqId, name);
            }
            else
            {
                if (this.faqFileRepository.FileExists(int.Parse(faqId), name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                var newFaqFile = new NewFaqFileDto(uploadedData, name, DateTime.Now, int.Parse(faqId));
                this.faqFileRepository.AddFile(newFaqFile);
                this.faqFileRepository.Commit();
            }
        }

        #endregion
    }
}