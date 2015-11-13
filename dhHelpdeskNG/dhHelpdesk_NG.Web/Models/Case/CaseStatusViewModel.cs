namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class CaseStatusViewModel
    {
        public CaseStatusViewModel()
        {            
        }

        public DataAttributeGroup CaseData { get; set; } 

    }

    public class DataAttributeGroup
    {
        public DataAttributeGroup()
        {
            this.Children = new List<DataAttributeGroup>();
        }

        public string AttributeName { get; set; }

        public string AttributeValue { get; set; }

        public List<DataAttributeGroup> Children { get; set;}
    }

    public class DataAttribute
    {
        public DataAttribute()
        {
        }

        public string AttributeName { get; set; }

        public string AttributeValue { get; set; }
    }

}