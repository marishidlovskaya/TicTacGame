namespace Tictactoe.Data
{
    public interface IUsers
    {
        public Dictionary<string, IPerson> ListOfUsers { get; set; }
    }
}
