using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Domain;
using DH.Helpdesk.VBCSharpBridge.Interfaces;
using DH.Helpdesk.VBCSharpBridge.Models;
using DH.Helpdesk.VBCSharpBridge.Resolver;
using DH.Helpdesk.Services.Services;
using Newtonsoft.Json;
using System;
using DH.Helpdesk.BusinessData.Models.Case;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Feedback;
using System.Net.Mail;
using DH.Helpdesk.Services.Services.Concrete;

namespace DH.Helpdesk.VBCSharpBridge
{
    public class CaseExposure : ICaseExposure
    {

        private ICaseService _caseService;

        public CaseExposure()
        {
            _caseService = ServiceResolver.GetCaseService();
        }
    
        public CaseBridge RunBusinessRules(CaseBridge caseObj)
        {

            try
            {

                //Map the CaseBridge to Case entity
                var caseEntity = MapCaseBridgeToCase(caseObj);

                // Run the business rules
                caseEntity = _caseService.ExecuteBusinessActionsM2T(caseEntity);
                caseObj.Performer_User_Id = caseEntity.Performer_User_Id;

            }
            catch (Exception ex)
            {
                // Log the exception
                throw ex;
            }


            //Return json string
            return caseObj;


        }

        private Case MapCaseBridgeToCase(CaseBridge caseBridge)
        {

            return new Case()
            {
                Customer_Id = caseBridge.Customer_Id,
                RegUserDomain = caseBridge.FromEmail,
            };
            
        }

        public Case GetCaseById(int caseId)
        {
            return _caseService.GetCaseById(caseId);
        }
        public string GetSurveyBodyString(int customerId, int caseId, int mailtemplateId, string toEmail, string helpdeskEmail, string helpdeskAddress, ref string body)
        {
            CaseExposure caseExposure = new CaseExposure();
            Case caseObj = caseExposure.GetCaseById(caseId);
            CaseMailSetting caseMailSetting = new CaseMailSetting(string.Empty, helpdeskEmail, helpdeskAddress, 1);
            List<Field> fields = new List<Field>();
            List<FeedbackField> feedbackFeelds = _caseService.GetFeedbackFields(mailtemplateId, caseObj, caseMailSetting, fields, toEmail, ref body, null, true);

            //MailMessage mail = new MailMessage();
            //CaseEmailExposure caseEmailExposure = new CaseEmailExposure();
            //mail = caseEmailExposure.GetMailMessage(customerId, caseId, mailtemplateId, toEmail, helpdeskEmail, fields);
            string apa = AddInformationToMailBodyAndSubject(body, feedbackFeelds);
            UpdateFeedbackStatus(feedbackFeelds);
            return apa;
        }
        private string AddInformationToMailBodyAndSubject(string text, List<FeedbackField> fields)
        {
            string ret = text;

            if (fields != null)
                foreach (var f in fields)
                {
                    ret = ret.Replace(f.Key, f.StringValue);
                }

            return ret;
        }
        public void UpdateFeedbackStatus(List<FeedbackField> templateFields)
        {
            _caseService.UpdateFeedbackStatus(templateFields);
        }
    }
}
