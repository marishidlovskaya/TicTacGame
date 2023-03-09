namespace Tictactoe.Data
{
    public class Game : IGame
    {
        private int[,] GameField { get; set; } = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        public string? Player1ConnectionId { get; set; }
        public string? Player2ConnectionId { get; set; }
        public string? Player1PlayingSymbol { get; set; }
        public string? Player2PlayingSymbol { get; set; }
        public bool? Player1Move { get; set; }
        public bool? Player2Move { get; set; }
        public string? Move { get; set; }

        public string? TypeOfGame { get; set; }
        public bool Draw { get; }

        public string[] RandomMove()
        {
            string[] randomMove = new string[2];
            var move = new Random();

            int i = move.Next(0, 3);
            int j = move.Next(0, 3);

            GameField[i, j] = 1;

            randomMove[0] = "rect" + i.ToString() + j.ToString();

            NewGenerate:
            i = move.Next(0, 3);
            j = move.Next(0, 3);

            if (GameField[i, j] != 1)
            {
                GameField[i, j] = -1;
                randomMove[1] = "rect" + i.ToString() + j.ToString();
                return randomMove;
            }
            else
            {
                goto NewGenerate;
            }
        }

        public void SetMove()
        {
            string ij = Move.Substring(4);
            int i = Int32.Parse(ij[0].ToString());
            int j = Int32.Parse(ij[1].ToString());

            if (Player1Move == true)
            {
                if (Player1PlayingSymbol == "O")
                {
                    GameField[i, j] = 1;
                }
                else
                {
                    GameField[i, j] = -1;
                }
            }
            if (Player2Move == true)
            {
                if (Player2PlayingSymbol == "X")
                {
                    GameField[i, j] = -1;
                }
                else
                {
                    GameField[i, j] = 1;
                }
            }
        }
        public string GetGameResult()
        {
            if (GameField[0, 0] + GameField[0, 1] + GameField[0, 2] == 3)
            {
                return Player1ConnectionId;
            }
            if (GameField[0, 0] + GameField[0, 1] + GameField[0, 2] == -3)
            {
                return Player2ConnectionId;
            }
            if (GameField[1, 0] + GameField[1, 1] + GameField[1, 2] == 3)
            {
                return Player1ConnectionId;
            }
            if (GameField[1, 0] + GameField[1, 1] + GameField[1, 2] == -3)
            {
                return Player2ConnectionId;
            }
            if (GameField[2, 0] + GameField[2, 1] + GameField[2, 2] == 3)
            {
                return Player1ConnectionId;
            }
            if (GameField[2, 0] + GameField[2, 1] + GameField[2, 2] == -3)
            {
                return Player2ConnectionId;
            }
            if (GameField[0, 0] + GameField[1, 0] + GameField[2, 0] == 3)
            {
                return Player1ConnectionId;
            }
            if (GameField[0, 0] + GameField[1, 0] + GameField[2, 0] == -3)
            {
                return Player2ConnectionId;
            }
            if (GameField[0, 1] + GameField[1, 1] + GameField[2, 1] == 3)
            {
                return Player1ConnectionId;
            }
            if (GameField[0, 1] + GameField[1, 1] + GameField[2, 1] == -3)
            {
                return Player2ConnectionId;
            }
            if (GameField[0, 2] + GameField[1, 2] + GameField[2, 2] == 3)
            {
                return Player1ConnectionId;
            }
            if (GameField[0, 2] + GameField[1, 2] + GameField[2, 2] == -3)
            {
                return Player2ConnectionId;
            }
            if (GameField[0, 0] + GameField[1, 1] + GameField[2, 2] == 3)
            {
                return Player1ConnectionId;
            }
            if (GameField[0, 0] + GameField[1, 1] + GameField[2, 2] == -3)
            {
                return Player2ConnectionId;
            }
            if (GameField[0, 2] + GameField[1, 1] + GameField[2, 0] == 3)
            {
                return Player1ConnectionId;
            }
            if (GameField[0, 2] + GameField[1, 1] + GameField[2, 0] == -3)
            {
                return Player2ConnectionId;
            }
            for (int k = 0; k < GameField.GetLength(0); k++)
            {
                for (int l = 0; l < GameField.GetLength(1); l++)
                {
                    if (GameField[k, l] == 0)
                    {
                        return "";
                    }
                }
            }
            return "draw";
        }
    } 
}
