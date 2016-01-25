namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Projects.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.Projects;

    public class NewProjectViewModelFactory : INewProjectViewModelFactory
    {
        public NewProjectViewModel Create(SelectList users, string guid)
        {
            var items = users.Select(x => new { Value = x.Value, Name = x.Text });
            var list = new MultiSelectList(items, "Value", "Name");
            //var list = new List<SelectListItem>();
            return new NewProjectViewModel
                       {
                           ProjectEditModel = new ProjectEditModel(),
                           Users = list, //users.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = string.Format("{0} {1}", x.FirstName, x.SurName) }).ToList(),
                           Guid = guid 
                       };
        }
    }
}