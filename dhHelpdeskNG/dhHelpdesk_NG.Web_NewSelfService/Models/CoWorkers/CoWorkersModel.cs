using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DH.Helpdesk.NewSelfService.Models.CoWorkers
{
    
    using DH.Helpdesk.Domain;        
    

    public class CoWorkersModel 
    {
        public CoWorkersModel()
        {             
        }

        //public CoWorkersModel(List<CoWorkerTab> cwTabs, int activeTabId)
        //{
        //    this.CoWorkersTabs = cwTabs;
        //    this.ActiveTabId = activeTabId;
        //}

        public List<CoWorker> CoWorkers { get; set; }

        //public int ActiveTabId { get; set; }
                
    }


    public class CoWorker
    {
        //public int Id { get; set; }

        public string EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public string JobKey { get; set; }

        public string Email { get; set; }

    }

    //public class CaseInfo
    //{
    //    public int CaseNo { get; set; }

    //    public string CaseName { get; set; }
        
    //}

    //public class CoWorkerTab
    //{
    //    public int Id { get; set; }

    //    public List<CaseInfo> Cases { get; set; }
    //}


}
