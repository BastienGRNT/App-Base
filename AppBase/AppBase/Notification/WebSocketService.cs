using System.Net.WebSockets;
using System.Text;

namespace AppBase.Notification;

public class WebSocketService
{
    private WebSocket? _webSocket;

    public async Task HandleWebSocketAsync(WebSocket webSocket)
    {
        _webSocket = webSocket;
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Message reçu : {message}");

                var responseMessage = "Notification envoyée par le serveur";
                var responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connexion fermée", CancellationToken.None);
                _webSocket = null;
            }
        }
    }

    public async Task SendNotificationAsync(string message)
    {
        if (_webSocket is { State: WebSocketState.Open })
        {
            var responseBytes = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}