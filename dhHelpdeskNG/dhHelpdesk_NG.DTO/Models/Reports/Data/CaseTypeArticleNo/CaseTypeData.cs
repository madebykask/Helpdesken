namespace DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo
{
    public sealed class CaseTypeData
    {
        public CaseTypeData()
        {            
        }

        public CaseTypeData(
                int id, 
                string name, 
                string details)
        {
            this.Details = details;
            this.Name = name;
            this.Id = id;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Details { get; set; }
    }
}