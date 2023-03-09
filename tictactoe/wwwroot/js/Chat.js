"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/tictactoe").build();

connection.on("ConnectedUsersList", function (users) {
    $(".listOfOnlineUsers").remove();
    $(".no-online-users").remove();

    if (users.length < 2) {
        var li = document.createElement("li");
        li.style.color = "red";
        li.className = 'list-group-item d-flex justify-content-between align-items-center no-online-users';
        document.getElementById("messagesList").appendChild(li);
        li.textContent = 'No online users.';
    }

    for (var i = 0; i < users.length; i++) {
        if (connection.connectionId !== users[i].key) {
            if (users[i].value.status === 'Online') {
                var databstoggle = document.createAttribute("data-bs-toggle");
                var databstarget = document.createAttribute("data-bs-target");
                databstoggle.value = "modal";
                databstarget.value = "#messageModal";
                var li = document.createElement("li");
                var span = document.createElement("span");
                li.className = 'list-group-item d-flex justify-content-between align-items-center listOfOnlineUsers listOfOnlineUsersHover';
                span.className = 'badge bg-success rounded-pill';
                document.getElementById("messagesList").appendChild(li);
                li.textContent = `${users[i].value.name}`;
                span.textContent = `${users[i].value.status}`;
                li.appendChild(span);
                li.setAttributeNode(databstoggle);
                li.setAttributeNode(databstarget);
            }
            if (users[i].value.status === 'Playing') {
                var li = document.createElement("li");
                var span = document.createElement("span");
                li.className = 'list-group-item d-flex justify-content-between align-items-center listOfOnlineUsers';
                span.className = 'badge bg-primary rounded-pill';
                document.getElementById("messagesList").appendChild(li);
                li.textContent = `${users[i].value.name}`;
                span.textContent = `${users[i].value.status}`;
                li.appendChild(span);
            }
            if (users[i].value.status === 'Waiting') {
                var li = document.createElement("li");
                var span = document.createElement("span");
                li.className = 'list-group-item d-flex justify-content-between align-items-center listOfOnlineUsers';
                span.className = 'badge bg-secondary rounded-pill';
                document.getElementById("messagesList").appendChild(li);
                li.textContent = `${users[i].value.name}`;
                span.textContent = `${users[i].value.status}`;
                li.appendChild(span);
            }
        }
    }

    const ilEvent = document.querySelectorAll('.listOfOnlineUsers');
    for (let i = 0; i < ilEvent.length; i++) {
        ilEvent[i].addEventListener('click', function () {
            $('#messageModalLabel').show();
            $('#messageWaitingModalLabel').hide();
            $('#messageDeclineModalLabel').hide();
            $('#gameRequestButton').show();
            $("#inviteModalSpan").text(ilEvent[i].textContent.replace("Online", ""));
        });
    }
});

connection.on("Request", function (sender, receiver, typeOfGame) {
    $('#acceptModalSpan').text(sender.name);
    
    if (typeOfGame === null) {
        $("#randomMoveGame").removeClass("d-flex");
        $('#randomMoveGame').attr("hidden", true);
    }
    else {
        $("#randomMoveGame").addClass("d-flex");
        $('#randomMoveGame').attr("hidden", false);
    }
    $('#acceptModal').modal('toggle');

    
});

connection.on("Decline", function () {
    $('#acceptModal').modal('hide');
    $('#messageModalLabel').hide();
    $('#messageWaitingModalLabel').hide();
    $('#messageDeclineModalLabel').show();
    $('#gameRequestButton').hide();
});

connection.on("RequestClosed", function () {
    $('#acceptModal').modal('hide');
});

connection.on("GameInitialization", function (Player1, Player2) {

    $('#rect00').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect00').attr("id"));
    })
    $('#rect01').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect01').attr("id"));
    })
    $('#rect02').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect02').attr("id"));
    })
    $('#rect10').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect10').attr("id"));
    })
    $('#rect11').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect11').attr("id"));
    })
    $('#rect12').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect12').attr("id"));
    })
    $('#rect20').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect20').attr("id"));
    })
    $('#rect21').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect21').attr("id"));
    })
    $('#rect22').on('click', function gameMoveEvent() {
        connection.invoke("PlayingGame", Player1.connectionId, Player2.connectionId, $('#rect22').attr("id"));
    })
    window.localStorage.removeItem("reciverlocalstorage");
    window.localStorage.setItem(Player1.name, Player1.connectionId);
    window.localStorage.setItem(Player2.name, Player2.connectionId);
    window.localStorage.setItem("gameId", Player2.connectionId + Player1.connectionId);
    $('#acceptModal').modal('hide');
    $('#messageModal').modal('hide');
    $('#gameModal').modal('toggle');
});

connection.on("InitPlayer1", function (Player1, Player2) {
    if (Player1.move === true) {
        $('#PlayerMove').text(Player1.name + ', now is your move').removeClass("text-muted").addClass("animation");
        $("#PlayerName").text(Player1.name);
        $("#playerSign").text(Player1.playingSymbol);
    }
    else {
        $('#PlayerMove').text('Move of ' + Player2.name).remove("animation").addClass("text-muted");
        $("#PlayerName").text(Player1.name);
        $("#playerSign").text(Player1.playingSymbol);
    }
});
connection.on("InitPlayer2", function (Player1, Player2) {
    if (Player2.move === true) {
        $('#PlayerMove').text(Player2.name + ', now is your move').removeClass("text-muted").addClass("animation");
        $("#PlayerName").text(Player2.name);
        $("#playerSign").text(Player2.playingSymbol);
    }
    else {
        $('#PlayerMove').text('Move of ' + Player1.name).remove("animation").addClass("text-muted");
        $("#PlayerName").text(Player2.name);
        $("#playerSign").text(Player2.playingSymbol);
    }
});

