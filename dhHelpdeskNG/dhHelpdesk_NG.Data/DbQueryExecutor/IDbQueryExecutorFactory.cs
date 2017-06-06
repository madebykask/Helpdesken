namespace DH.Helpdesk.Dal.DbQueryExecutor
{
    public interface IDbQueryExecutorFactory
    {
        IDbQueryExecutor Create();
    }
}