﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MTCG.Server
{
    internal class RequestHandler
    {
        public void ParseHttpRequest(string data)
        {
            var request = data.Split('\n');
            int rlen = request.Length;
            //Console.WriteLine(request[0]);
            var httpinfo = request[0].Split(' ');
            switch (httpinfo[1])
            {
                case "/users": //get userinfo, check if already exists, if not add to db
                    //Console.WriteLine("\n\nHTTPINFO: " + httpinfo[1] + "\n\n");
                    dynamic something = JsonObject.Parse(request[rlen - 1]);
                    Console.WriteLine(something["Username"]);
                    Console.WriteLine(something["Password"]);
                    break;
            }
            //Console.WriteLine(request[rlen-1]);
            //foreach (var line in request)
            //{

            //    Console.WriteLine(line);
            //}
        }
    }
}