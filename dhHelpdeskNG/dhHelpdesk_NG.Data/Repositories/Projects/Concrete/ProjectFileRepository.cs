namespace dhHelpdesk_NG.Data.Repositories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Dal.Mappers;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Projects;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Input;
    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class ProjectFileRepository : Repository, IProjectFileRepository
    {
        private readonly IBusinessModelToEntityMapper<NewProjectFileDto, ProjectFile> newModelMapper;

        private readonly IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview> overviewMapper;

        private readonly IFilesStorage filesStorage;

        public ProjectFileRepository(
            IDatabaseFactory databaseFactory,
            IBusinessModelToEntityMapper<NewProjectFileDto, ProjectFile> newModelMapper,
            IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview> overviewMapper,
            IFilesStorage filesStorage)
            : base(databaseFactory)
        {
            this.newModelMapper = newModelMapper;
            this.overviewMapper = overviewMapper;
            this.filesStorage = filesStorage;
        }

        public bool FileExists(int projectId, string fileName)
        {
            return this.DbContext.ProjectFiles.Any(f => f.Project_Id == projectId && f.FileName == fileName);
        }

        public List<string> FindFileNames(int projectId)
        {
            return this.DbContext.ProjectFiles.Where(f => f.Project_Id == projectId).Select(f => f.FileName).ToList();
        }

        public List<ProjectFileOverview> FindFileOverviews(List<int> projectIdIds)
        {
            var projectFileEntities = this.DbContext.ProjectFiles.Where(f => projectIdIds.Contains(f.Project_Id));
            return projectFileEntities.Select(this.overviewMapper.Map).ToList();
        }

        public byte[] GetFileContent(int projectId, string fileName)
        {
            return this.filesStorage.GetFileContent(Topic.Project, projectId, fileName);
        }

        public void Add(NewProjectFileDto businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.DbContext.ProjectFiles.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);

            this.filesStorage.SaveFile(businessModel.Content, businessModel.Name, Topic.Project, businessModel.ProjectId);
        }

        public void AddFiles(List<NewProjectFileDto> businessModels)
        {
            foreach (var newFaqFile in businessModels)
            {
                this.Add(newFaqFile);
            }
        }

        public void Delete(int projectId)
        {
            var projectFiles = this.DbContext.ProjectFiles.Where(f => f.Project_Id == projectId).ToList();
            projectFiles.ForEach(f => this.DbContext.ProjectFiles.Remove(f));
        }

        public void Delete(int projectId, string fileName)
        {
            var projectFile = this.DbContext.ProjectFiles.Single(f => f.Project_Id == projectId && f.FileName == fileName);
            this.DbContext.ProjectFiles.Remove(projectFile);
        }
    }
}