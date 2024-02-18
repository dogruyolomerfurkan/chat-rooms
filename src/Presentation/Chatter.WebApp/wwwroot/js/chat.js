$(function () {

    var $SendMessageBtn = $("#sendMessageBtn");
    var $MessageBox = $("#message-list");
    var $MessageBody = $("#message-body");
    var $NewMessageInput = $("#newMessageInput");

    var $notificationBell = $("#notification-bell");


    function scrollToBottom() {
        var container = document.getElementById("message-body");
        container.scrollTop = container.scrollHeight;
        console.log("Aşağı kaydırdım");
    }
    window.onload = scrollToBottom;

    var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    signalRConnection.on("ChatRoom", function (chatMessage, userShortInfo, senderConnectionId) {
        var connectionId = signalRConnection.connectionId;
        console.log(connectionId);
        $notificationBell.css('color', 'red');
        if (connectionId != senderConnectionId) {
            $MessageBox.append(`<div class="message received-message">
                                <span class="message username">${userShortInfo.userName}</span>
                                <p>${chatMessage.message}</p>
                            </div>`);            
        }
        else {
            $MessageBox.append(`<div class="message sent-message">
                                <span class="message username">${userShortInfo.userName}</span>
                                <p>${chatMessage.message}</p>
                            </div>`);
            
        }
        scrollToBottom();
    });

    $SendMessageBtn.click(function sendMessageToChatRoom(roomId) {

        var message = $NewMessageInput.val();
        console.log(message);
        $NewMessageInput.val("");
        
        //2. parametrede grubun id'si g�nderilecek
        signalRConnection.invoke("SendMessage", message, 7);
    });


    signalRConnection.start().then(function () {
        signalRConnection.invoke("GetMessages", 7)
            .then(function (result) {
                console.log("Success");
                for (var i = 0; i < result.length; i++) {
                    console.log(result[i]);
                }
            })
    }).catch(function (err) {
        return console.error(err.toString());
    });


})