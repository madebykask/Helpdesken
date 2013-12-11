using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region PROJECT

    public interface IProjectRepository : IRepository<Project>
    {
    }

    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PROJECTCOLLABORATOR

    public interface IProjectCollaboratorRepository : IRepository<ProjectCollaborator>
    {
    }

    public class ProjectCollaboratorRepository : RepositoryBase<ProjectCollaborator>, IProjectCollaboratorRepository
    {
        public ProjectCollaboratorRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PROJECTFILE

    public interface IProjectFileRepository : IRepository<ProjectFile>
    {
    }

    public class ProjectFileRepository : RepositoryBase<ProjectFile>, IProjectFileRepository
    {
        public ProjectFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PROJECTLOG

    public interface IProjectLogRepository : IRepository<ProjectLog>
    {
    }

    public class ProjectLogRepository : RepositoryBase<ProjectLog>, IProjectLogRepository
    {
        public ProjectLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PROJECTSCHEDULE

    public interface IProjectScheduleRepository : IRepository<ProjectSchedule>
    {
    }

    public class ProjectScheduleRepository : RepositoryBase<ProjectSchedule>, IProjectScheduleRepository
    {
        public ProjectScheduleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
