namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IFaqFileRepository : IRepository<FAQFile>
    {
        bool FileExists(int faqId, string fileName);

        void DeleteByFaqId(int faqId);

        List<string> FindFileNamesByFaqId(int faqId);

        List<FaqFileOverview> FindFileOverviewsByFaqIds(List<int> faqIds); 

        byte[] GetFileContentByFaqIdAndFileName(int faqId, string fileName);

        void AddFile(NewFaqFile newFaqFile);

        void AddFiles(List<NewFaqFile> newFaqFiles);

        void DeleteByFaqIdAndFileName(int faqId, string fileName);
    }
}