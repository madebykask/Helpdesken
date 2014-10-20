namespace DH.Helpdesk.BusinessData.Models.Licenses.Licenses
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LicenseFileModel : EntityBusinessModel
    {
        public LicenseFileModel(
                int id,
                int licenseId, 
                string fileName, 
                DateTime createdDate)
        {
            this.Id = id;
            this.CreatedDate = createdDate;
            this.FileName = fileName;
            this.LicenseId = licenseId;
        }

        public LicenseFileModel(
                int licenseId, 
                string fileName)
        {
            this.FileName = fileName;
            this.LicenseId = licenseId;            
        }

        public LicenseFileModel(
                int licenseId, 
                string fileName,
                byte[] file)
        {
            this.FileName = fileName;
            this.LicenseId = licenseId;
            this.File = file;
        }

        public int LicenseId { get; private set; }

        [NotNullAndEmpty]
        [MaxLength(200)]
        public string FileName { get; private set; }

        public byte[] File { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public bool IsEmpty()
        {
            return this.File == null || this.File.Length == 0;
        }
    }
}