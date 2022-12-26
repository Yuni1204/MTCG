using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
                // Set the TcpListener on port .
                Int32 port = 10001;
                IPAddress localAddr = IPAddress.Loopback; //localhost

                Socket socket = new Socket(localAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;
                //String result = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    using TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    clientCommunication(socket, client);

                    //Byte[] buffer = new Byte[client.ReceiveBufferSize];

                    //data = null;
                    ////result = null;

                    //// Get a stream object for reading and writing
                    //NetworkStream stream = client.GetStream();

                    //int i = stream.Read(buffer, 0, client.ReceiveBufferSize);
                    //data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                    //Console.WriteLine($"Received: {data}");

                    ///*
                    //// Loop to receive all the data sent by the client.
                    //while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    //{
                    //    //Console.WriteLine("\nCONSOLEWRITELINE" + data + "END OF CONSOLEWRITELINE\n");
                    //    // Translate data bytes to a ASCII string
                    //    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    //    //
                    //    //
                    //    //
                    //    //
                    //    //
                    //    //zeile 55 Bytes alle zuerst einlesen-> in eine liste von byte arrays
                    //    result = result + data;
                    //    Console.WriteLine("Received: {0}", data);

                    //    // Process the data sent by the client.




                    //    //data = data.ToUpper();
                    //    //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    //    // Send back a response.
                    //    //stream.Write(msg, 0, msg.Length);
                    //    //Console.WriteLine("Sent: {0}", data);
                    //    //
                    //    if (i < bytes.Length) { break; }
                    //}
                    //*/
                    //RequestHandler handler = new RequestHandler();
                    //handler.ParseHttpRequest(data);

                    ////data = "HTTP/1.1 201 SUCCESSS" + Environment.NewLine;

                    //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    //// Send back a response.
                    //stream.Write(msg, 0, msg.Length);
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

        public void clientCommunication(Socket socket, TcpClient client)
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

            RequestHandler handler = new RequestHandler();
            data = handler.ParseHttpRequest(data);
            //buffer = System.Text.Encoding.ASCII.GetBytes(data);
            string test = "this is a test!";
            //data = "HTTP/1.1 201 SUCCESSS" + Environment.NewLine;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

            // Send back a response.
            stream.Write(msg, 0, msg.Length);

            //stream.Write(msg, 0, msg.Length);

        }
    }
}