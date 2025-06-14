﻿using DH.Helpdesk.SelfService.Models.Case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.SelfService.Models.SelfService
{
    public class SelfServiceModel
    {
        public SelfServiceModel(int customerId, int languageId)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
        }

        public int IsEmptyCase { get; set; }

        public int CustomerId { get; set; }

        public int LanguageId { get; set; }

        public string AUser { get; set; }

        public string ExLogFileGuid { get; set; }

        public string MailGuid { get; set; }

        public CaseOverviewModel CaseOverview { get; set; }

        public NewCaseModel NewCase { get; set; }

        public UserCasesModel UserCases { get; set; }

        public SelfServiceConfigurationModel Configuration { get; set; }

    }
}