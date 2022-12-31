using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Server
{
    internal class HttpResponse
    {
        public const string json = "{\"Username\": \"Ninja\", \"Password\": \"Tutel\"}";
        private string test = $"""
            HTTP/1.1 200 OK {Environment.NewLine}
            Content-Type: application/json {Environment.NewLine}
            Content-Length: {json.Length} {Environment.NewLine}
            {Environment.NewLine}
            {json} {Environment.NewLine}

            """;

        public string UserCreate201()
        {
            return $"""
                    HTTP/1.1 201 User successfully created {Environment.NewLine}
                    User successfully created{Environment.NewLine}
                    """;
                //Content-Type: application/json {Environment.NewLine}
                //Content-Length: 0 {Environment.NewLine}
        }

        public string UserCreate409()
        {
            return $"""
                    HTTP/1.1 409 User with same username already registered {Environment.NewLine}
                    
                    """;
            
        }

        public string Session200()
        {
            return $"""
                    HTTP/1.1 200 User login successful {Environment.NewLine}
                    
                    """;
        }

        public string Session401()
        {
            return $"""
                    HTTP/1.1 401 Invalid username/password provided {Environment.NewLine}
                    
                    """;
        }

        public string testmethod()
        {
            return this.test;
        }
    }
}
