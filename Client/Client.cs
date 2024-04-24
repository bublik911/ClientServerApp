
using System.Net;
using System.Net.Sockets;
using System.Text;



namespace SocketClient
{

    class Program
    {
        const string IP = "127.0.0.1";
        const int Port = 11000;

        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(Port);
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

        static void SendMessageFromSocket(int port)
        {

            byte[] bytes = new byte[1024];

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IP), port);

            Socket sender = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sender.Connect(endPoint);

            Console.Write("Введите запрос: ");
            string message = Console.ReadLine();

            Console.WriteLine("Сокет соединяется с localhost:8000");
            byte[] msg = Encoding.UTF8.GetBytes(message);

            int bytesSent = sender.Send(msg);

            int bytesRec = sender.Receive(bytes);

            Console.WriteLine("\nОтвет от сервера: {0}\n\n", Encoding.UTF8.GetString(bytes, 0, bytesRec));

            if (message.IndexOf("<TheEnd>") == -1)
                SendMessageFromSocket(port);

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}