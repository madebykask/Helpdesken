using System;

namespace dhHelpdesk_NG.DTO.DTOs.Case
{
    public class CaseFileDto
    {
        public CaseFileDto(byte[] content, string filename, DateTime createdDate, int caseId)
        {
            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException("content", "Value cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("name", "Value cannot be null or empty.");
            }
            if (CaseId <= 0)
            {
                throw new ArgumentOutOfRangeException("caseId", "Must be more than zero.");
            }

            this.Content = content;
            this.FileName = filename;
            this.CreatedDate = createdDate;
            this.CaseId = caseId;
        }

        public int Id { get; set; }
        public byte[] Content { get; private set; }
        public string FileName { get; private set; }
        public string TemporaryFilePath { get; private set; }
        public int CaseId { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }
}
