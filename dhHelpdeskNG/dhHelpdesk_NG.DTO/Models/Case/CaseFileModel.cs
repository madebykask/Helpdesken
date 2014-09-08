namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    public sealed class CaseFileModel
    {
        public CaseFileModel(
                int id, 
                int caseId, 
                string fileName, 
                DateTime createdDate, 
                string userName)
        {
            this.UserName = userName;
            this.CreatedDate = createdDate;
            this.FileName = fileName;
            this.CaseId = caseId;
            this.Id = id;
        }

        public int Id { get; private set; }

        public int CaseId { get; private set; }

        public string FileName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public string UserName { get; private set; }
    }
}