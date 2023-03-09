namespace Tictactoe.Data
{
    public interface IGames
    {
        public Dictionary<string, IGame> ListOfGames { get; set; }
    }
}
