using System;
using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseFileDto
    {
        public CaseFileDto(
            byte[] content, 
            string basePath,
            string filename, 
            DateTime createdDate, 
            int referenceId)
            : this(content, basePath, filename, createdDate, referenceId, null)
        {
        }

        public CaseFileDto(
            byte[] content, 
            string basePath,
            string filename, 
            DateTime createdDate, 
            int referenceId,
            int? userId)
        {
            if (content == null || content.Length == 0)
                throw new ArgumentNullException(nameof(content), "Value cannot be null or empty.");
            

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename), "Value cannot be null or empty.");
            

            if (referenceId <= 0)
                throw new ArgumentOutOfRangeException(nameof(referenceId), "Must be more than zero.");

            Content = content;
            BasePath = basePath;
            FileName = filename;
            CreatedDate = createdDate;
            ReferenceId = referenceId;
            UserId = userId;
        }

        public CaseFileDto(
            string basePath,
            string filename,
            int referenceId,
            bool isCaseFile)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename), "Value cannot be null or empty.");
            
            if (referenceId <= 0)
                throw new ArgumentOutOfRangeException(nameof(referenceId), "Must be more than zero.");
            
            
            BasePath = basePath;
            FileName = filename;
            ReferenceId = referenceId;
            IsCaseFile = isCaseFile;
        }

        public int Id { get; set; }

        public byte[] Content { get; private set; }

        public string BasePath { get; private set; }

        public string FileName { get; private set; }

        public string TemporaryFilePath { get; set; }

        public int ReferenceId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int? UserId { get; private set; }

        public bool IsCaseFile { get; set; }
    }
}
