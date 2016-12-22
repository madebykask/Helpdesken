using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.EForm.Model.Entities;

namespace DH.Helpdesk.EForm.Model.Abstract
{
    public interface ITextRepository
    {
        IEnumerable<TextTranslation> GetTextTranslations(int? textType);
    }
}
