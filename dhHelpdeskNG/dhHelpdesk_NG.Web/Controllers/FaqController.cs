using DH.Helpdesk.Web.Common.Tools.Files;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Admin.Users;
    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Faq;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Attributes;
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

        private readonly IUserPermissionsChecker userPermissionsChecker;

        private readonly IMasterDataService masterDataService;

        private readonly ILanguageService _languageService;

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
            IWorkingGroupRepository workingGroupRepository, 
            ILanguageService languageService, 
            IUserPermissionsChecker userPermissionsChecker)
            : base(masterDataService)
        {
            this.masterDataService = masterDataService;
            this.editFaqModelFactory = editFaqModelFactory;
            this.faqCategoryRepository = faqCategoryRepository;
            this.faqFileRepository = faqFileRepository;
            this.faqRepository = faqRepository;
            this.faqService = faqService;
            this.indexModelFactory = indexModelFactory;
            this.newFaqModelFactory = newFaqModelFactory;
            this.workingGroupRepository = workingGroupRepository;
            this.userPermissionsChecker = userPermissionsChecker;
            this._languageService = languageService;

            this.userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Faq);
        }

        [HttpPost]
        [UserPermissions(UserPermission.FaqPermission)]
        public void DeleteCategory(int id)
        {
            this.faqService.DeleteCategory(id);
        }

        [HttpPost]
        [UserPermissions(UserPermission.FaqPermission)]
        public void DeleteFaq(int id)
        {
            this.faqService.DeleteFaq(id);
        }

        [HttpPost]
        [UserPermissions(UserPermission.FaqPermission)]
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
            byte[] fileContent;

            if (GuidHelper.IsGuid(faqId))
                fileContent = this.userTemporaryFilesStorage.GetFileContent(fileName, faqId);
            else
            {
                var faq = this.faqService.FindById(int.Parse(faqId));
                var basePath = string.Empty;
                if (faq != null)
                    basePath = masterDataService.GetFilePath(faq.CustomerId);

                fileContent = this.faqFileRepository.GetFileContentByFaqIdAndFileName(int.Parse(faqId), basePath, fileName);
            }

            return this.File(fileContent, "application/octet-stream", fileName);
        }

        [HttpGet]
        public ViewResult EditCategory(int id, int languageId)
        {
            var category = this.faqCategoryRepository.GetCategoryById(id, languageId);
            if (category == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var hasFaqs = this.faqRepository.AnyFaqWithCategoryId(id);
            var hasSubcategories = this.faqCategoryRepository.CategoryHasSubcategories(id);

            var languageOverviewsOrginal = _languageService.GetOverviews();
            var languageOverviews =
                languageOverviewsOrginal.Select(
                    o =>
                    new ItemOverview(Translation.GetCoreTextTranslation(o.Name),
                        o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");

            var userHasFaqAdminPermission = this.userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);
            var model = new EditCategoryModel(category.Id, category.Name, hasFaqs, hasSubcategories, userHasFaqAdminPermission, languageList);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        [UserPermissions(UserPermission.FaqPermission)]
        public RedirectToRouteResult EditCategory(EditCategoryInputModel model)
        {
            var editCategory = new EditCategory(model.Id, model.Name, model.LanguageId, DateTime.UtcNow);
            faqService.UpdateCategory(editCategory);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult EditFaq(int id, int languageId, bool showDetails = false)
        {
            var faq = this.faqService.GetFaqById(id, languageId);
            if (faq == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var categoriesWithSubcategories = faqService.GetCategoriesWithSubcategoriesByCustomerId(SessionFacade.CurrentCustomer.Id, languageId);

            var fileNames = this.faqFileRepository.FindFileNamesByFaqId(id);
            List<ItemOverview> workingGroups;

            if (faq.WorkingGroupId.HasValue)
            {
                workingGroups =
                    this.workingGroupRepository.FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
                        faq.CustomerId, faq.WorkingGroupId.Value).OrderBy(w => w.Name).ToList();
            }
            else
            {
                workingGroups = this.workingGroupRepository.FindActiveOverviews(faq.CustomerId).OrderBy(w=> w.Name).ToList();
            }

            var userHasFaqAdminPermission = this.userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);

            var languageOverviewsOrginal = _languageService.GetOverviews(true);
            var languageOverviews =
                languageOverviewsOrginal.Select(
                    o =>
                    new ItemOverview(Translation.GetCoreTextTranslation(o.Name),
                        o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");

            var model = this.editFaqModelFactory.Create(faq, categoriesWithSubcategories, fileNames, workingGroups, userHasFaqAdminPermission, languageList, languageId, showDetails);
            ViewData["FN"] = GetFAQFileNames(faq.Id.ToString());

            return this.View(model);
        }

        [HttpPost, ValidateInput(false)]
        //[BadRequestOnNotValid]
        [UserPermissions(UserPermission.FaqPermission)]
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
                DateTime.Now,
                model.LanguageId);

            this.faqService.UpdateFaq(updatedFaq);
            return this.RedirectToAction("Index", new { showDetails = model.ShowDetails});
        }

        [HttpGet]
        public JsonResult Faqs(int categoryId)
        {
            var faqOverviews = this.faqService.FindOverviewsByCategoryId(categoryId, SessionFacade.CurrentLanguageId);

            var faqModels =
                faqOverviews.Select(
                    f => new FaqOverviewModel(f.Id, f.CreatedDate, f.Text)).ToList();

            return this.Json(faqModels, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public JsonResult FaqsDetailed(int categoryId)
        {
            var faqDetailedOverviews = this.faqService.FindDetailedOverviewsByCategoryId(categoryId, SessionFacade.CurrentLanguageId);
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
        public ViewResult Index(bool showDetails = false)
        {
            var categoriesWithSubcategories = faqService.GetCategoriesWithSubcategoriesByCustomerId(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

            var userHasFaqAdminPermission = this.userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);

            IndexModel model;
            if (!categoriesWithSubcategories.Any())
            {
                model = this.indexModelFactory.Create(null, null, null, userHasFaqAdminPermission);
                return this.View(model);
            }

            var firstCategoryId = 0;
            if (string.IsNullOrEmpty(SessionFacade.TemporaryValue))
            {
                firstCategoryId = categoriesWithSubcategories.First().Id;
                SessionFacade.TemporaryValue = firstCategoryId.ToString();
            }
            else
                firstCategoryId = int.Parse(SessionFacade.TemporaryValue);

            var faqs = this.faqService.FindOverviewsByCategoryId(firstCategoryId, SessionFacade.CurrentLanguageId);
            model = this.indexModelFactory.Create(categoriesWithSubcategories, firstCategoryId, faqs, userHasFaqAdminPermission);            

            return this.View(model);
        }

        [HttpGet]
        public ViewResult NewCategory(int? parentCategoryId)
        {
            var userHasFaqAdminPermission = this.userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);
            var model = new NewCategoryModel(parentCategoryId, userHasFaqAdminPermission);
            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        [UserPermissions(UserPermission.FaqPermission)]
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

            var categoriesWithSubcategories = faqService.GetCategoriesWithSubcategoriesByCustomerId(currentCustomerId, SessionFacade.CurrentLanguageId);

            var workingGroups = this.workingGroupRepository.FindActiveOverviews(currentCustomerId).OrderBy(w=> w.Name).ToList();

            var userHasFaqAdminPermission = this.userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);
            var model = this.newFaqModelFactory.Create(Guid.NewGuid().ToString(), categoriesWithSubcategories, categoryId, workingGroups, userHasFaqAdminPermission);
            ViewData["FN"] = GetFAQFileNames(model.TemporaryId);

            return this.View(model);
        }

        [HttpPost, ValidateInput(false)]
        //[BadRequestOnNotValid]
        [UserPermissions(UserPermission.FaqPermission)]
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
            var basePath = masterDataService.GetFilePath(SessionFacade.CurrentCustomer.Id);
            var newFaqFiles = temporaryFiles.Select(f => new Services.BusinessModels.Faq.NewFaqFile(f.Content, basePath, f.Name, currentDateTime)).ToList();
            this.faqService.AddFaq(newFaq, newFaqFiles);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewFAQPopup(string question, string answer, string internalanswer)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categoriesWithSubcategories = faqService.GetCategoriesWithSubcategoriesByCustomerId(currentCustomerId, SessionFacade.CurrentLanguageId);


            var workingGroups = this.workingGroupRepository.FindActiveOverviews(currentCustomerId).OrderBy(w=> w.Name).ToList();
            if (categoriesWithSubcategories == null || categoriesWithSubcategories.Count < 1)
            {
                ViewData["Err"] = "FAQ Category is empty! First add a category.";
                return this.View("NewFAQPopup");
            }
            else
            {
                var userHasFaqAdminPermission = this.userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);
                var model = this.newFaqModelFactory.Create(Guid.NewGuid().ToString(), categoriesWithSubcategories, categoriesWithSubcategories.First().Id, workingGroups, userHasFaqAdminPermission);
                ViewData["FN"] = GetFAQFileNames(model.TemporaryId);
                return this.View(model);
            }

            
       }

        [HttpGet]
        public JsonResult Search(string pharse)
        {
            var faqOverviews = this.faqService.SearchOverviewsByPharse(pharse, SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

            var faqModels =
                faqOverviews.Select(
                    f => new FaqOverviewModel(f.Id, f.CreatedDate, f.Text)).ToList();

            return this.Json(faqModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchDetailed(string pharse)
        {
            var faqDetailedOverviews = this.faqService.SearchDetailedOverviewsByPharse(
                pharse, SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

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
        [UserPermissions(UserPermission.FaqPermission)]
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

                var faq = this.faqService.FindById(int.Parse(faqId));
                var basePath = string.Empty;
                if (faq != null)
                    basePath = masterDataService.GetFilePath(faq.CustomerId);

                var newFaqFile = new NewFaqFile(uploadedData, basePath, name, DateTime.Now, int.Parse(faqId));
                this.faqService.AddFile(newFaqFile);
            }
        }

        public ActionResult SetSelectedCategory(string value)
        {
            SessionFacade.TemporaryValue = value;

            return this.Json(new { success = true });
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