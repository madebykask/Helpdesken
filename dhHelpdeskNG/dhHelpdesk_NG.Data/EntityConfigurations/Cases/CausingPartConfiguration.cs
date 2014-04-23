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
            this.HasKey(c => c.Id);
            this.Property(c => c.ParentId).IsOptional().HasColumnName("Parent_CausingPart_Id");
            this.Property(c => c.Name).IsRequired().HasMaxLength(100);
            this.Property(c => c.Description).IsOptional().HasMaxLength(300);
            this.Property(c => c.Status).IsRequired();

            this.ToTable("tblCausingPart");
        }
    }
}