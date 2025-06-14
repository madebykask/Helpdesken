﻿namespace DH.Helpdesk.Dal.EntityConfigurations
{    
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DH.Helpdesk.Domain;

    internal sealed class CaseSolutionConditionConfiguration : EntityTypeConfiguration<CaseSolutionConditionEntity>
    {
        #region Constructors and Destructors

        internal CaseSolutionConditionConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.CaseSolution_Id).IsOptional();
            Property(e => e.CaseSolutionConditionGUID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(e => e.Property_Name).IsRequired().HasMaxLength(100);
            Property(e => e.Values).IsRequired();
            Property(e => e.Description).IsOptional().HasMaxLength(200);
            Property(e => e.CreatedByUser_Id).IsOptional();
            Property(e => e.ChangedByUser_Id).IsOptional();
            Property(e => e.Status).IsRequired();
            Property(e => e.CreatedDate).IsRequired();
            Property(e => e.ChangedDate).IsRequired();

            Property(e => e.SortOrder).IsRequired();
            Property(e => e.Name).IsOptional().HasMaxLength(200);

            HasOptional(c => c.CaseSolution)
                .WithMany(cs => cs.Conditions)
                .HasForeignKey(x => x.CaseSolution_Id)
                .WillCascadeOnDelete(false);

            ToTable("tblCaseSolutionCondition");
        }

        #endregion
    }
}