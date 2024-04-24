using ClassLib;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

const string IP = "127.0.0.1";
const int Port = 11000;

IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IP), Port);


using Socket listener = new(
    endPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);


User[] users = new[] {new User("Egor", 19),
                      new User("Vadim", 20),
                      new User("Bebrick", 52) };

try
{
    listener.Bind(endPoint);
    listener.Listen(100);
    Console.WriteLine("Сервер включен");
    while (true)
    {
        Console.WriteLine($"Ждем кого-нибудь на порт {Port}");
        var handler = await listener.AcceptAsync();
        Console.WriteLine("Клиент подключен");

        string data = null;

        byte[] bytes = new byte[1024];
        int bytesRec = await handler.ReceiveAsync(bytes, SocketFlags.None);

        if (bytesRec > 0)
        {
            data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

            Console.WriteLine("Сообщение получено");

            switch (data)
            {
                case "get/name=Egor":
                    Stopwatch timer = new Stopwatch();
                    string json = JsonConvert.SerializeObject(users[0]);
                    byte[] response = Encoding.UTF8.GetBytes(json);
                    await handler.SendAsync(response, SocketFlags.None);
                    Console.WriteLine($"Ответ отправлен. Время выполнения: {timer.Elapsed.TotalMilliseconds}");
                    break;
                case "get/name=Vadim":
                    Stopwatch timer1 = new Stopwatch();
                    json = JsonConvert.SerializeObject(users[1]);
                    response = Encoding.UTF8.GetBytes(json);
                    await handler.SendAsync(response, SocketFlags.None);
                    Console.WriteLine($"Ответ отправлен. Время выполнения: {timer1.Elapsed.TotalMilliseconds}");
                    break;
                case "get/name=Bebrick":
                    Stopwatch timer2 = new Stopwatch();
                    json = JsonConvert.SerializeObject(users[2]);
                    response = Encoding.UTF8.GetBytes(json);
                    await handler.SendAsync(response, SocketFlags.None);
                    Console.WriteLine($"Ответ отправлен. Время выполнения: {timer2.Elapsed.TotalMilliseconds}");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Клиент завершил соединение с сервером.");
            break;
        }

        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
        Console.WriteLine("Сервер завершил соединение с клиентом.");
    }
}
catch (Exception err)
{
    Console.WriteLine(err.ToString());
}
finally
{
    listener.Dispose();
    Console.WriteLine("Socket closed");
}


