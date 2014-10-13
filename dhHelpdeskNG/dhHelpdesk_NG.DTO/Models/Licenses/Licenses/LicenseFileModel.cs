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
                int id,
                int licenseId, 
                string fileName)
        {
            this.Id = id;
            this.FileName = fileName;
            this.LicenseId = licenseId;            
        }

        [IsId]
        public int LicenseId { get; private set; }

        [NotNullAndEmpty]
        [MaxLength(200)]
        public string FileName { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}