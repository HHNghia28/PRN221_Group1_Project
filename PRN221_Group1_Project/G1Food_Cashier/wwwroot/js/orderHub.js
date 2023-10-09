// Tạo kết nối tới hub SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("wss://localhost:44303/orderHub") // Điều này dựa trên URL bạn đã cấu hình cho hub trong Startup.cs
    .build();

// Khi kết nối thành công
connection.start().then(function () {
    console.log('Connected to orderHub');
}).catch(function (err) {
    console.error('Error connecting to orderHub: ' + err);
});

// Đăng ký một sự kiện từ hub
connection.on("ReceiveMessage", function (message) {
    console.log('Received message: ' + message);

    if (message === "Pending") {
        alert("Pednig")
    }
});