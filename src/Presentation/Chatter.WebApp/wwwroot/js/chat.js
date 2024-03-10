// var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

$(function () {
    let $SendMessageBtn = $("#sendMessageBtn");
    let $MessageBox = $("#message-list");
    let $MessageBody = $("#message-body");
    let $UserProfileDiv = $(".user-profile ");
    let $NewMessageInput = $("#newMessageInput");
    let $notificationBell = $("#notification-bell");
    let $ChatList = $("#chat-list");
    
    function scrollToBottom() {

        document.documentElement.scrollTop = document.documentElement.scrollHeight;
        $MessageBody.animate({scrollTop: $MessageBody[0].scrollHeight}, 1000);
    }

    window.onload = scrollToBottom;

    var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    signalRConnection.on("ChatRoom", function (chatMessage, userShortInfo, roomInfo) {
        chatMessage.userInfo = userShortInfo;
        roomInfo.lastChatMessage = chatMessage;
        updateChatList(roomInfo);
        
        if ((typeof roomId === 'undefined' || roomInfo.id != roomId) && userShortInfo.id != userId) {
            const toast = bootstrap.showToast({
                header: roomInfo.title,
                headerSmall: "Şimdi",
                closeButtonClass: "btn-close-white",
                toastClass: "text-bg-secondary",
                
                body: `<p>${chatMessage.message}</p>` + "<div>" + `<a href="\\Room\\Chat\\${roomInfo.id}" class='btn btn-primary me-1 btn-sm'>Chate git</a>` + "<button class='btn btn-secondary btn-sm' data-bs-dismiss='toast'>Kapat</button>" + "</div>",
                delay: 5000
            })
           
            const newMessageAudio = new Audio('/audio/new_message.mp3');
            newMessageAudio.play();
            return;
        }

        let date = new Date(chatMessage.sentDate);
        if (userShortInfo.id != userId) {
            $MessageBox.append(`<div class="message received-message">
                                <span class="message username">${userShortInfo.userName}</span>
                                 <div class="message-content">
                                    <p class="message-text">${chatMessage.message}</p>
                                    <small class="message-date">${date.getHours()}:${date.getMinutes()}</small>
                                </div>
                            </div>`);
            const audio = new Audio('/audio/received_message.mp3');
            audio.play();
        } else if (roomInfo.id == roomId) {
            $MessageBox.append(`<div class="message sent-message">
                                <span class="message username">${userShortInfo.userName}</span>
                                    <div class="message-content">
                                        <p class="message-text">${chatMessage.message}</p>
                                        <small class="message-date">${date.getHours()}:${date.getMinutes()}</small>
                                    </div>
                                </div>`);

        }
        scrollToBottom();
    });

    signalRConnection.on("UserConnected", function (userIds, otherRoomInfo) {
        userIds.forEach(userId => {
            setOnline(userId);
        });
        if (otherRoomInfo != null) {
            addOtherChatInfo(otherRoomInfo);
        }
    });

    signalRConnection.on("UserDisconnected", function (userIds) {
        userIds.forEach(userId => {
            setOffline(userId);
        });
    });

    function addOtherChatInfo(otherRoomInfo) {
        var chatInfo = document.getElementById(`chat-info-${otherRoomInfo.id}`);
        if (chatInfo != null) {
            return;
        }
        $ChatList.append(`<div class="chat-info" id="chat-info-${otherRoomInfo.id}">
                                <a href="\\Room\\Chat\\${otherRoomInfo.id}">
                <div class="d-flex justify-content-between ">
                    <h4 class="card-title" style="margin:0">
                        <b style="">${otherRoomInfo.title}</b>
                    </h4>
                    <span class="message-date" style="position:relative; opacity: 0.6; padding-right:20px" >${new Date(otherRoomInfo.lastChatMessage.sentDate).getHours()}:${new Date(otherRoomInfo.lastChatMessage.sentDate).getMinutes().toString().padStart(2, '0')}</span>
                </div>
                <div>
                    <span>${otherRoomInfo.lastChatMessage.userInfo.userName}: ${otherRoomInfo.lastChatMessage.message}</span>
                </div>
            </a>
        </div>`);
    }

    function updateChatList(otherRoomInfo) {
        var chatInfo = document.getElementById(`chat-info-${otherRoomInfo.id}`);
        if (chatInfo != null) {
            chatInfo.remove();
        }
        
        $ChatList.prepend(`<div class="chat-info" id="chat-info-${otherRoomInfo.id}">
                                <a href="\\Room\\Chat\\${otherRoomInfo.id}">
                <div class="d-flex justify-content-between ">
                    <h4 class="card-title" style="margin:0">
                        <b style="">${otherRoomInfo.title}</b>
                    </h4>
                    <span class="message-date" style="position:relative; opacity: 0.6; padding-right:20px" >${new Date(otherRoomInfo.lastChatMessage.sentDate).getHours()}:${new Date(otherRoomInfo.lastChatMessage.sentDate).getMinutes().toString().padStart(2, '0')}</span>
                </div>
                <div>
                    <span>${otherRoomInfo.lastChatMessage.userInfo.userName}: ${otherRoomInfo.lastChatMessage.message}</span>
                </div>
            </a>
        </div>`);
    }

    function setOnline(userId) {
        $(`.user-profile #${userId}-icon`).css('color', 'green');

    }

    function setOffline(userId) {
        $(`.user-profile #${userId}-icon`).css('color', 'red');
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
        if (message === "") return;
        $NewMessageInput.val("");

        //2. parametrede grubun id'si g�nderilecek
        signalRConnection.invoke("SendMessage", message, roomId);
    }

    signalRConnection.start();
})


