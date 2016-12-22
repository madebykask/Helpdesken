using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECT.Model.Entities;

namespace ECT.Model.Abstract
{
    public interface ITextRepository
    {
        IEnumerable<TextTranslation> GetTextTranslations(int? textType);
    }
}
