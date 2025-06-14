﻿namespace DH.Helpdesk.Dal.EntityConfigurations.CaseDocument
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseDocumentTemplateConfiguration : EntityTypeConfiguration<CaseDocumentTemplateEntity>
    {
        #region Constructors and Destructors

        internal CaseDocumentTemplateConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Name).IsOptional().HasMaxLength(50);
            Property(e => e.CaseDocumentTemplateGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(e => e.MarginTop).IsRequired();
            Property(e => e.MarginBottom).IsRequired();
            Property(e => e.MarginLeft).IsRequired();
            Property(e => e.MarginRight).IsRequired();
            Property(e => e.HeaderHeight).IsRequired();
            Property(e => e.FooterHeight).IsRequired();

            Property(e => e.ShowFooterFromPageNr).IsRequired();
            Property(e => e.ShowHeaderFromPageNr).IsRequired();
            Property(e => e.Style).IsRequired();

            Property(e => e.ShowAlternativeHeaderOnFirstPage).IsRequired();
            Property(e => e.ShowAlternativeFooterOnFirstPage).IsRequired();

            Property(e => e.DraftHeight).IsRequired();
            Property(e => e.DraftYLocation).IsRequired();
            Property(e => e.DraftRotateAngle).IsRequired();
            Property(e => e.HtmlViewerWidth).IsRequired();

            ToTable("tblCaseDocumentTemplate");
        }

        #endregion
    }
}