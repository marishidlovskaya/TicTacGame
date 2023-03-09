using Microsoft.AspNetCore.SignalR;
using Tictactoe.Data;

namespace Tictactoe.Hubs
{
    public class TictactoeHub : Hub
    {
        private readonly IUsers _users;
        private readonly IPerson _person;
        private readonly IGames _games;
        private readonly IGame _game;

        public TictactoeHub(IUsers users, IPerson person, IGames games, IGame game)
        {
            _users = users;
            _person = person;
            _games = games;
            _game = game;
        }

        public Task AddUserToList(string user)
        {
            _person.Name = user;
            _person.Status = "Online";

            _users.ListOfUsers.Add(Context.ConnectionId, _person);
            return Task.CompletedTask;
        }

        public Task CheckUserExist(string user)
        {
            foreach (var item in _users.ListOfUsers.Values)
            {
                if (user == item.Name)
                {
                    Clients.Client(Context.ConnectionId).SendAsync("UserAlreadyExist");
                    return Task.CompletedTask;
                }
            }
            Clients.Client(Context.ConnectionId).SendAsync("UserDoesntExist");
            return Task.CompletedTask;
        }

        public async Task SendOnlineUsers()
        {
            await Clients.All.SendAsync("ConnectedUsersList", _users.ListOfUsers.ToArray());
        }

        public async Task DeleteUser(string userConnectionId)
        {
            _users.ListOfUsers.Remove(userConnectionId);
            await Clients.All.SendAsync("ConnectedUsersList", _users.ListOfUsers.ToArray());
        }

        public async Task SendRequest(string receiver, string sender, string typeOfGame)
        {
            _game.TypeOfGame = typeOfGame;

            string receiverid = "";

            foreach (var item in _users.ListOfUsers)
            {
                if (item.Value.Name == receiver)
                {
                    receiverid = item.Key;
                }
            }
            _games.ListOfGames.Add(sender + receiverid, _game);

            _users.ListOfUsers[sender].Status = "Waiting";
            _users.ListOfUsers[receiverid].Status = "Waiting";

            await Clients.Client(receiverid).SendAsync("Request", _users.ListOfUsers[sender], _users.ListOfUsers[receiverid], typeOfGame);
            await Clients.All.SendAsync("ConnectedUsersList", _users.ListOfUsers.ToArray());
        }

        public async Task DeclineRequest(string receiver, string sender)
        {
            string receiverid = "";

            foreach (var item in _users.ListOfUsers)
            {
                if (item.Value.Name == receiver)
                {
                    receiverid = item.Key;
                }
            }
            _users.ListOfUsers[sender].Status = "Online";

            await Clients.Clients(sender, receiverid).SendAsync("Decline");
            await Clients.All.SendAsync("ConnectedUsersList", _users.ListOfUsers.ToArray());
        }

        public async Task CloseRequest(string receiver, string sender)
        {
            if (receiver != null)
            {
                string receiverid = "";

                foreach (var item in _users.ListOfUsers)
                {
                    if (item.Value.Name == receiver)
                    {
                        receiverid = item.Key;
                    }
                }
                _users.ListOfUsers[sender].Status = "Online";
                _users.ListOfUsers[receiverid].Status = "Online";

                await Clients.Clients(receiverid).SendAsync("RequestClosed");
                await Clients.All.SendAsync("ConnectedUsersList", _users.ListOfUsers.ToArray());
            }
        }

