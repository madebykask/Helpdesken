namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportModel
    {
        public ReportModel(string item, string owner, int? ownerId)
        {
            this.Item = item;
            this.Owner = owner;
            this.OwnerId = ownerId;
        }

        [NotNull]
        public string Item { get; set; }

        public string Owner { get; set; }

        public int? OwnerId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ReportModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Item != null ? this.Item.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.Owner != null ? this.Owner.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.OwnerId.GetHashCode();
                return hashCode;
            }
        }

        protected bool Equals(ReportModel other)
        {
            return string.Equals(this.Item, other.Item) && string.Equals(this.Owner, other.Owner) && this.OwnerId == other.OwnerId;
        }
    }
}