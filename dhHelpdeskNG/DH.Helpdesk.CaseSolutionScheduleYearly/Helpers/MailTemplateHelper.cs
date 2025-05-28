using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionScheduleYearly.Helpers
{
    public  class MailTemplateHelper
    {
        public static string GetMailTemplateIdentifier(string identifier)
        {
            switch (identifier.ToUpperInvariant())
            {
                case "CASENUMBER": return "[#1]";
                case "CUSTOMERNAME": return "[#2]";
                case "PERSONS_NAME": return "[#3]";
                case "CAPTION": return "[#4]";
                case "DESCRIPTION": return "[#5]";
                case "FIRSTNAME": return "[#6]";
                case "SURNAME": return "[#7]";
                case "PERSONS_EMAIL": return "[#8]";
                case "PERSONS_PHONE": return "[#9]";
                case "TEXT_EXTERNAL": return "[#10]";
                case "TEXT_INTERNAL": return "[#11]";
                case "PRIORITYNAME": return "[#12]";
                case "WORKINGGROUPEMAIL": return "[#13]";
                case "WORKINGGROUP": return "[#15]";
                case "REGTIME": return "[#16]";
                case "INVENTORYNUMBER": return "[#17]";
                case "PERSONS_CELLPHONE": return "[#18]";
                case "AVAILABLE": return "[#19]";
                case "PRIORITY_DESCRIPTION": return "[#20]";
                case "WATCHDATE": return "[#21]";
                case "LASTCHANGEDBYUSER": return "[#22]"; 
                case "MISCELLANEOUS": return "[#23]";
                case "PLACE": return "[#24]";
                case "CASETYPE": return "[#25]";
                case "CATEGORY": return "[#26]";
                case "PRODUCTAREA": return "[#27]";
                case "REPORTEDBY": return "[#28]";
                case "REGUSER": return "[#29]";
                case "PERFORMER_PHONE": return "[#70]";
                case "PERFORMER_CELLPHONE": return "[#71]";
                case "PERFORMER_EMAIL": return "[#72]";
                case "ISABOUT_PERSONSNAME": return "[#73]";
                case "AUTOCLOSEDAYS": return "[#80]";
                default: return ""; // Return empty string for unknown identifiers like in VB
            }
        }

    }
}
