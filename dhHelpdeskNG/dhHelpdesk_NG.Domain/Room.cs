namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Room : Entity
    {
        public Room()
        {
        }

        public int Floor_Id { get; set; }
        public int IsActive { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Floor Floor { get; set; }
    }
}
