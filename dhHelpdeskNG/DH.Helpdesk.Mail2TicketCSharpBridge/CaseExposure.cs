using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Dal.DbContext;
using DH.Helpdesk.Dal.Repositories.BusinessRules.Concrete;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Mail2TicketCSharpBridge.Interfaces;
using DH.Helpdesk.Mail2TicketCSharpBridge.Models;
using DH.Helpdesk.Mail2TicketCSharpBridge.Resolver;
using DH.Helpdesk.Services.BusinessLogic.Cases;
using DH.Helpdesk.Services.Services;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Mail2TicketCSharpBridge
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
                var actions = _caseService.CheckBusinessRules(BREventType.OnSaveCase, caseEntity, caseEntity);
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
