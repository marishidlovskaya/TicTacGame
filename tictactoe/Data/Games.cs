namespace Tictactoe.Data
{
    public class Games : IGames
    {
        public Dictionary<string, IGame> ListOfGames { get; set; } = new();
    }
}
