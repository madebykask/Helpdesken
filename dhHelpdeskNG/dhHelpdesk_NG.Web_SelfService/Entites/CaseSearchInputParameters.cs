using DH.Helpdesk.SelfService.Infrastructure;
using DH.Helpdesk.SelfService.Models;

namespace DH.Helpdesk.SelfService.Entites
{
    public class CaseSearchInputParameters
    {
        public int CustomerId { get; set; }
        public int LanguageId { get; set; }
        public string IdentityUser { get; set; }
        public string ProgressId { get; set; }
        public string PharasSearch { get; set; }
        public int MaxRecords { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }

        public static CaseSearchInputParameters Create(CaseSearchInputModel m)
        {
            return new CaseSearchInputParameters
            {
                CustomerId = m.CustomerId,
                //LanguageId = m.LanguageId,
                //IdentityUser = m.UserId,
                ProgressId = m.ProgressId,
                PharasSearch = m.PharasSearch,
                //MaxRecords = m.MaxRecords,
                SortBy = m.SortBy,
                SortAscending = m.SortOrder == (int)Enums.SortOrder.Asc
            };
        }
    }
}