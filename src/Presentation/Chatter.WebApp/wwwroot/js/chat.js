// var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

$(function () {
        let $SendMessageBtn = $("#sendMessageBtn");
        let $MessageBox = $("#message-list");
        let $MessageBody = $("#message-body");
        let $UserProfileDiv = $(".user-profile ");
        let $NewMessageInput = $("#newMessageInput");

        let $notificationBell = $("#notification-bell");


        function scrollToBottom() {
            var container = document.getElementById("message-body");
            container.scrollTop = container.scrollHeight;
        }

        window.onload = scrollToBottom;

        var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

        signalRConnection.on("ChatRoom", function (chatMessage, userShortInfo, chatRoomId) {
            // $("#toast1").toast("show");
            var toastElList = [].slice.call(document.querySelectorAll('.toast'))
            var toastList = toastElList.map(function(toastEl) {
                return new bootstrap.Toast(toastEl)
            })
            toastList.forEach(toast => toast.show())
            if (chatRoomId != roomId) {
                const audio = new Audio('/audio/new_message.mp3');
                audio.play();
                return;
            }

            if (userShortInfo.id != userId) {
                
                $MessageBox.append(`<div class="message received-message">
                                <span class="message username">${userShortInfo.userName}</span>
                                <p>${chatMessage.message}</p>
                            </div>`);
                const audio = new Audio('/audio/received_message.mp3');
                audio.play();
            } else {
                $MessageBox.append(`<div class="message sent-message">
                                <span class="message username">${userShortInfo.userName}</span>
                                <p>${chatMessage.message}</p>
                            </div>`);

            }
            scrollToBottom();
        });

        signalRConnection.on("UserConnected", function (userIds) {
            userIds.forEach(userId => {
                setOnline(userId);
            });
        });

        signalRConnection.on("UserDisconnected", function (userIds) {
            userIds.forEach(userId => {
                setOffline(userId);
            });
        });

        function setOnline(userId) {
            $(`.user-profile #${userId}`).css('color', 'green');

        }

        function setOffline(userId) {
            $(`.user-profile #${userId}`).css('color', 'red');

        }

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


