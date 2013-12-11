namespace dhHelpdesk_NG.Data.Repositories.Faq
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Input;
    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface IFaqFileRepository : IRepository<FAQFile>
    {
        bool FileExists(int faqId, string fileName);

        void DeleteByFaqId(int faqId);

        List<string> FindFileNamesByFaqId(int faqId);

        List<FaqFileOverview> FindFileOverviewsByFaqIds(List<int> faqIds); 

        byte[] GetFileContentByFaqIdAndFileName(int faqId, string fileName);

        void AddFile(NewFaqFileDto newFaqFile);

        void AddFiles(List<NewFaqFileDto> newFaqFiles);

        void DeleteByFaqIdAndFileName(int faqId, string fileName);
    }
}