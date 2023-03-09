namespace Tictactoe.Data
{
    public interface IPerson
    {
        public string? Status { get; set; }
        public string? Name { get; set; }
        public int? Score { get; set; }
        public bool? Move { get; set; }
        public string? ConnectionId { get; set; }
        public string? PlayingSymbol { get; set; }
    }
}