using System;
using System.Collections.Generic;

namespace DH.Helpdesk.SelfService.Models.FAQ
{
    public class FAQIndexViewModel
    {        
        public FAQIndexViewModel()
        {
            FAQCategories = new List<FAQCategoryViewModel>();
        }

        public IList<FAQCategoryViewModel> FAQCategories { get; set; }
    }

    public sealed class FAQCategoryViewModel
    {
        public FAQCategoryViewModel()
        {
            FaqRows = new List<FAQRowModel>();
            SubCategories = new List<FAQCategoryViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<FAQRowModel> FaqRows { get; set; }

        public IList<FAQCategoryViewModel> SubCategories { get; set; }

    }
    
    public sealed class FAQRowModel
    {
        public FAQRowModel()
        {
            Files = new List<FAQFileModel>();
        }

        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public string InternalAnswer { get; set; }

        public string Url1 { get; set; }

        public string Url2 { get; set; }

        public DateTime CreatedDate { get; set; }

        public IList<FAQFileModel> Files { get; set; }

    }

    public sealed class FAQFileModel
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}