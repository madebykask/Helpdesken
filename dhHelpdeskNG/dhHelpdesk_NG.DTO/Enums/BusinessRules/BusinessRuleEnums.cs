﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Enums.BusinessRules
{
    public enum Events
    {
        OnSaveCase = 1,
        OnOpenCase = 2
    }
    
    public enum RuleField
    {
        CaseType   = 1,
        SubStatus  = 2,
        ProductAea = 3,
        Status     = 4
    }

    public enum RuleActionType
    {
        ShowMessage = 0,
        SendEmail   = 1
    }
    public enum GDPRType
    {
        //Anonymization = 1,
        //Deletion = 2
        Avpersonifiering = 1,
        Radering = 2
    }
}
