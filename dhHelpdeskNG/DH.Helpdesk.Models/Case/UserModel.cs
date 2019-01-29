namespace DH.Helpdesk.Models.Case
{
    public class UserModel
    {
        public int Id { get; }
        public string Name { get; }

        public UserModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}