using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECT.Core.Service;
using ECT.FormLib.Controllers;
using ECT.FormLib.Models;
using ECT.FormLib.Pdfs;
using ECT.Model.Abstract;
using ECT.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace ECT.FormLib.Areas.Netherlands.Controllers
{
    public class TerminationBasicController : FormLibBaseController
    {      
        public const string xmlPath = "netherlands/hiringbasic.xml";
        private readonly IContractRepository _contractRepository;

        public TerminationBasicController(IContractRepository contractRepository, IFileService fileService, IUserRepository userRepository)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
        }

    //    private NetherlandsPdfPage GetPdfPage(int customerId, string searchKey, string language, string id, Department department)
    //    {
    //        var pdfPage = new NetherlandsPdfPage();
            
    //        pdfPage.department = department;
    //        pdfPage.Language = language;
    //        pdfPage.id = id;

    //        return pdfPage;
    //    }

    //    public ActionResult Contract(string id, int caseId, string path)
    //    {
    //        if (string.IsNullOrEmpty(id))
    //            throw new ArgumentNullException("id");

    //        var contract = _contractRepository.Get(caseId);

    //        if (contract == null)
    //            throw new HttpException(404, "Page not found");

    //        path = (!string.IsNullOrEmpty(path) ? path + ".xml" : mainXmlPath);
    //        var model = new FormModel(path)
    //        {
    //            Contract = contract,
    //            Status = contract.StateSecondaryId,
    //            IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
    //        };

    //        model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormGuid));

    //        var units = _contractRepository.GetUnits(model.CustomerId, FormLibSessions.User.Id, null).ToList();
    //        if (units.Any())
    //        {
    //            model.PopulateFormElementWithOptions("BusinessUnit", units.ToDictionary());
    //        }
    //        model.Company = _contractRepository.GetCompany(model.Contract.Company.Key);

    //        var department = _contractRepository.GetDepartmentByKey(model.Contract.Unit.Key, model.CustomerId);
    //        model.Department = department;

    //        //Get document data
    //        model.DocumentData = _contractRepository.GetDocumentData(caseId);


    //        var lang = "en";
    //        if (id.IndexOf("Dutch") > 0)
    //            lang = "nl";

    //        var page = GetPdfPage(model.CustomerId, model.Contract.Unit.Key, lang, id, department);
    //        page.Contract = model.Contract;
    //        var workStream = new MemoryStream();

    //        var document = new Document(PageSize.A4, 42, 45, 55, 50);
    //        var pdfWriter = PdfWriter.GetInstance(document, workStream);

    //        pdfWriter.PageEvent = page;
    //        pdfWriter.CloseStream = false;

    //        document.Open();

    //        var html = RenderRazorViewToString("~/FormLibContent/Areas/Netherlands/Views/TerminationComplete/Pdfs/" + id + ".cshtml", model, false);
    //        using (TextReader htmlViewReader = new StringReader(html))
    //        {
    //            using (var htmlWorker = new HTMLWorkerExtended(document))
    //            {
    //                string VerdanauniTff = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "VERDANA.TTF");
    //                iTextSharp.text.FontFactory.Register(VerdanauniTff);
    //                htmlWorker.Open();
    //                var styleSheet = new iTextSharp.text.html.simpleparser.StyleSheet();
    //                styleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.FACE, "Verdana Unicode MS");
    //                styleSheet.LoadTagStyle(HtmlTags.BODY, HtmlTags.ENCODING, BaseFont.IDENTITY_H);
    //                htmlWorker.SetStyleSheet(styleSheet);
    //                htmlWorker.Parse(htmlViewReader);
    //            }
    //        }

    //        document.Close();

    //        byte[] byteInfo = workStream.ToArray();
    //        workStream.Write(byteInfo, 0, byteInfo.Length);
    //        workStream.Position = 0;

    //        return this.Pdf(byteInfo);
    //    }

    
    }
}