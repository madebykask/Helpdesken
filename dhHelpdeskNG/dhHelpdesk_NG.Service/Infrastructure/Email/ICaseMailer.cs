﻿using System.Collections.Generic;

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Infrastructure.Email
{
    public interface ICaseMailer
    {
        void InformNotifierIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,                
                bool dontSendMailToNotfier,
                Case newCase,
                string helpdeskMailFromAdress,
                List<string> files,
                MailSenders mailSenders,
                bool isCreatingCase,
                bool caseMailSetting_DontSendMail,
                string AbsoluterUrl);

        void InformOwnerDefaultGroupIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,
                bool dontSendMailToNotfier,
                Case newCase,
                string helpdeskMailFromAdress,
                List<string> files,
                string AbsoluterUrl);

        void InformAboutInternalLogIfNeeded(
                int caseHistoryId,
                List<Field> fields,
                CaseLog log,
                Case newCase,
                string helpdeskMailFromAdress,
                List<string> files,
                string AbsoluterUrl,
                MailSenders mailSenders);
    }
}