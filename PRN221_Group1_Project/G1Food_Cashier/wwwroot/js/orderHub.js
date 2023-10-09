var socket = new WebSocket("wss://localhost:44303/orderHub");

// Xử lý sự kiện khi kết nối được thiết lập
socket.onopen = function (event) {
    console.log("Kết nối WebSocket đã được thiết lập.");
    sendProtocolVersion();
};

function sendProtocolVersion() {
    var data = {
        protocol: "json",
        version: 1
    };

    var jsonMessage = JSON.stringify(data);

    socket.send(jsonMessage);
}


// Xử lý sự kiện khi nhận được dữ liệu từ máy chủ
socket.onmessage = function (event) {
    var receivedData = event.data;

    // Kiểm tra xem dữ liệu đã được gửi thành công
    if (receivedData === "Message sent successfully") {
        console.log("Thông báo đã được gửi thành công đến hub.");
    } else {
        console.log("Nhận dữ liệu từ máy chủ: " + receivedData);
    }
};

// Xử lý sự kiện khi kết nối đóng
socket.onclose = function (event) {
    if (event.wasClean) {
        console.log("Kết nối đã đóng sạch, mã đóng: " + event.code + " - " + event.reason);
    } else {
        console.error("Kết nối bị lỗi.");
    }
};

// Xử lý sự kiện khi xảy ra lỗi
socket.onerror = function (error) {
    console.error("Lỗi kết nối: " + error.message);
};

// Gửi dữ liệu đến máy chủ thông qua kết nối WebSocket
function sendMessage(message) {
    socket.send(message);
}

// Đóng kết nối WebSocket khi không cần sử dụng nữa
function closeWebSocket() {
    socket.close();
}
