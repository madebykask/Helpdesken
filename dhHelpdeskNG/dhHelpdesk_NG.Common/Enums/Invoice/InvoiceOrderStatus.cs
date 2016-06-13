namespace DH.Helpdesk.Common.Enums
{
    public enum InvoiceOrderStates
    {
        NotSaved = 0,
        Saved    = 1,
        Sent     = 2,
        Deleted  = 9
    }

    public enum InvoiceOrderFetchStatus
    {
        All = -1,

        AllNotSent = 0,

        AllSent = 1, 

        Orders = 2,

        OrderNotSent = 3,

        OrderSent = 4,
       
        Credits = 5,

        CreditNotSent = 6,

        CreditSent = 7

    }
}
