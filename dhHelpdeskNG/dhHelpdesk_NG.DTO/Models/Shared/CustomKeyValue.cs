namespace DH.Helpdesk.BusinessData.Models
{
    public class CustomKeyValue<T1, T2>
    {

        public CustomKeyValue()
        {

        }

        public CustomKeyValue(T1 key, T2 Value)
        {

        }

        public T1 Key { get; set; }

        public T2 Value { get; set; }
    }
}
