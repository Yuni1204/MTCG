using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MTCG.DB;
using MTCG.Server;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace MTCG
{
    class MyTcpListener
    {
        public void StartServer()
        {
            TcpListener server = null;
            try
            {
                if (!(new DataBase().ResetTables()))
                {
                    throw new Exception("Truncate failed");
                }

                // Set the TcpListener on port .
                Int32 port = 10001;
                IPAddress localAddr = IPAddress.Loopback; //localhost

                //Socket socket = new Socket(localAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                //String data = null;
                //String result = null;

                RequestHandler handler = new RequestHandler();
                handler.LoginHighEloGamer(); //for unique feature test

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = new TcpClient();
                    client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Thread thr = new Thread(() => clientCommunication(client, handler));
                    thr.Start();
                    //ThreadPool.QueueUserWorkItem((c) => clientCommunication(client, handler));
                    //ThreadPool.QueueUserWorkItem((c) => clientCommunication(client, handler));
                    //clientCommunication(client, handler);


                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        public void clientCommunication(TcpClient client, RequestHandler handler)
        {
            string data = null;
            Byte[] buffer = new Byte[client.ReceiveBufferSize];

            data = null;
            //result = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            int i = stream.Read(buffer, 0, client.ReceiveBufferSize);
            data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
            Console.WriteLine($"Received: {data}");

            
            data = handler.ParseHttpRequest(data);
            //buffer = System.Text.Encoding.ASCII.GetBytes(data);
            //string test = "this is a test!";
            //data = "HTTP/1.1 201 SUCCESSS" + Environment.NewLine;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

            // Send back a response.
            stream.Write(msg, 0, msg.Length);

            //stream.Write(msg, 0, msg.Length);

        }
    }
}