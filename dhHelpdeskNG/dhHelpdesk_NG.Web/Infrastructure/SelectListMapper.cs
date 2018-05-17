using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.BusinessData.Models.User.Interfaces;
using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.Web.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using System;
    
    public static class SelectListMapper
    {
        private static readonly string CURRENT_USER_ITEM_CAPTION = "Inloggad användare";

        #region IdName

        private class IdName
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        #endregion

        public static SelectList MapToSelectList(
            this IEnumerable<IUserCommon> userCollection,
            Setting customerSettings,
            bool addEmpty = false,
            bool addCurrentUser = false)
        {
            List<IdName> options;
            if (customerSettings.IsUserFirstLastNameRepresentation == 0)
            {
                options =
                    userCollection.OrderBy(it => it.SurName)
                        .ThenBy(it => it.FirstName)
                        .Select(
                            it => new IdName() { Id = it.Id.ToString(), Name = string.Format("{0} {1}", it.SurName, it.FirstName) })
                        .ToList();
            }
            else
            {
                options =
                    userCollection.OrderBy(it => it.FirstName)
                        .ThenBy(it => it.SurName)
                        .Select(
                            it => new IdName() { Id = it.Id.ToString(), Name = string.Format("{0} {1}", it.FirstName, it.SurName) })
                        .ToList();
            }

            if (addCurrentUser)
            {
                options.Insert(0, new IdName() { Id = "-1", Name = string.Format("-- {0} --", Translation.GetCoreTextTranslation(CURRENT_USER_ITEM_CAPTION)) });
            }

            if (addEmpty)
            {
                options.Insert(0, new IdName() { Id = string.Empty, Name = string.Empty });
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
                            it => new IdName() { Id = it.Id.ToString(), Name = string.Format("{0} {1}", it.LastName, it.FirstName) })
                        .ToList();
            }
            else
            {
                options =
                    userCollection.OrderBy(it => it.FirstName)
                        .ThenBy(it => it.LastName)
                        .Select(
                            it => new IdName() { Id = it.Id.ToString(), Name = string.Format("{0} {1}", it.FirstName, it.LastName) })
                        .ToList();
            }

            if (addEmpty)
            {
                options.Insert(0, new IdName() { Id = string.Empty, Name = string.Empty });
            }

            return new SelectList(options, "Id", "Name");
        }

        public static CustomSelectList MapToCustomSelectList(
            this IEnumerable<IUserCommon> userCollection,
            string selectedItems,
            Setting customerSettings,
            bool addEmpty = false)
        {
            CustomSelectList list = new CustomSelectList();
            if (customerSettings.IsUserFirstLastNameRepresentation == 0)
            {
                list.Items.AddItems(
                    userCollection.OrderBy(it => it.SurName)
                        .ThenBy(it => it.FirstName)
                        .Select(
                            it => new ListItem(it.Id.ToString(), string.Format("{0} {1}", it.SurName, it.FirstName), Convert.ToBoolean(it.IsActive)))
                        .ToList());
                list.SelectedItems.AddItems(selectedItems, false);
            }
            else
            {
                list.Items.AddItems(
                    userCollection.OrderBy(it => it.FirstName)
                        .ThenBy(it => it.SurName)
                        .Select(
                            it => new ListItem(it.Id.ToString(), string.Format("{0} {1}", it.FirstName, it.SurName), Convert.ToBoolean(it.IsActive)))
                        .ToList());
                list.SelectedItems.AddItems(selectedItems, false);
            }

            if (addEmpty)
            {
                list.Items.AddItem(new ListItem("0", string.Empty, true));
            }

            return list;
        }
    }
}