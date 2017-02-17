using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using DH.Helpdesk.EForm.Core.Service;
using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.FormLib.Pdfs;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;

namespace DH.Helpdesk.EForm.FormLib.Areas.SouthKorea.Controllers
{
    public class AdditionalPaymentsDeductionController : FormLib.Controllers.FormLibBaseController
    {
        private readonly IContractRepository _contractRepository;

        public AdditionalPaymentsDeductionController(IContractRepository contractRepository, IUserRepository userRepository, IFileService fileService)
            : base(userRepository, contractRepository, fileService)
        {
            _contractRepository = contractRepository;
        }

        [HttpPost]
        public JsonResult Print(string id, int caseId, string path, string allowanceNr)
        {
            if (allowanceNr == "1")
            {
                allowanceNr = "";
            }

            var response = new AjaxResponse();

            if (string.IsNullOrEmpty(id))
                response.Exception = new ArgumentNullException("id").Message;

            var contract = _contractRepository.Get(caseId);

            if (contract == null)
            {
                response.Exception = new HttpException(404, "Page not found").Message;
                return Json(response);
            }

            var xp = (!string.IsNullOrEmpty(path) ? path + ".xml" : mainXmlPath);
            var model = new FormModel(xp)
            {
                Contract = contract,
                Status = contract.StateSecondaryId,
                IsLooked = !string.IsNullOrEmpty(Request.QueryString["locked"])
            };

            model.Form = _contractRepository.GetFormByGuid(model.FormGuid);
            model.MultiNr = allowanceNr;
            model.PopulateForm(_contractRepository.GetFormDictionary(caseId, model.FormGuid));

            var answer = FormLibSessions.User.FullName;
            var dictionary = new Dictionary<string, string> { { id, answer } };
            model.SetAnswer(id, answer);

            _contractRepository.SaveNew(model.FormGuid
                    , model.Contract.Id
                    , FormLibSessions.User.Id
                    , FormLibSessions.User.RegUserId
                    , model.Status
                    , FormLibUtils.GetSource()
                    , model.Language
                    , Request.UserHostAddress
                    , null
                    , dictionary
                    , FormLibSessions.User.FullName);

            _contractRepository.SaveFileViewLog(new FileViewLog
            {
                CaseId = model.Contract.Id,
                UserId = FormLibSessions.User.Id,
                CreatedDate = DateTime.UtcNow,
                FileSource = 3,
                FileName = id,
                FilePath = "..."
            });

            ////Get document data
            //model.DocumentData = _contractRepository.GetDocumentData(caseId);

            response.Result = ""; // Show who printed out the document. Shouldnt be used anymore FormLibSessions.User.FullName;

            return Json(response);
        }

        

        
    }
}

