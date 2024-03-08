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

        signalRConnection.on("ChatRoom", function (chatMessage, userShortInfo, roomInfo) {

            if ((typeof roomId === 'undefined' || roomInfo.id != roomId) && userShortInfo.id != userId) {
                const toast = bootstrap.showToast({
                    header: roomInfo.title,
                    headerSmall: "Şimdi",
                    body: `<p>${chatMessage.message}</p>` +
                        "<div>" +
                        `<a href="\\Room\\Detail\\${roomInfo.id}" class='btn btn-primary me-1 btn-sm'>Chate git</a>` +
                        "<button class='btn btn-secondary btn-sm' data-bs-dismiss='toast'>Kapat</button>" +
                        "</div>",
                    delay: 5000
                })
                const newMessageAudio = new Audio('/audio/new_message.mp3');
                newMessageAudio.play();
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

        if (document.getElementById('newMessageInput') != null) {
            document.getElementById('newMessageInput').addEventListener('keydown', function (e) {
                if (e.key === 'Enter') {
                    sendMessage();
                }
            });
        }
        if (document.getElementById('sendMessageBtn') != null) {
            $SendMessageBtn.click(sendMessage);
        }

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


