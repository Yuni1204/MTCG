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

        public string HttpResp201()
        {
            return $"""
                    HTTP/1.1 201 User successfully created {Environment.NewLine}
                    
                    """;
                //Content-Type: application/json {Environment.NewLine}
                //Content-Length: 0 {Environment.NewLine}
        }

        public string testmethod()
        {
            return this.test;
        }
    }
}
