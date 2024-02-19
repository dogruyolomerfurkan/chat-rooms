// var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

$(function () {
        let $SendMessageBtn = $("#sendMessageBtn");
        let $MessageBox = $("#message-list");
        let $MessageBody = $("#message-body");
        let $NewMessageInput = $("#newMessageInput");

        let $notificationBell = $("#notification-bell");


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
            } else {
                $MessageBox.append(`<div class="message sent-message">
                                <span class="message username">${userShortInfo.userName}</span>
                                <p>${chatMessage.message}</p>
                            </div>`);

            }
            scrollToBottom();
        });

        document.getElementById('newMessageInput').addEventListener('keydown', function (e) {
            if (e.key === 'Enter') {
                sendMessage();
            }
        });

        $SendMessageBtn.click(sendMessage);

        function sendMessage() {

            var message = $NewMessageInput.val();
            if (message === "")
                return;
            $NewMessageInput.val("");

            //2. parametrede grubun id'si g�nderilecek
            signalRConnection.invoke("SendMessage", message, roomId);
        }

        signalRConnection.start();


    }
)


