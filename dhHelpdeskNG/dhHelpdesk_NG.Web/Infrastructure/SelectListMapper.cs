namespace DH.Helpdesk.Web.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    
    public static class SelectListMapper
    {
        public static SelectList MapToSelectList(
            this IEnumerable<User> userCollection,
            Setting customerSettings,
            bool addEmpty = false)
        {
            List<IdName> options;
            if (customerSettings.IsUserFirstLastNameRepresentation == 0)
            {
                options =
                    userCollection.OrderBy(it => it.SurName)
                        .ThenBy(it => it.FirstName)
                        .Select(
                            it => new IdName() { Id = it.Id, Name = string.Format("{0} {1}", it.SurName, it.FirstName) })
                        .ToList();
            }
            else
            {
                options =
                    userCollection.OrderBy(it => it.FirstName)
                        .ThenBy(it => it.SurName)
                        .Select(
                            it => new IdName() { Id = it.Id, Name = string.Format("{0} {1}", it.FirstName, it.SurName) })
                        .ToList();
            }

            if (addEmpty)
            {
                options.Insert(0, new IdName() { Id = 0, Name = string.Empty });
            }

            return new SelectList(options, "Id", "Name");
        }

        public static SelectList MapToSelectList(
           this IEnumerable<UserLists> userCollection,
           Setting customerSettings,
           bool addEmpty = false)
        {
            List<IdName> options;
            if (customerSettings.IsUserFirstLastNameRepresentation == 0)
            {
                options =
                    userCollection.OrderBy(it => it.LastName)
                        .ThenBy(it => it.FirstName)
                        .Select(
                            it => new IdName() { Id = it.Id, Name = string.Format("{0} {1}", it.LastName, it.FirstName) })
                        .ToList();
            }
            else
            {
                options =
                    userCollection.OrderBy(it => it.FirstName)
                        .ThenBy(it => it.LastName)
                        .Select(
                            it => new IdName() { Id = it.Id, Name = string.Format("{0} {1}", it.FirstName, it.LastName) })
                        .ToList();
            }

            if (addEmpty)
            {
                options.Insert(0, new IdName() { Id = 0, Name = string.Empty });
            }

            return new SelectList(options, "Id", "Name");
        }

        private class IdName
        {
            public int Id { get; set; }
            
            public string Name { get; set; }
        }
    }
}