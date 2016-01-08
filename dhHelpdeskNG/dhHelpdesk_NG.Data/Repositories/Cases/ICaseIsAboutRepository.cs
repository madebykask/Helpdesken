using System.Collections.Generic;
namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Domain;
    
    public interface ICaseIsAboutRepository: INewRepository
    {
        int SaveCaseIsAbout(Case c, out IDictionary<string, string> errors);
        
    }
}