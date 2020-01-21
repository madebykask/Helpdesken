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
            string userName,
            bool canDelete,
            bool isTemporary = false)
        {
            this.UserName = userName;
            this.CreatedDate = createdDate;
            this.FileName = fileName;
            this.CaseId = caseId;
            this.Id = id;
            this.CanDelete = canDelete;
            this.IsTemporary = isTemporary;
        }

        public int Id { get; private set; }

        public int CaseId { get; private set; }

        public string FileName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public string UserName { get; private set; }

        public bool CanDelete { get; private set; }

        public bool IsTemporary { get; private set; }
    }
}