namespace DH.Helpdesk.BusinessData.Models.Licenses.Licenses
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class LicenseFileModel : Shared.Input.BusinessModel
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
                string fileName,
                bool forDelete)
        {
            this.FileName = fileName;
            this.LicenseId = licenseId;
            this.ForDelete = forDelete;
        }

        public int LicenseId { get; private set; }

        [NotNullAndEmpty]
        [MaxLength(200)]
        public string FileName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public bool ForDelete { get; private set; }
    }
}