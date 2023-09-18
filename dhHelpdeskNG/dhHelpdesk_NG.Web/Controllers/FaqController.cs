using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using DH.Helpdesk.BusinessData.Enums;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Models.Faq.Input;
using DH.Helpdesk.BusinessData.Models.Faq.Output;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Common.Enums;
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
using DH.Helpdesk.Web.Models.Faq.Input;
using DH.Helpdesk.Web.Models.Faq.Output;

using NewFaq = DH.Helpdesk.Services.BusinessModels.Faq.NewFaq;
using NewFaqFile = DH.Helpdesk.BusinessData.Models.Faq.Input.NewFaqFile;
using System.IO;

namespace DH.Helpdesk.Web.Controllers
{
    public sealed class FaqController : BaseController
    {
        #region Fields

        private readonly IEditFaqModelFactory _editFaqModelFactory;
        private readonly IFaqCategoryRepository _faqCategoryRepository;
        private readonly IFaqFileRepository _faqFileRepository;
        private readonly IFaqRepository _faqRepository;
        private readonly IFaqService _faqService;
        private readonly IIndexModelFactory _indexModelFactory;
        private readonly INewFaqModelFactory _newFaqModelFactory;
        private readonly ITemporaryFilesCache _userTemporaryFilesStorage;
        private readonly IWorkingGroupRepository _workingGroupRepository;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly IMasterDataService _masterDataService;
        private readonly ILanguageService _languageService;

        private const string DefaultSortField = "Text";
        private const SortOrder DefaultSortOrder = SortOrder.Asc;
		private readonly IGlobalSettingService _globalSettingService;

		#endregion

