using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Web;

namespace Win7.Services
{
	public class WebSocketServer
	{
		public WebSocketServer()
		{
			Start();
		}

		async void Start()
		{
			var listener = new HttpListener();
			listener.Prefixes.Add("http://localhost:8080/");
			listener.Start();

			while (true)
			{
				var context = await listener.GetContextAsync();
				Console.WriteLine("connected");

				var websocketContext = await context.AcceptWebSocketAsync(null);
				ProcessClient(websocketContext.WebSocket);
			}
		}

		async void ProcessClient(WebSocket websocket)
		{
			var data = new byte[1500];
			var buffer = new ArraySegment<byte>(data);

			while (true)
			{
				var result = await websocket.ReceiveAsync(buffer, CancellationToken.None);

				if (result.CloseStatus != null)
				{
					Console.WriteLine("socket closed");
					websocket.Abort();
					return;
				}

				Console.WriteLine(">>> " + Encoding.UTF8.GetString(data, 0, result.Count));
				await websocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
			}
		}
	}
}