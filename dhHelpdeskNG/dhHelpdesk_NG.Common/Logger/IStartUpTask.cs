namespace DH.Helpdesk.Common.Logger
{
    public interface IStartUpTask
    {
        bool IsEnabled { get; }

        void Configure();
    }
}