//using System.Web;
//using Fleck;
//using sepending.Application.Services;
//using System.Linq;
//using Application.Services;
//namespace Application.WebSockets;

//public class WebSocketHandler
//{
//     private readonly WebSocketConnectionManager _connectionManager;

//    public WebSocketHandler(WebSocketConnectionManager connectionManager)
//    {
//        _connectionManager = connectionManager;
//    }

//    public void StartServer(string url)
//    {
//        var server = new WebSocketServer(url);

//        server.Start(connection =>
//        {
//            int? userId = null;

//            connection.OnOpen = () =>
//            {
//                var path = connection.ConnectionInfo.Path; // "/chat?userId=123"
//                var queryIndex = path.IndexOf("?");

//                if (queryIndex >= 0)
//                {
//                    var query = path.Substring(queryIndex + 1); // "userId=123"
//                    var queryParams = HttpUtility.ParseQueryString(query);

//                    if (int.TryParse(queryParams["userId"], out var uid))
//                    {
//                        userId = uid;
//                        Console.WriteLine($"User {userId} connected!");
//                        _connectionManager.AddConnection(userId.Value, connection);
//                    }
//                }

//                if (userId == null)
//                {
//                    Console.WriteLine("Connection rejected: missing userId");
//                    connection.Close();
//                }
//            };

//            connection.OnClose = () =>
//            {
//                if (userId != null)
//                {
//                    Console.WriteLine($"User {userId} disconnected!");
//                    _connectionManager.RemoveConnection(userId.Value, connection);
//                }
//            };

//            connection.OnMessage = message =>
//            {
//                Console.WriteLine($"Received from {userId}: {message}");

//                try
//                {
//                    var obj = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(message);

//                    if (obj != null &&
//                        obj.TryGetValue("toUserId", out var toUser) &&
//                        int.TryParse(toUser, out var toUserId))
//                    {
//                        var receiverConn = _connectionManager.GetConnections(toUserId);
//                        if (receiverConn.Count() != 0)
//                        {
//                            // ✅ Gửi JSON về FE
//                            var response = new
//                            {
//                                fromUserId = userId,
//                                text = obj["text"]
//                            };
//                            Console.WriteLine(response);

//                            var json = System.Text.Json.JsonSerializer.Serialize(response);
//                            foreach (var receiver in receiverConn)
//                            {
//                                if (receiver.IsAvailable)
//                                    receiver.Send(json);
//                            }
                            
//                            // ✅ Báo cho sender là đã gửi thành công
//                            var ack = new
//                            {
//                                status = "sent",
//                                toUserId = toUserId,
//                                text = obj["text"]
//                            };
//                            connection.Send(System.Text.Json.JsonSerializer.Serialize(ack));
//                        }
//                        else
//                        {
//                            var offlineMsg = new
//                            {
//                                fromUserId = 0,
//                                text = $"User {toUserId} is offline."
//                            };
//                            Console.WriteLine(offlineMsg);
//                            connection.Send(System.Text.Json.JsonSerializer.Serialize(offlineMsg));
                            
//                            var ack = new
//                            {
//                                status = "failed",
//                                toUserId = toUserId,
//                                text = obj["text"]
//                            };
//                            connection.Send(System.Text.Json.JsonSerializer.Serialize(ack));
//                        }
//                    }
//                    else
//                    {
//                        var invalidMsg = new
//                        {
//                            fromUserId = 0,
//                            text = "Invalid format"
//                        };
//                        connection.Send(System.Text.Json.JsonSerializer.Serialize(invalidMsg));
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Error parsing message: {ex.Message}");
//                    var errorMsg = new
//                    {
//                        fromUserId = 0,
//                        text = $"Invalid message format: {message}"
//                    };
//                    connection.Send(System.Text.Json.JsonSerializer.Serialize(errorMsg));
//                }
//            };

//        });
//    }
//}