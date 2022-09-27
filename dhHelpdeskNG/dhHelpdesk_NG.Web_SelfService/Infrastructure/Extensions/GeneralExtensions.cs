namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.User.Input;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.SelfService.Models.Case;
    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Web.Common.Enums.Case;

    public static class GeneralExtensions
    {
        public static bool UserHasRole(UserOverview user, string roleToHave)
        {
            var bolRet = false;

            if (user != null)
                if (user.UserGroupId.ToString() == roleToHave)
                {
                    bolRet = true;
                }

            return bolRet;
        }

        public static string ReturnSelectedValueCaseSolution(int id, IEnumerable<CaseSolutionSchedule> css)
        {
            string strRet = "";

            if (css != null)
            {
                foreach (CaseSolutionSchedule s in css)
                {
                    if (s.CaseSolution_Id == id)
                    {
                        strRet = "checked";
                        break;
                    }
                }
            }

            return strRet;
        }

        public static string CaseSolution_SetSelectedScheduleDay_Day(this HtmlHelper html, string compare, CaseSolutionSchedule css)
        {
            if (css != null)
            {
                foreach (var s in css.ScheduleDay.Split(','))
                {
                    if (s == compare)
                        return " checked=\"checked\"";
                }
            }

            return "";
        }

        public static string returnMaskedPwdForCustomerSettings(int id, string ldapPassword)
        {
            if (id == 0 && ldapPassword == null || ldapPassword == "")
                return string.Empty;
            else
                return "|||||||||||";
        }

        public static IList<SelectListItem> BuildComputerCategoriesSelectList(IList<ComputerUserCategoryOverview> computerUserCategories, NewCaseModel model, bool addEmpty = false)
        {
            // Computer user categories
            model.ComputerUserCategories = computerUserCategories.Where(o => !o.IsEmpty).ToList();
            model.EmptyComputerCategoryName = computerUserCategories.FirstOrDefault(o => o.IsEmpty)?.Name;

            var computerCategoriesSelectList =
            BuildComputerCategoriesSelectListInner(model.ComputerUserCategories, model.RegardingCategoryId, model.EmptyComputerCategoryName);

            if (addEmpty)
            {
                computerCategoriesSelectList.Insert(0, new SelectListItem()
                {
                    Value = "",
                    Text = ""
                });
            }

            return computerCategoriesSelectList;
        }

        public static bool IsReadOnly(this NewCaseModel model, GlobalEnums.TranslationCaseFields caseFieldName)
        {
            //if (model.DynamicCase != null && model.FieldSettings.Select(f=> f.LockedcaseFieldName))
            //    return true;

            var isReadOnly = true;

            var isGlobalVisibility = model.FieldSettings.Select(f => f.Name).Contains(caseFieldName.ToString());
            if (!isGlobalVisibility)
            {
                return false;
            }

            return isReadOnly;
        }

        private static IList<SelectListItem> BuildComputerCategoriesSelectListInner(IList<ComputerUserCategoryOverview> categories, int? selectedCategoryId, string emptyCategoryName)
        {
            var computerCategoriesSelectList = categories.Select(o => new SelectListItem()
            {
                Text = Translation.Get(o.Name),
                Value = o.Id.ToString(),
                Selected = selectedCategoryId.HasValue && o.Id == selectedCategoryId.Value
            }).OrderBy(x => x.Text).ToList();

            computerCategoriesSelectList.Insert(0, new SelectListItem()
            {
                Text = Translation.Get(emptyCategoryName ?? ComputerUserCategory.EmptyCategoryDefaultName),
                Value = ComputerUserCategory.EmptyCategoryId.ToString(),
                Selected = selectedCategoryId == ComputerUserCategory.EmptyCategoryId //used for empty category
            });

            return computerCategoriesSelectList.ToList();
        }
    }
}