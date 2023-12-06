using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Domain;
using DH.Helpdesk.VBCSharpBridge.Interfaces;
using DH.Helpdesk.VBCSharpBridge.Models;
using DH.Helpdesk.VBCSharpBridge.Resolver;
using DH.Helpdesk.Services.Services;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace DH.Helpdesk.VBCSharpBridge
{
    public class CaseExposure : ICaseExposure
    {

        private ICaseService _caseService;

        public CaseExposure()
        {
            _caseService = ServiceResolver.GetCaseService();
        }


        
        public string RunBusinessRules(CaseBridge caseObj)
        {

            try
            {

                //Map the CaseBridge to Case entity
                var caseEntity = MapCaseBridgeToCase(caseObj);

                // Run the business rules
                var actions = _caseService.CheckBusinessRules(BREventType.OnCreateCaseM2T, caseEntity, null);
                if (actions.Any())
                { 
                    //Fix this for the execution
                    //_caseService.ExecuteBusinessActions(actions, currentCase.Id, caseLog, userTimeZone, caseHistoryId, basePath, langId, caseMailSetting, allLogFiles);
                }

            }
            catch (Exception ex)
            {
                // Log the exception
                throw ex;
            }


            //Return json string
            return JsonConvert.SerializeObject(caseObj);


        }

        private Case MapCaseBridgeToCase(CaseBridge caseBridge)
        {

            return new Case()
            {
                Id = caseBridge.Id,
                RegUserDomain = caseBridge.FromEmail,
            };
            
        }

    }
}
