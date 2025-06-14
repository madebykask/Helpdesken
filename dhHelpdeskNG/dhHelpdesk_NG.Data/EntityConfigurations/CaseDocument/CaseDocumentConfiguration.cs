﻿namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseDocumentConfiguration : EntityTypeConfiguration<CaseDocumentEntity>
    {
        #region Constructors and Destructors

        internal CaseDocumentConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).IsRequired();
            Property(e => e.CaseDocumentGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(e => e.Name).IsOptional().HasMaxLength(100);
            Property(e => e.Description).IsOptional().HasMaxLength(200);
            Property(e => e.Customer_Id).IsOptional();
            Property(e => e.FileType).IsRequired().HasMaxLength(10);
            Property(e => e.SortOrder).IsRequired();
            Property(e => e.CreatedByUser_Id).IsOptional();
            Property(e => e.ChangedByUser_Id).IsOptional();
            Property(e => e.Status).IsRequired();
            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsRequired();
            Property(e => e.CaseDocumentTemplate_Id).IsRequired();

            this.HasRequired(c => c.CaseDocumentTemplate)
                .WithMany(c => c.CaseDocuments)
                .HasForeignKey(c => c.CaseDocumentTemplate_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(c => c.Conditions)
                .WithOptional()
                .HasForeignKey(c => c.CaseDocument_Id)
                .WillCascadeOnDelete(false);

            this.HasMany(x => x.CaseDocumentParagraphsKeys)
                .WithRequired()
                .HasForeignKey(x => x.CaseDocument_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblCaseDocument");
        }

        #endregion
    }
}