connection.on("RandomMove", function (move) {
    $("#" + move[0].replace("rect", "O")).hide();
    $("#" + move[1].replace("rect", "X")).hide();
    $("#" + move[0]).hide("slow");
    $("#" + move[1]).hide("slow");
});

connection.on("switchOnGameField", function (player) {
    $("#hideField").hide();
    $('#PlayerMove').text(player.name + ', now is your move').removeClass("text-muted").addClass("animation");
});
connection.on("switchOffGameField", function (player) {
    $("#hideField").show();
    $('#PlayerMove').text('Move of ' + player.name).remove("animation").addClass("text-muted");
});

connection.on("Game", function (Player1, Player2, PlayerMove) {

    if (Player1.move === true) {
        $("#" + PlayerMove.replace("rect", "X")).hide();
        $("#" + PlayerMove).hide("slow");
    }
    if (Player2.move === true) {
        $("#" + PlayerMove.replace("rect", "O")).hide();
        $("#" + PlayerMove).hide("slow");
    }
});

connection.on("Winner", function () {
    $('#PlayerMove').attr("hidden", true);
    $('#YouWin').attr("hidden", false);
});
connection.on("Draw", function () {
    $('#PlayerMove').attr("hidden", true);
    $('#YouWin').attr("hidden", true);
    $('#Draw').attr("hidden", false);
});
connection.on("Looser", function () {
    $('#PlayerMove').attr("hidden", true);
    $('#YouWin').attr("hidden", true);
    $('#Draw').attr("hidden", true);
    $('#Looser').attr("hidden", false);
});
connection.on("Terminate", function () {
    console.log('test');
    $('#PlayerName').text('');
    $('#playerSign').text('');
    $('.svgFigure').show();
    $('.clickRect').show();
    $('.hideField').show();
    $('#PlayerMove').attr("hidden", false);
    $('#YouWin').attr("hidden", true);
    $('#Draw').attr("hidden", true);
    $('#Looser').attr("hidden", true);
    $('#gameModal').modal('hide');
    window.localStorage.clear();

});

document.getElementById("registerButton").addEventListener('click', function (event) {
    var user = document.getElementById("usernameInput").value;
    console.log(user);
    connection.start().then(function () {
        connection.invoke("CheckUserExist", user);
        connection.on("UserDoesntExist", function () {
            $(".login-form").remove();
            $(".logout").removeAttr('hidden')
            var h3 = document.createElement("h3");
            h3.className = '';
            h3.textContent = 'Welcome to the game, ' + user + '.';
            $(".nameOfUser").text(user);
            document.getElementById("welcome").appendChild(h3);
            var p = document.createElement("p");
            p.className = '';
            p.textContent = 'Online Tic-Tac-Toe is a digital two-player strategy game played on a 3x3 grid using X or O. Players take turns marking squares to get three in a row, with the winner being the first to achieve this. So, enjoy the game.';
            var pChooseUser = document.createElement("p");
            pChooseUser.className = 'welcome-to-the-game';
            document.getElementById("welcome").appendChild(p);
            document.getElementById("welcome").appendChild(pChooseUser);
            connection.invoke("AddUserToList", user);
            connection.invoke("SendOnlineUsers");

            $('#TypeOfGameChecked').on('click', function () {
                if ($('#TypeOfGameChecked').attr('checked') === 'checked') {
                    $('#TypeOfGameChecked').attr('checked', false);
                }
                else {
                    $('#TypeOfGameChecked').attr('checked', true);
                }
            })

        });
        connection.on("UserAlreadyExist", function () {
            connection.off("UserAlreadyExist");
            connection.off("UserDoesntExist");
            connection.stop().then(function () {
                $('#userAlreadyExist').attr("hidden", false);
            });
        });
    });
});

document.getElementById("logOutButton").addEventListener('click', function (event) {
    connection.invoke("DeleteUser", connection.connectionId);
    connection.stop().then(function () {
        window.location.href = '/Home';
    });
});

document.getElementById("gameRequestButton").addEventListener('click', function (event) {

    let typeOfGame = $('#TypeOfGameChecked').attr('checked');

    $('#messageModalLabel').hide();
    $('#messageWaitingModalLabel').show();
    $('#messageDeclineModalLabel').hide();
    $('#gameRequestButton').hide();
    window.localStorage.setItem("reciverlocalstorage", $("#inviteModalSpan").text());
    connection.invoke("SendRequest", $("#inviteModalSpan").text(), connection.connectionId, typeOfGame);
});

document.getElementById("declineButton").addEventListener('click', function (event) {
    connection.invoke("DeclineRequest", $("#acceptModalSpan").text(), connection.connectionId);
});

document.getElementById("closeRequestButton").addEventListener('click', function (event) {
    connection.invoke("CloseRequest", window.localStorage.getItem("reciverlocalstorage"), connection.connectionId);
});

document.getElementById("acceptButton").addEventListener('click', function (event) {
    connection.invoke("StartPlyingGame", $("#acceptModalSpan").text(), connection.connectionId);
});

document.getElementById("closeGameId").addEventListener('click', function (event) {
    $('.clickRect').off("click");
    connection.invoke("CloseGame", connection.connectionId, window.localStorage.getItem("gameId"));
});



const beforeUnloadListener = (event) => {
    event.preventDefault();
    connection.invoke("DeleteUser", connection.connectionId);
};

addEventListener('beforeunload', beforeUnloadListener, { capture: true });

var lockResolver;
if (navigator && navigator.locks && navigator.locks.request) {
    const promise = new Promise((res) => {
        lockResolver = res;
    });

    navigator.locks.request('unique_lock_name', { mode: "shared" }, () => {
        return promise;
    });
}