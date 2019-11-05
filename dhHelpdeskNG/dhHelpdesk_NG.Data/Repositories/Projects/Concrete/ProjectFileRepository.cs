using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Dal.Repositories.Projects.Concrete
{
	using System.Collections.Generic;
	using System.Linq;

	using DH.Helpdesk.BusinessData.Models.Projects.Input;
	using DH.Helpdesk.BusinessData.Models.Projects.Output;
	using DH.Helpdesk.Dal.Dal;
	using DH.Helpdesk.Dal.Enums;
	using DH.Helpdesk.Dal.Infrastructure;
	using DH.Helpdesk.Dal.Mappers;
	using DH.Helpdesk.Domain.Projects;
	using BusinessData.Models;

	public class ProjectFileRepository : Repository, IProjectFileRepository
    {
        private readonly INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile> newModelMapper;

        private readonly IEntityToBusinessModelMapper<ProjectFile, ProjectFileOverview> overviewMapper;

        private readonly IFilesStorage filesStorage;

        public ProjectFileRepository(
            IDatabaseFactory databaseFactory,
            INewBusinessModelToEntityMapper<NewProjectFile, ProjectFile> newModelMapper,
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

        public List<string> FindFileNamesExcludeSpecified(int projectId, List<string> excludeFiles)
        {
            return
                this.DbContext.ProjectFiles.Where(
                    f => f.Project_Id == projectId && !excludeFiles.Contains(f.FileName))
                    .Select(f => f.FileName)
                    .ToList();
        }

        public FileContentModel GetFileContent(int projectId, string basePath, string fileName)
        {
            return this.filesStorage.GetFileContent(ModuleName.Project, projectId, basePath, fileName);
        }

        public void Add(NewProjectFile businessModel)
        {
            var entity = this.newModelMapper.Map(businessModel);
            this.DbContext.ProjectFiles.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);

            this.filesStorage.SaveFile(businessModel.Content, businessModel.BasePath, businessModel.Name, ModuleName.Project, businessModel.ProjectId);
        }

        public void AddFiles(List<NewProjectFile> businessModels)
        {
            foreach (var newFaqFile in businessModels)
            {
                this.Add(newFaqFile);
            }
        }

        public void DeleteFiles(int projectId, string basePath, List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                this.Delete(projectId, basePath, fileName);
            }
        }

        public void Delete(int projectId)
        {
            var projectFiles = this.DbContext.ProjectFiles.Where(f => f.Project_Id == projectId).ToList();
            projectFiles.ForEach(f => this.DbContext.ProjectFiles.Remove(f));

            // todo need to remove from file storage
        }

        public void Delete(int projectId, string basePath, string fileName)
        {
            var projectFile = this.DbContext.ProjectFiles.Single(f => f.Project_Id == projectId && f.FileName == fileName);
            this.DbContext.ProjectFiles.Remove(projectFile);
            this.filesStorage.DeleteFile(ModuleName.Project, projectId, basePath, fileName);
        }
    }
}