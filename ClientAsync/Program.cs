using System.Net;
using System.Net.Sockets;
using System.Text;


const string IP = "127.0.0.1";
const int Port = 11000;

while (true)
{
    try
    {
        byte[] bytes = new byte[1024];

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IP), Port);

        Socket sender = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        await sender.ConnectAsync(endPoint);

        Console.Write("Введите запрос: ");
        string message = Console.ReadLine();

        Console.WriteLine("Сокет соединяется с localhost:8000");
        byte[] msg = Encoding.UTF8.GetBytes(message);

        int bytesSent = await sender.SendAsync(msg, SocketFlags.None);


        int bytesRec = await sender.ReceiveAsync(bytes, SocketFlags.None);

        Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
    finally
    {
        Console.ReadLine();
    }
}