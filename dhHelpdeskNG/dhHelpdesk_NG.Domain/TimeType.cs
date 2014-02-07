
namespace DH.Helpdesk.Domain
{
    public class TimeType : Entity
    {
        public int Invoice { get; set; }
        public int InvoiceTimeFromRegisteredTime { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
