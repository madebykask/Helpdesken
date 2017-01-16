namespace DH.Helpdesk.Web.Models.Login
{
    public class UserStatisticsModel
    {
        public UserStatisticsModel(int numberOfUsers)
        {
            NumberOfUsers = numberOfUsers;
        }

        public int NumberOfUsers { get; set; }

    }
}