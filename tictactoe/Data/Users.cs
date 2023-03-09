namespace Tictactoe.Data
{
    public class Users : IUsers
    {
        public Dictionary<string, IPerson> ListOfUsers { get; set; } = new();
    }
}
