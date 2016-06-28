using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
namespace DH.Helpdesk.Web.Models.Shared
{
    public sealed class ReportModel
    {
        #region Constructors and Destructors       

        public ReportModel()
        {            
        }

        public ReportModel(bool canShow)
        {
            CanShow = canShow;
        }

        public bool CanShow { get; set; }

        #endregion

        public ReportViewer Report { get; set; }

        public List<ReportDataModel> ReportData { get; set; }
    }    

    public sealed class ReportDataModel 
    {        
        public ReportDataModel(int id, string fieldName, string fieldCaption, 
                               string fieldValue,  int inOrder, string lineType)
        {
            Id = id;
            FieldName = fieldName;
            FieldCaption = fieldCaption;
            FieldValue = fieldValue;
            InOrder = inOrder;
            LineType = lineType;
        }

        public int Id { get; private set; }
        public string FieldName { get; private set; }
        public string FieldCaption { get; private set; }
        public string FieldValue { get; private set; }
        public int InOrder { get; private set; }
        public string LineType { get; private set; }
    }

}