namespace DH.Helpdesk.Domain
{

    public class ActionSettingEntity : Entity
    {
        public int Customer_Id { get; set; }

        public int ObjectId { get; set; }

        public string ObjectValue { get; set; }

        public string ObjectClass { get; set; }

        public bool Visibled { get; set; }        
    }
}
