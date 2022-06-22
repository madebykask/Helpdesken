namespace DH.Helpdesk.Dal.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    using DH.Helpdesk.Domain;

    public class ParentChildRelationConfiguration : EntityTypeConfiguration<ParentChildRelation>
    {
        internal ParentChildRelationConfiguration()
        {
            this.HasKey(it => new { it.AncestorId, it.DescendantId });
            this.Property(it => it.AncestorId).HasColumnName("Ancestor_Id");
            this.Property(it => it.DescendantId).HasColumnName("Descendant_Id");
			this.Property(it => it.Independent).HasColumnName("Independent");
            this.Property(it => it.RelationType).HasColumnName("RelationType");
            this.ToTable("tblParentChildCaseRelations");
        }
    }
}
