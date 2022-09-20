using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models.Faq.Output;
using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;
using DH.Helpdesk.Services.Services;

namespace DH.Helpdesk.SelfService.Controllers
{
    using BusinessData.Models.Faq.Input;
    using Infrastructure.Common.Concrete;
    using Models.FAQ;
    using System;
    using System.Collections.Generic;

    public class FAQController : BaseController
    {
        private static string baseFilePath = "";

        private readonly IMasterDataService _masterDataService;
        private readonly ICustomerService _customerService;
        private readonly IFaqService _faqService;
        
        public FAQController(IMasterDataService masterDataService,
                             ISelfServiceConfigurationService configurationService,
                             ICustomerService customerService,
                             ICaseSolutionService caseSolutionService,
                             IFaqService faqService)
                : base(configurationService, masterDataService, caseSolutionService)
        {
            _masterDataService = masterDataService;
            _customerService = customerService;
            _faqService = faqService;
        }

        public ActionResult Index()
        {
            if (SessionFacade.CurrentCustomer == null)
            {
                ErrorGenerator.MakeError("Customer is not valid!");
                return RedirectToAction("Index", "Error");
            }

            var customerId = SessionFacade.CurrentCustomer.Id;
            var model = GetIndexViewModel(customerId, SessionFacade.CurrentLanguageId);

            baseFilePath = _masterDataService.GetFilePath(customerId);

            return View(model);
        }

        private FAQIndexViewModel GetIndexViewModel(int customerId, int languageId)
        {
            var ret = new FAQIndexViewModel();
            var allFaqCats = _faqService.GetFaqCategories(customerId, languageId).OrderBy(c => c.Name).ToList();
            var allFaqs = _faqService.GetFaqs(customerId, languageId, false);

            var parentsCategories = allFaqCats.Where(c => !c.Parent_Id.HasValue).ToList();

            foreach (var cat in parentsCategories)
            {
                ret.FAQCategories.Add(GetCategoryWithFaq(allFaqCats, allFaqs, cat));
            }

            return ret;
        }

        private FAQCategoryViewModel GetCategoryWithFaq(IList<FaqCategory> categories, IList<Faq> faqs, FaqCategory currentCategory)
        {
            var ret = new FAQCategoryViewModel();
            ret.Id = currentCategory.Id;
            ret.Name = currentCategory.Name;
            ret.FaqRows = GetFaqsFor(faqs, currentCategory.Id);

            var firstLevelChildren = categories.Where(c => c.Parent_Id.HasValue && c.Parent_Id.Value == currentCategory.Id).ToList();
            foreach (var child in firstLevelChildren)
            {
                ret.SubCategories.Add(GetCategoryWithFaq(categories, faqs, child));
            }

            return ret;
        }

        private List<FAQRowModel> GetFaqsFor(IList<Faq> faqs, int categoryId)
        {
            var ret = new List<FAQRowModel>();
            ret = faqs.Where(f => f.FaqCategoryId == categoryId)
                      .Select(f =>
                        new FAQRowModel
                        {
                            Id = f.Id,
                            Question = f.Question,
                            Answer = f.Answer,
                            InternalAnswer = f.InternalAnswer,
                            Url1 = f.UrlOne,
                            Url2 = f.UrlTwo,
                            CreatedDate = f.CreatedDate,
                            Files = f.Files.Select(fi => new FAQFileModel
                            {
                                Id = fi.Id,
                                Faq_Id = fi.Faq_Id,
                                FileName = fi.FileName,
                                CreatedDate = fi.CreatedDate
                            })
                                           .OrderBy(fi => fi.CreatedDate)
                                           .ToList()
                        }).OrderBy(f=> f.Question).ToList();

            return ret;
        }

        [HttpGet]
        public FileContentResult DownloadFile(string faqId, string fileName)
        {
            var fileContent = new byte[] { };
            try
            {
                fileContent = _faqService.GetFileContentByFaqIdAndFileName(int.Parse(faqId), baseFilePath, fileName);
            }
            catch (Exception)
            {

            }
            return File(fileContent, "application/octet-stream", fileName);
        }
    }
}
