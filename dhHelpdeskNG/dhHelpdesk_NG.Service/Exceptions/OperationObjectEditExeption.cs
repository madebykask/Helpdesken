namespace DH.Helpdesk.Services.Exceptions
{
    public sealed class OperationObjectEditExeption: BusinessLogicException
    {
        public OperationObjectEditExeption(string message)
            : base(message)
        {
        }
    }
}