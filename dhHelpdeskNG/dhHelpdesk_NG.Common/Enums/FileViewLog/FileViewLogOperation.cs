namespace DH.Helpdesk.Common.Enums.FileViewLog
{
    public enum FileViewLogOperation
    {
        Legacy = 0, // TODO: Regard old as view?
        View = 1,
        Delete = 2,
        Add = 3,
        AddTemporary = 4
    }
    public enum FileViewLogFileSource
    {
        Helpdesk = 5,
        Selfservice = 6,
        WebApi = 7
    }
}