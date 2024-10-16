using DH.Helpdesk.Domain;
using DH.Helpdesk.VBCSharpBridge.Interfaces;
using DH.Helpdesk.VBCSharpBridge.Models;
using DH.Helpdesk.VBCSharpBridge.Resolver;
using DH.Helpdesk.Services.Services;
using System;
using DH.Helpdesk.BusinessData.Models.Case;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Feedback;

namespace DH.Helpdesk.VBCSharpBridge
{
    public class CaseExposure : ICaseExposure
    {
        private string _absoluteUri = "";
        private readonly ICaseService _caseService;

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
                caseObj.WorkingGroup_Id = caseEntity.WorkingGroup_Id;

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
        public string GetSurveyBodyString(int caseId, int mailtemplateId, string toEmail, string helpdeskEmail, string port, string helpdeskAddress, ref string body)
        {
            CaseExposure caseExposure = new CaseExposure();
            Case caseObj = caseExposure.GetCaseById(caseId);

            if(port == "443")
            {
                _absoluteUri = "https://" + helpdeskAddress;
            }
            else
            {
                _absoluteUri = "http://" + helpdeskAddress;
            }
            CaseMailSetting caseMailSetting = new CaseMailSetting(string.Empty, helpdeskEmail, _absoluteUri, 1);

            List<Field> fields = new List<Field>();
            List<FeedbackField> feedbackFeelds = _caseService.GetFeedbackFields(mailtemplateId, caseObj, caseMailSetting, fields, toEmail, ref body, null, true);
            
            CaseEmailBridge caseEmailBridge = new CaseEmailBridge();
            caseEmailBridge.SurveyBody = AddInformationToMailBodyAndSubject(body, feedbackFeelds);

            UpdateFeedbackStatus(feedbackFeelds);
            return caseEmailBridge.SurveyBody;
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
