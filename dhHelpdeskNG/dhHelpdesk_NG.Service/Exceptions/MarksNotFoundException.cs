namespace DH.Helpdesk.Services.Exceptions
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class MarksNotFoundException : BusinessLogicException
    {
        public MarksNotFoundException(string message, List<string> marks)
            : base(message)
        {
            this.Marks = marks;
        }

        [NotNull]
        public List<string> Marks { get; private set; }
    }
}