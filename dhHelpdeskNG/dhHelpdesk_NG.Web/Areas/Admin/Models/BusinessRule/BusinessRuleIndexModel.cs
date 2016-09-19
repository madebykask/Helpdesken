using DH.Helpdesk.Common.Enums.BusinessRule;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Admin.Models.BusinessRule
{
    public class BusinessRuleIndexModel
    {
        public BusinessRuleIndexModel()
        {
        }

        public Customer Customer { get; set; }

        public BRModuleModel Module { get; set; }
    }

    public sealed class BRModuleModel
    {
        public BRModuleModel(int id)
        {
            Id = id;
            
            switch (id)
            {
                case BRModule.CASE:
                    Caption = "Ärende";
                    break;
                default:
                    Caption = string.Empty;
                    break;
            }            
        }

        public int Id { get; private set; }

        public string Caption { get; private set; }
    }
}