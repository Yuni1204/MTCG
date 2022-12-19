using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MTCG.Server;

namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // socket from moodle
            //var serverSock = new Socket(
            //    AddressFamily.InterNetwork,
            //    SocketType.Stream,
            //    ProtocolType.Tcp
            //);
            //serverSock.Bind(new IPEndPoint(IPAddress.Loopback, 10101));
            //serverSock.Listen(5);
            //var clientSock = serverSock.Accept();
            //clientSock.Send(Encoding.ASCII.GetBytes("Hello World"));
            
            var db = new DataBase();
            //db.addUser();
            //string stringlines = "hello world\nwello horld\nline 3";
            //var rqhandler = new RequestHandler();
            //rqhandler.ParseHttpRequest(stringlines);
            MyTcpListener server = new MyTcpListener();
            server.StartServer();
            //var Game = new Game();
            //Game.Beschimpfen();
        }
    }
}
