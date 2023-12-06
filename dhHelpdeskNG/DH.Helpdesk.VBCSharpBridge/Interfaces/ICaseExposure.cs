using DH.Helpdesk.VBCSharpBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.VBCSharpBridge.Interfaces
{
    public interface ICaseExposure
    {
        CaseBridge RunBusinessRules(CaseBridge caseObj);
    }
}
