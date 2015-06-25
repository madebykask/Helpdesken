namespace DH.Helpdesk.Web.Enums
{
    public enum UserFirstLastNameModes
    {  
        LastFirstNameMode = 0,
        FirstLastNameMode = 1
    }

    public static class UserFirstLastNameModesEnumExtension
    {
        /// <summary>
        /// Item is (DH.Helpdesk.Domain) Settings.IsUserFirstLastNameRepresentation
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static UserFirstLastNameModes AsUserFirstLastNameMode(this int item)
        {
            if (item == 1)
            {
                return UserFirstLastNameModes.FirstLastNameMode;
            }

            return UserFirstLastNameModes.LastFirstNameMode;
        }
    }
}