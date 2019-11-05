namespace DH.Helpdesk.Dal.Repositories.Faq
{
	using System.Collections.Generic;

	using DH.Helpdesk.BusinessData.Models.Faq.Input;
	using DH.Helpdesk.BusinessData.Models.Faq.Output;
	using DH.Helpdesk.Dal.Infrastructure;
	using DH.Helpdesk.Domain.Faq;
	using BusinessData.Models;

	public interface IFaqFileRepository : IRepository<FaqFileEntity>
    {
        bool FileExists(int faqId, string fileName);

        void DeleteByFaqId(int faqId);

        List<string> FindFileNamesByFaqId(int faqId);

        List<FaqFileOverview> FindFileOverviewsByFaqIds(List<int> faqIds); 

        FileContentModel GetFileContentByFaqIdAndFileName(int faqId, string basePath, string fileName);

        void AddFile(NewFaqFile newFaqFile);

        void AddFiles(List<NewFaqFile> newFaqFiles);

        void DeleteByFaqIdAndFileName(int faqId, string fileName);
    }
}