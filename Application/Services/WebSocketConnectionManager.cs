using Fleck;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

namespace Application.Services;

public class WebSocketConnectionManager
{
    private readonly Dictionary<int, List<WebSocket>> _connections = new();

    private static WebSocketConnectionManager? _instance;
    public static WebSocketConnectionManager Instance => _instance ??= new WebSocketConnectionManager();

    public void AddConnection(int userId, WebSocket socket)
    {
        if (!_connections.ContainsKey(userId)) _connections[userId] = new List<WebSocket>();
        _connections[userId].Add(socket);
    }

    public void RemoveConnection(int userId, WebSocket socket)
    {
        if (_connections.TryGetValue(userId, out var list))
        {
            list.Remove(socket);
            if (list.Count == 0) _connections.Remove(userId);
        }
    }

    public async Task SendToUserAsync(string message, int fromUserId)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
        if (dict == null || !dict.TryGetValue("toUserId", out var toUser)) return;

        if (int.TryParse(toUser, out var toUserId) && _connections.TryGetValue(toUserId, out var sockets))
        {
            var json = JsonSerializer.Serialize(new { fromUserId, text = dict["text"] });
            foreach (var sock in sockets.ToList())
            {
                if (sock.State == WebSocketState.Open)
                {
                    var bytes = Encoding.UTF8.GetBytes(json);
                    await sock.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
