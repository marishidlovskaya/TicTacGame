namespace Tictactoe.Data
{
    public interface IGame
    {
        public string? Player1ConnectionId { get; set; }
        public string? Player2ConnectionId { get; set; }
        public string? Player1PlayingSymbol { get; set; }
        public string? Player2PlayingSymbol { get; set; }
        public bool? Player1Move { get; set; }
        public bool? Player2Move { get; set; }
        public string? Move { get; set; }
        public string? TypeOfGame { get; set; }
        public string[] RandomMove();
        public void SetMove();
        public string GetGameResult();
    }
}
