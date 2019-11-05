using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Enums.FileViewLogs;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.FileViewLog;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Linq;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Dal.Mappers;

namespace DH.Helpdesk.Dal.Repositories
{
	public interface IFileViewLogRepository : IRepository<FileViewLogEntity>
	{
		IList<FileViewLogListItemModel> Find(FileViewLogListFilter filter);
	}
	public class FileViewLogRepository : RepositoryBase<FileViewLogEntity>, IFileViewLogRepository
	{

		public FileViewLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
		}

		public IList<FileViewLogListItemModel> Find(FileViewLogListFilter filter)
        {
            var query = Table.AsNoTracking().Where(f => f.Case.Customer_Id == filter.CustomerId);
            if (filter.DepartmentsIds != null)
            {
                query = query.Where(f => (filter.IncludeEmptyDepartments && !f.Case.Department_Id.HasValue) ||
                    (f.Case.Department_Id.HasValue && filter.DepartmentsIds.Contains(f.Case.Department_Id.Value)));
            }

            if (filter.PeriodFrom.HasValue)
                query = query.Where(f => f.CreatedDate >= filter.PeriodFrom.Value);
            if (filter.PeriodTo.HasValue)
                query = query.Where(f => f.CreatedDate <= filter.PeriodTo.Value);

            switch (filter.Sort.Name)
            {
                case (FileViewLogSortFields.Date):
                {
                    query = filter.Sort.SortBy == SortBy.Ascending
                        ? query.OrderBy(f => f.CreatedDate)
                        : query.OrderByDescending(f => f.CreatedDate);
                    break;
                }
                case (FileViewLogSortFields.Case):
                {
                    query = filter.Sort.SortBy == SortBy.Ascending
                        ? query.OrderBy(f => f.Case.CaseNumber)
                        : query.OrderByDescending(f => f.Case.CaseNumber);
                    break;
                }
                case (FileViewLogSortFields.File):
                {
                    query = filter.Sort.SortBy == SortBy.Ascending
                        ? query.OrderBy(f => f.FileName)
                        : query.OrderByDescending(f => f.FileName);
                    break;
                }
                case (FileViewLogSortFields.Path):
                {
                    query = filter.Sort.SortBy == SortBy.Ascending
                        ? query.OrderBy(f => f.FilePath)
                        : query.OrderByDescending(f => f.FilePath);
                    break;
                }
                case (FileViewLogSortFields.ProductArea):
                {
                    query = filter.Sort.SortBy == SortBy.Ascending
                        ? query.OrderBy(f => f.Case.ProductArea.Name)
                        : query.OrderByDescending(f => f.Case.ProductArea.Name);
                    break;
                }
                case (FileViewLogSortFields.User):
                //{
                //    query = filter.Sort.SortBy == SortBy.Ascending
                //        ? query.OrderBy(f => f.User.UserID)
                //        : query.OrderByDescending(f => f.User.UserID);
                //    break;
                //}
                case (FileViewLogSortFields.Department):
                default:
                {
                    query = filter.Sort.SortBy == SortBy.Ascending
                        ? query.OrderBy(f => f.Case.Department.DepartmentName)
                        : query.OrderByDescending(f => f.Case.Department.DepartmentName);
                    break;
                }
            }

            return query.Take(filter.AmountPerPage).Select(f => new FileViewLogListItemModel
            {
                Log = new FileViewLogModel
                {
                    Case_Id = f.Case_Id,
                    CreatedDate = f.CreatedDate,
                    FileName = f.FileName,
                    FilePath = f.FilePath,
                    FileSource = f.FileSource,
                    Id = f.Id,
                    Operation = f.Operation ?? FileViewLogOperation.Legacy,
                    User_Id = f.User_Id,
                    UserName = f.UserName
                },
                DepartmentId = f.Case.Department_Id,
                DepartmentName = f.Case.Department_Id.HasValue ? f.Case.Department.DepartmentName : "",
                ProductAreaId = f.Case.ProductArea_Id,
                ProductAreaName = f.Case.ProductArea_Id.HasValue ? f.Case.ProductArea.Name : "",
                UserName = (f.User_Id.HasValue ? f.User.UserID : f.UserName) ?? "",
                CaseNumber = f.Case.CaseNumber
            }).ToList();
        }
	}
}
