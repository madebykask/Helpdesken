// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartConfiguration.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPartConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Cases;

    /// <summary>
    /// The causing type configuration.
    /// </summary>
    internal sealed class CausingPartConfiguration : EntityTypeConfiguration<CausingPart>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CausingPartConfiguration"/> class.
        /// </summary>
        internal CausingPartConfiguration()
        {
            this.HasKey(x => x.Id);

            this.HasRequired(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ParentId).IsOptional().HasColumnName("Parent_CausingPart_Id");
            this.Property(c => c.Name).IsRequired().HasMaxLength(100);
            this.Property(c => c.Description).IsOptional().HasMaxLength(300);
            this.Property(c => c.Status).IsRequired();

            this.ToTable("tblCausingPart");
        }
    }
}