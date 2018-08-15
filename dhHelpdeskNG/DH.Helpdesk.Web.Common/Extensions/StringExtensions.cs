namespace DH.Helpdesk.Web.Common.Extensions
{

    public static class StringExtensions
    {

        public static string ReturnCustomerUserValue(this string valueToReturn)
        {
            var ret = string.Empty;

            if (!string.IsNullOrWhiteSpace(valueToReturn))  
                if (valueToReturn != "0")   
                    ret = valueToReturn; 

            return ret;
        }
    }
}