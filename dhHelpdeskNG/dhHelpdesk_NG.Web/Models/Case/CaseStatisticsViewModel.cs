namespace DH.Helpdesk.Web.Models.Case
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Web.Infrastructure;

    public sealed class CaseStatisticsViewModel
    {
        public CaseStatisticsViewModel()
        {
            this.CaseData = new DataAttributeGroup();
        }

        public string ExpandedGroup { get; set; }

        public DataAttributeGroup CaseData { get; set; }         

    }

    public class DataAttributeGroup
    {
        public DataAttributeGroup()
        {
            this.Children = new List<DataAttributeGroup>();
        }

        public DataAttributeGroup(string attributeId, string attributeName, string attributeValue)
        {
            this.AttributeId = attributeId;
            this.AttributeName = attributeName;
            this.AttributeValue = attributeValue;
            this.Children = new List<DataAttributeGroup>();
        }

        public DataAttributeGroup(string attributeId, string attributeName, 
                                  string attributeValue, List<DataAttributeGroup> children)
        {
            this.AttributeId = attributeId;
            this.AttributeName = attributeName;
            this.AttributeValue = attributeValue;
            this.Children = children;
        }

        public string AttributeId { get; set; }

        public string AttributeName { get; set; }

        public string AttributeValue { get; set; }        

        public List<DataAttributeGroup> Children { get; set; }

        public void AddChild(DataAttributeGroup child)
        {
            if (this.Children == null)
            {
                this.Children = new List<DataAttributeGroup>();
                this.Children.Add(child);
            }
            else
                this.Children.Add(child);
        }
    }

}