        public async Task StartPlyingGame(string receiver, string sender)
        {
            string receiverid = "";

            foreach (var item in _users.ListOfUsers)
            {
                if (item.Value.Name == receiver)
                {
                    receiverid = item.Key;
                }
            }

            if (_users.ListOfUsers[sender].Move == null && _users.ListOfUsers[receiverid].Move == null)
            {
                _users.ListOfUsers[sender].Status = "Playing";
                _users.ListOfUsers[receiverid].Status = "Playing";

                var rand1 = new Random();
                var rand2 = new Random();
                rand1.Next(1, 10);
                rand2.Next(1, 10);
                bool firstMove = rand1.Next(1, 10) > rand2.Next(1, 10);
                _users.ListOfUsers[sender].Move = firstMove;
                _users.ListOfUsers[receiverid].Move = !firstMove;

                if (_games.ListOfGames[receiverid + sender].TypeOfGame != null)
                {
                    string[] randomMove;
                    randomMove = _games.ListOfGames[receiverid + sender].RandomMove();
                    await Clients.Clients(sender, receiverid).SendAsync("RandomMove", randomMove);
                }

                _users.ListOfUsers[receiverid].ConnectionId = receiverid;
                _users.ListOfUsers[sender].ConnectionId = sender;
                _users.ListOfUsers[receiverid].PlayingSymbol = "O";
                _users.ListOfUsers[sender].PlayingSymbol = "X";

                await Clients.Clients(sender, receiverid).SendAsync("GameInitialization", _users.ListOfUsers[sender], _users.ListOfUsers[receiverid]);
                await Clients.All.SendAsync("ConnectedUsersList", _users.ListOfUsers.ToArray());

                if (_users.ListOfUsers[sender].Move == true)
                {
                    await Clients.Client(sender).SendAsync("switchOnGameField");
                }
                if (_users.ListOfUsers[receiverid].Move == true)
                {
                    await Clients.Client(receiverid).SendAsync("switchOnGameField");
                }
            }

            await Clients.Client(sender).SendAsync("InitPlayer1", _users.ListOfUsers[sender], _users.ListOfUsers[receiverid]);
            await Clients.Client(receiverid).SendAsync("InitPlayer2", _users.ListOfUsers[sender], _users.ListOfUsers[receiverid]);
        }

        public async Task PlayingGame(string Player1, string Player2, string PlayerMove)
        {
            _users.ListOfUsers[Player1].Move = !_users.ListOfUsers[Player1].Move;
            _users.ListOfUsers[Player2].Move = !_users.ListOfUsers[Player2].Move;

            await Clients.Clients(Player1, Player2).SendAsync("Game", _users.ListOfUsers[Player1], _users.ListOfUsers[Player2], PlayerMove);

            _games.ListOfGames[Player2 + Player1].Player1PlayingSymbol = _users.ListOfUsers[Player1].PlayingSymbol;

            _games.ListOfGames[Player2 + Player1].Player2PlayingSymbol = _users.ListOfUsers[Player2].PlayingSymbol;

            _games.ListOfGames[Player2 + Player1].Player1Move = _users.ListOfUsers[Player1].Move;

            _games.ListOfGames[Player2 + Player1].Player2Move = _users.ListOfUsers[Player2].Move;

            _games.ListOfGames[Player2 + Player1].Player1ConnectionId = Player1;

            _games.ListOfGames[Player2 + Player1].Player2ConnectionId = Player2;

            _games.ListOfGames[Player2 + Player1].Move = PlayerMove;

            _games.ListOfGames[Player2 + Player1].SetMove();

            string gameRsult = _games.ListOfGames[Player2 + Player1].GetGameResult();

            if (gameRsult == "draw")
            {
                await Clients.Clients(Player1, Player2).SendAsync("Draw");
                _games.ListOfGames.Remove(Player2 + Player1);
            }

            if (gameRsult != "" && gameRsult != "draw")
            {
                await Clients.Client(gameRsult).SendAsync("Winner");
                await Clients.Client(Player2).SendAsync("switchOffGameField", _users.ListOfUsers[Player2]);
                await Clients.Client(Player1).SendAsync("switchOffGameField", _users.ListOfUsers[Player2]);

                if (gameRsult == Player1)
                {
                    await Clients.Client(Player2).SendAsync("Looser");
                }
                else
                {
                    await Clients.Client(Player1).SendAsync("Looser");
                }
                _games.ListOfGames.Remove(Player2 + Player1);
            }

            if (gameRsult == "" || gameRsult == null)
            {
                if (_users.ListOfUsers[Player1].Move == true)
                {
                    await Clients.Client(Player1).SendAsync("switchOnGameField", _users.ListOfUsers[Player1]);
                    await Clients.Client(Player2).SendAsync("switchOffGameField", _users.ListOfUsers[Player1]);
                }
                if (_users.ListOfUsers[Player2].Move == true)
                {
                    await Clients.Client(Player2).SendAsync("switchOnGameField", _users.ListOfUsers[Player2]);
                    await Clients.Client(Player1).SendAsync("switchOffGameField", _users.ListOfUsers[Player2]);
                }
            }
        }
        public async Task CloseGame(string player, string gameId)
        {
            _games.ListOfGames.Remove(gameId);
            _users.ListOfUsers[player].ConnectionId = null;
            _users.ListOfUsers[player].Move = null;
            _users.ListOfUsers[player].PlayingSymbol = null;
            _users.ListOfUsers[player].Status = "Online";
            await Clients.Client(player).SendAsync("Terminate");
            await Clients.All.SendAsync("ConnectedUsersList", _users.ListOfUsers.ToArray());
        }
    }
}