using System.Collections.Generic;
using System.Data;
namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class ReportData
    {
        public ReportData(string reportName)
        {
            this.ReportName = reportName;
            this.DataSets = new List<ReportDataSet>();
        }

        public void AddDataSet(string dataSetName, DataTable dataSet)
        {
            this.DataSets.Add(new ReportDataSet(dataSetName, dataSet));
        }

        public void AddDataSets(IList<ReportDataSet> dataSets)
        {
            this.DataSets.AddRange(dataSets);
        }

        public string ReportName { get; private set; }

        public List<ReportDataSet> DataSets { get; private set; }
    }

    public class ReportDataSet
    {
        public ReportDataSet(string dataSetName, DataTable dataSet)
        {
            this.DataSetName = dataSetName;
            this.DataSet = dataSet;
        }

        public string DataSetName { get; private set; }

        public DataTable DataSet { get; private set; }
    }

}