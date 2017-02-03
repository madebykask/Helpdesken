namespace DH.Helpdesk.Web.Models.Login
{
    public class UserStatisticsModel
    {
        public UserStatisticsModel(string numberOfUsers)
        {
            NumberOfUsers = numberOfUsers;
        }

        public string NumberOfUsers { get; set; }

    }
}