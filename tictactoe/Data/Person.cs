namespace Tictactoe.Data
{
    public class Person : IPerson
    {
        public string? Status { get; set; }
        public string? Name { get; set; }
        public int? Score { get; set; }
        public bool? Move { get; set; } = null;
        public string? ConnectionId { get; set; }
        public string? PlayingSymbol { get; set; }
    }
}