		#region ctor

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
            IUserPermissionsChecker userPermissionsChecker,
			IGlobalSettingService globalSettingService)
            : base(masterDataService)
        {
            _masterDataService = masterDataService;
            _editFaqModelFactory = editFaqModelFactory;
            _faqCategoryRepository = faqCategoryRepository;
            _faqFileRepository = faqFileRepository;
            _faqRepository = faqRepository;
            _faqService = faqService;
            _indexModelFactory = indexModelFactory;
            _newFaqModelFactory = newFaqModelFactory;
            _workingGroupRepository = workingGroupRepository;
            _userPermissionsChecker = userPermissionsChecker;
            _languageService = languageService;
			_globalSettingService = globalSettingService;
			_userTemporaryFilesStorage = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Faq);
        }

        #endregion

        #region Index

        [HttpGet]
        public ViewResult Index(bool showDetails = false)
        {
            var categoriesWithSubcategories = _faqService.GetCategoriesWithSubcategoriesByCustomerId(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

            var userHasFaqAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);

            IndexModel model;
            if (!categoriesWithSubcategories.Any())
            {
                model = _indexModelFactory.Create(null, null, null, SessionFacade.CurrentLanguageId, userHasFaqAdminPermission);
                return View(model);
            }

            var firstCategoryId = 0;
            if (string.IsNullOrEmpty(SessionFacade.TemporaryValue))
            {
                firstCategoryId = categoriesWithSubcategories.First().Id;
                SessionFacade.TemporaryValue = firstCategoryId.ToString();
            }
            else
            {
                firstCategoryId = int.Parse(SessionFacade.TemporaryValue);
            }

            var faqs =
                _faqService.FindOverviewsByCategoryId(firstCategoryId, SessionFacade.CurrentCustomer.Id, DefaultSortField, DefaultSortOrder, SessionFacade.CurrentLanguageId);

            model = _indexModelFactory.Create(categoriesWithSubcategories, firstCategoryId, faqs, SessionFacade.CurrentLanguageId, userHasFaqAdminPermission);
            model.SortBy = DefaultSortField;
            model.SortOrder = DefaultSortOrder;

            return View(model);
        }

        #endregion

        #region Faq Search Actions

        [HttpGet]
        public ActionResult Faqs(int categoryId, string sortBy, SortOrder sortOrder)
        {
            var faqs =
                _faqService.FindOverviewsByCategoryId(categoryId, SessionFacade.CurrentCustomer.Id, sortBy, sortOrder, SessionFacade.CurrentLanguageId)
                .Select(f => new FaqOverviewModel(f.Id, f.CreatedDate, f.ChangedDate, f.Text))
                .ToList();

            return PartialView("_FaqList", faqs);
        }

        [HttpGet]
        public ActionResult FaqsDetailed(int categoryId, string sortBy, SortOrder sortOrder)
        {
            var faqs =
                _faqService.FindDetailedOverviewsByCategoryId(categoryId, SessionFacade.CurrentCustomer.Id, sortBy, sortOrder, SessionFacade.CurrentLanguageId)
                .Select(MapToDetailedOverviewModel)
                .ToList();

            return PartialView("_FaqListWithDetails", faqs);
        }

        [HttpGet]
        public ActionResult Search(string pharse, string sortBy, SortOrder sortOrder)
        {
            var faqs =
                _faqService.SearchOverviewsByPharse(pharse, SessionFacade.CurrentCustomer.Id, sortBy, sortOrder, SessionFacade.CurrentLanguageId)
                .Select(f => new FaqOverviewModel(f.Id, f.CreatedDate, f.ChangedDate, f.Text))
                .ToList();

            return PartialView("_FaqList", faqs);
        }

        [HttpGet]
        public ActionResult SearchDetailed(string pharse, string sortBy, SortOrder sortOrder)
        {
            var faqs =
                _faqService.SearchDetailedOverviewsByPharse(pharse, SessionFacade.CurrentCustomer.Id, sortBy, sortOrder, SessionFacade.CurrentLanguageId)
                .Select(MapToDetailedOverviewModel)
                .ToList();
            
            return PartialView("_FaqListWithDetails", faqs);
        }

        private FaqDetailedOverviewModel MapToDetailedOverviewModel(FaqDetailedOverview item)
        {
            var faqModel = new FaqDetailedOverviewModel(
                item.Id,
                item.CreatedDate,
                item.ChangedDate,
                item.Text,
                item.Answer,
                item.InternalAnswer,
                item.Files,
                item.UrlOne,
                item.UrlTwo);
            return faqModel;
        }

        #endregion

        #region Faq Edit Actions

        [HttpGet]
        public ViewResult NewFaq(int categoryId)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categoriesWithSubcategories = _faqService.GetCategoriesWithSubcategoriesByCustomerId(currentCustomerId, SessionFacade.CurrentLanguageId);

            var workingGroups = _workingGroupRepository.FindActiveOverviews(currentCustomerId).OrderBy(w => w.Name).ToList();

			var fileUploadWhiteList = _globalSettingService.GetFileUploadWhiteList();

            var userHasFaqAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);
            var model = _newFaqModelFactory.Create(Guid.NewGuid().ToString(), categoriesWithSubcategories, categoryId, workingGroups, userHasFaqAdminPermission, fileUploadWhiteList);
            ViewData["FN"] = GetFAQFileNames(model.TemporaryId);

            return View(model);
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

            var temporaryFiles = _userTemporaryFilesStorage.FindFiles(model.Id);
            var basePath = _masterDataService.GetFilePath(SessionFacade.CurrentCustomer.Id);
            var newFaqFiles = temporaryFiles.Select(f => new Services.BusinessModels.Faq.NewFaqFile(f.Content, basePath, f.Name, currentDateTime)).ToList();
            _faqService.AddFaq(newFaq, newFaqFiles);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewFAQPopup(string question, string answer, string internalanswer)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categoriesWithSubcategories = _faqService.GetCategoriesWithSubcategoriesByCustomerId(currentCustomerId, SessionFacade.CurrentLanguageId);

			var fileUploadWhiteList = _globalSettingService.GetFileUploadWhiteList();

            var workingGroups = _workingGroupRepository.FindActiveOverviews(currentCustomerId).OrderBy(w => w.Name).ToList();
            if (categoriesWithSubcategories == null || categoriesWithSubcategories.Count < 1)
            {
                ViewData["Err"] = "FAQ Category is empty! First add a category.";
                return View("NewFAQPopup");
            }
            else
            {
                var userHasFaqAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);
                var model = _newFaqModelFactory.Create(Guid.NewGuid().ToString(), categoriesWithSubcategories, categoriesWithSubcategories.First().Id, workingGroups, userHasFaqAdminPermission, fileUploadWhiteList);
                ViewData["FN"] = GetFAQFileNames(model.TemporaryId);
                return View(model);
            }
        }

        [HttpGet]
        public ViewResult EditFaq(int id, int languageId, bool showDetails = false)
        {
            var faq = _faqService.GetFaqById(id, languageId);
            //faq.Answer = faq.Answer.Replace("\r\n", "<br>");
            if (faq == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var categoriesWithSubcategories = _faqService.GetCategoriesWithSubcategoriesByCustomerId(SessionFacade.CurrentCustomer.Id, languageId);

            var fileNames = _faqFileRepository.FindFileNamesByFaqId(id);
            List<ItemOverview> workingGroups;

            if (faq.WorkingGroupId.HasValue)
            {
                workingGroups =
                    _workingGroupRepository.FindActiveByCustomerIdIncludingSpecifiedWorkingGroup(
                        faq.CustomerId, faq.WorkingGroupId.Value).OrderBy(w => w.Name).ToList();
            }
            else
            {
                workingGroups = _workingGroupRepository.FindActiveOverviews(faq.CustomerId).OrderBy(w => w.Name).ToList();
            }

            var userHasFaqAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);

            var languageOverviewsOrginal = _languageService.GetOverviews(true);
            var languageOverviews =
                languageOverviewsOrginal.Select(
                    o =>
                        new ItemOverview(Translation.GetCoreTextTranslation(o.Name),
                            o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");

			var fileUploadWhiteList = _globalSettingService.GetFileUploadWhiteList();

            var model = _editFaqModelFactory.Create(faq, categoriesWithSubcategories, fileNames, workingGroups, userHasFaqAdminPermission, languageList, languageId, showDetails, fileUploadWhiteList);
            ViewData["FN"] = GetFAQFileNames(faq.Id.ToString());

            return View(model);
        }
        
        [HttpPost]
        [UserPermissions(UserPermission.FaqPermission)]
        public void DeleteFaq(int id)
        {
            _faqService.DeleteFaq(id);
        }

            #endregion

        #region File Actions

        [HttpGet]
        public FileContentResult DownloadFile(string faqId, string fileName)
        {            
            byte[] fileContent;

            if (GuidHelper.IsGuid(faqId))
            {
                fileContent = _userTemporaryFilesStorage.GetFileContent(fileName, faqId);
            }
            else
            {
                var faq = _faqService.FindById(int.Parse(faqId));
                var basePath = string.Empty;
                if (faq != null)
                    basePath = _masterDataService.GetFilePath(faq.CustomerId);

				var model = _faqFileRepository.GetFileContentByFaqIdAndFileName(int.Parse(faqId), basePath, fileName);
				fileContent = model.Content;
            }

            return File(fileContent, "application/octet-stream", fileName);
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

            _faqService.UpdateFaq(updatedFaq);
            return RedirectToAction("Index", new { showDetails = model.ShowDetails});
        }
        
        [HttpGet]
        public ActionResult Files(string faqId)
        {
            var fileNames = GuidHelper.IsGuid(faqId)
                ? _userTemporaryFilesStorage.FindFileNames(faqId)
                : _faqFileRepository.FindFileNamesByFaqId(int.Parse(faqId));
            var model = new FAQFileModel(){FAQId=faqId,FAQFiles=fileNames.ToList()};
            return PartialView("_FAQFiles", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        [UserPermissions(UserPermission.FaqPermission)]
        public void UploadFile(string faqId, string name)
        {
            var uploadedFile = Request.Files[0];
            var uploadedData = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(uploadedData, 0, uploadedData.Length);

			var extension = Path.GetExtension(name);
			if (!_globalSettingService.IsExtensionInWhitelist(extension))
			{
				throw new ArgumentException($"File extension not valid: {name}");
			}

            if (GuidHelper.IsGuid(faqId))
            {
                if (_userTemporaryFilesStorage.FileExists(name, faqId))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                _userTemporaryFilesStorage.AddFile(uploadedData, name, faqId);
            }
            else
            {
                if (_faqFileRepository.FileExists(int.Parse(faqId), name))
                {
                    throw new HttpException((int)HttpStatusCode.Conflict, null);
                }

                var faq = _faqService.FindById(int.Parse(faqId));
                var basePath = string.Empty;
                if (faq != null)
                    basePath = _masterDataService.GetFilePath(faq.CustomerId);

                var newFaqFile = new NewFaqFile(uploadedData, basePath, name, DateTime.Now, int.Parse(faqId));
                _faqService.AddFile(newFaqFile);
            }
        }

        [HttpPost]
        [UserPermissions(UserPermission.FaqPermission)]
        public void DeleteFile(string faqId, string fileName)
        {
            if (GuidHelper.IsGuid(faqId))
            {
                _userTemporaryFilesStorage.DeleteFile(fileName, faqId);
            }
            else
            {
                _faqFileRepository.DeleteByFaqIdAndFileName(int.Parse(faqId), fileName);
                _faqFileRepository.Commit();
            }
        }

        #endregion

        #region Category Actions

        [HttpGet]
        public ViewResult NewCategory(int? parentCategoryId)
        {
            var userHasFaqAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);
            var model = new NewCategoryModel(parentCategoryId, userHasFaqAdminPermission);
            return View(model);
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

            _faqService.AddCategory(newCategory);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult EditCategory(int id, int languageId)
        {
            var category = _faqCategoryRepository.GetCategoryById(id, languageId);
            if (category == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, null);
            }

            var hasFaqs = _faqRepository.AnyFaqWithCategoryId(id);
            var hasSubcategories = _faqCategoryRepository.CategoryHasSubcategories(id);

            var languageOverviewsOrginal = _languageService.GetOverviews(true);
            var languageOverviews =
                languageOverviewsOrginal.Select(
                    o =>
                        new ItemOverview(Translation.GetCoreTextTranslation(o.Name),
                            o.Value.ToString())).ToList();
            var languageList = new SelectList(languageOverviews, "Value", "Name");

            var userHasFaqAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.FaqPermission);
            var model = new EditCategoryModel(category.Id, category.Name, hasFaqs, hasSubcategories, userHasFaqAdminPermission, languageList);
            return View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        [UserPermissions(UserPermission.FaqPermission)]
        public RedirectToRouteResult EditCategory(EditCategoryInputModel model)
        {
            var editCategory = new EditCategory(model.Id, model.Name, model.LanguageId, DateTime.UtcNow);
            _faqService.UpdateCategory(editCategory);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [UserPermissions(UserPermission.FaqPermission)]
        public void DeleteCategory(int id)
        {
            _faqService.DeleteCategory(id);
        }

        [HttpPost]
        public ActionResult SetSelectedCategory(string value)
        {
            SessionFacade.TemporaryValue = value;
            return Json(new { success = true });
        }

        #endregion

        #region Private Methods

        private string GetFAQFileNames(string id)
        {
            var fileNames = GuidHelper.IsGuid(id)
                ? _userTemporaryFilesStorage.FindFileNames(id)
                : _faqFileRepository.FindFileNamesByFaqId(int.Parse(id));

            return String.Join("|", fileNames);
        }

        #endregion
    }
}