
namespace DH.Helpdesk.Domain
{
    /// <summary>
    /// The entity.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The is new.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsNew()
        {
            return this.Id <= 0;
        }
    }
}
