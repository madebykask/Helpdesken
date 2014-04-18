// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingTypeConfiguration.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingTypeConfiguration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.EntityConfigurations.Cases
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain.Cases;

    /// <summary>
    /// The causing type configuration.
    /// </summary>
    internal sealed class CausingTypeConfiguration : EntityTypeConfiguration<CausingType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CausingTypeConfiguration"/> class.
        /// </summary>
        internal CausingTypeConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.ParentId).IsOptional();
            this.Property(c => c.Name).IsRequired().HasMaxLength(100);
            this.Property(c => c.Description).IsOptional().HasMaxLength(300);
            this.Property(c => c.IsActive).IsRequired();

            this.ToTable("tblCausingType");
        }
    }
}