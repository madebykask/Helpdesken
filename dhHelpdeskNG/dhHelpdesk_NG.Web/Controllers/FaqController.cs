namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Faq;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Faq;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Web.Models.Faq.Input;
    using DH.Helpdesk.Web.Models.Faq.Output;

    using NewFaq = DH.Helpdesk.Services.BusinessModels.Faq.NewFaq;
    using NewFaqFile = DH.Helpdesk.BusinessData.Models.Faq.Input.NewFaqFile;

    public sealed class FaqController : BaseController
    {
        #region Fields

        private readonly IEditFaqModelFactory editFaqModelFactory;

        private readonly IFaqCategoryRepository faqCategoryRepository;

        private readonly IFaqFileRepository faqFileRepository;

        private readonly IFaqRepository faqRepository;

        private readonly IFaqService faqService;

        private readonly IIndexModelFactory indexModelFactory;

        private readonly INewFaqModelFactory newFaqModelFactory;

        private readonly ITemporaryFilesCache userTemporaryFilesStorage;

        private readonly IWorkingGroupRepository workingGroupRepository;

        #endregion

        #region Public Methods and Operators

        public FaqController(
            IMasterDataService masterDataService,
            IEditFaqModelFactory editFaqModelFactory,
            IFaqCategoryRepository faqCategoryRepository,
            IFaqFileRepository faqFileRepository,
            IFaqRepository faqRepository,
            IFaqService faqService,
            IIndexModelFactory indexModelFactory,
            INewFaqModelFactory newFaqModelFactory,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            IWorkingGroupRepository workingGroupRepository)
            : base(masterDataService)
        {
            this.editFaqModelFactory = editFaqModelFactory;
            this.faqCategoryRepository = faqCategoryRepository;
            this.faqFileRepository = faqFileRepository;
            this.faqRepository = faqRepository;
            this.faqService = faqService;
            this.indexModelFactory = indexModelFactory;
            this.newFaqModelFactory = newFaqModelFactory;
            this.workingGroupRepository = workingGroupRepository;

            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Faq);
        }

        [HttpPost]
        public void DeleteCategory(int id)
        {
            this.faqService.DeleteCategory(id);
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
                this.userTemporaryFilesStorage.DeleteFile(fileName, faqId);
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
                                  ? this.userTemporaryFilesStorage.GetFileContent(fileName, faqId)
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
        [BadRequestOnNotValid]
        public RedirectToRouteResult EditCategory(EditCategoryInputModel model)
        {
            this.faqCategoryRepository.UpdateNameById(model.Id, model.Name);
            this.faqCategoryRepository.Commit();
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult EditFaq(int id)
        {
            var faq = this.faqService.FindById(id);
            if (faq == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var categoriesWithSubcategories =
                this.faqCategoryRepository.FindCategoriesWithSubcategoriesByCustomerId(SessionFacade.CurrentCustomer.Id);

            var fileNames = this.faqFileRepository.FindFileNamesByFaqId(id);
            List<ItemOverview> workingGroups;

            if (faq.WorkingGroupId.HasValue)
            {
                workingGroups =
                    this.workingGroupRepository.FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
                        faq.CustomerId, faq.WorkingGroupId.Value);
            }
            else
            {
                workingGroups = this.workingGroupRepository.FindActiveOverviews(faq.CustomerId);
            }

            var model = this.editFaqModelFactory.Create(faq, categoriesWithSubcategories, fileNames, workingGroups);
            ViewData["FN"] = GetFAQFileNames(faq.Id.ToString());

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult EditFaq(EditFaqInputModel model)
        {
            var updatedFaq = new ExistingFaq(
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

            this.faqService.UpdateFaq(updatedFaq);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Faqs(int categoryId)
        {
            var faqOverviews = this.faqService.FindOverviewsByCategoryId(categoryId);

            var faqModels =
                faqOverviews.Select(
                    f => new FaqOverviewModel(f.Id, f.CreatedDate, f.Text)).ToList();

            return this.Json(faqModels, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public JsonResult FaqsDetailed(int categoryId)
        {
            var faqDetailedOverviews = this.faqService.FindDetailedOverviewsByCategoryId(categoryId);
            var faqIds = faqDetailedOverviews.Select(f => f.Id).ToList();
            var faqFiles = this.faqFileRepository.FindFileOverviewsByFaqIds(faqIds);
            var faqModels = new List<FaqDetailedOverviewModel>(faqFiles.Count);

            foreach (var faqDetailedOverview in faqDetailedOverviews)
            {
                var faqModel = new FaqDetailedOverviewModel(
                    faqDetailedOverview.Id,
                    faqDetailedOverview.CreatedDate,
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
        public ActionResult Files(string faqId)
        {
            var fileNames = GuidHelper.IsGuid(faqId)
                                ? this.userTemporaryFilesStorage.FindFileNames(faqId)
                                : this.faqFileRepository.FindFileNamesByFaqId(int.Parse(faqId));
            var model = new FAQFileModel(){FAQId=faqId,FAQFiles=fileNames.ToList()};
            return this.PartialView("_FAQFiles", model);
        }


        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ViewResult"/>.
        /// </returns>
        [HttpGet]
        public ViewResult Index()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categoriesWithSubcategories =
                this.faqCategoryRepository.FindCategoriesWithSubcategoriesByCustomerId(currentCustomerId);

            IndexModel model;
            if (!categoriesWithSubcategories.Any())
            {
                model = this.indexModelFactory.Create(null, null, null);
                return this.View(model);
            }

            var firstCategoryId = categoriesWithSubcategories.First().Id;
            var faqs = this.faqService.FindOverviewsByCategoryId(firstCategoryId);
            model = this.indexModelFactory.Create(categoriesWithSubcategories, firstCategoryId, faqs);

            return this.View(model);
        }

        [HttpGet]
        public ViewResult NewCategory(int? parentCategoryId)
        {
            var model = new NewCategoryModel(parentCategoryId);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult NewCategory(NewCategoryInputModel model)
        {
            var newCategory = new NewCategory(
                model.Name,
                DateTime.Now,
                SessionFacade.CurrentCustomer.Id,
                model.ParentCategoryId);

            this.faqService.AddCategory(newCategory);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewFaq(int categoryId)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categoriesWithSubcategories =
                this.faqCategoryRepository.FindCategoriesWithSubcategoriesByCustomerId(currentCustomerId);

            var workingGroups = this.workingGroupRepository.FindActiveOverviews(currentCustomerId);
            var model = this.newFaqModelFactory.Create(Guid.NewGuid().ToString(), categoriesWithSubcategories, categoryId, workingGroups);
            ViewData["FN"] = GetFAQFileNames(model.TemporaryId);

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
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

            var temporaryFiles = this.userTemporaryFilesStorage.FindFiles(model.Id);
            var newFaqFiles = temporaryFiles.Select(f => new Services.BusinessModels.Faq.NewFaqFile(f.Content, f.Name, currentDateTime)).ToList();
            this.faqService.AddFaq(newFaq, newFaqFiles);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Search(string pharse)
        {
            var faqOverviews = this.faqService.SearchOverviewsByPharse(pharse, SessionFacade.CurrentCustomer.Id);

            var faqModels =
                faqOverviews.Select(
                    f => new FaqOverviewModel(f.Id, f.CreatedDate, f.Text)).ToList();

            return this.Json(faqModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchDetailed(string pharse)
        {
            var faqDetailedOverviews = this.faqService.SearchDetailedOverviewsByPharse(
                pharse, SessionFacade.CurrentCustomer.Id);

            var faqIds = faqDetailedOverviews.Select(f => f.Id).ToList();
            var faqFiles = this.faqFileRepository.FindFileOverviewsByFaqIds(faqIds);
            var faqModels = new List<FaqDetailedOverviewModel>(faqDetailedOverviews.Count);

            foreach (var faqDetailedOverview in faqDetailedOverviews)
            {
                var faqModel = new FaqDetailedOverviewModel(
                    faqDetailedOverview.Id,
                    faqDetailedOverview.CreatedDate,
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
        [BadRequestOnNotValid]
        public void UploadFile(string faqId, string name)
        {
            var uploadedFile = this.Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

            if (GuidHelper.IsGuid(faqId))
            {
                if (this.userTemporaryFilesStorage.FileExists(name, faqId))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                this.userTemporaryFilesStorage.AddFile(uploadedData, name, faqId);
            }
            else
            {
                if (this.faqFileRepository.FileExists(int.Parse(faqId), name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                var newFaqFile = new NewFaqFile(uploadedData, name, DateTime.Now, int.Parse(faqId));
                this.faqService.AddFile(newFaqFile);
            }
        }

        #endregion

        private string GetFAQFileNames(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                               ? this.userTemporaryFilesStorage.FindFileNames(id)
                                : this.faqFileRepository.FindFileNamesByFaqId(int.Parse(id));

            return String.Join("|", fileNames);

        }

 
    }
}