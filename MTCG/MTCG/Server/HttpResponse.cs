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
            string content = $"{Environment.NewLine}{Environment.NewLine}User successfully created";
            return $"""
                    HTTP/1.1 201 User successfully created
                    Content-Type: text/plain
                    Content-Length: {content.Length}{Environment.NewLine}
                    {Environment.NewLine}
                    User successfully created
                    """;
        }

        public string UserCreate409()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}User with same username already registered";
            return $"""
                    HTTP/1.1 409 User with same username already registered
                    Content-Type: text/plain
                    Content-Length: {content.Length}{Environment.NewLine}
                    {Environment.NewLine}
                    User with same username already registered
                    """;
        }

        public string Session200(string username)
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}User login successful{Environment.NewLine}Token: {username + "-mtcgToken"}";
            return $"""
                    HTTP/1.1 200 User login successful 
                    Content-Type: text/plain
                    Content-Length: {content.Length}{Environment.NewLine}
                    {Environment.NewLine}
                    User login successful{Environment.NewLine}
                    Token: {username + "-mtcgToken"}
                    """;
        }

        public string Session401()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}Invalid username/password provided";
            return $"""
                    HTTP/1.1 401 Invalid username/password provided 
                    Content-Type: text/plain
                    Content-Length: {content.Length}{Environment.NewLine}
                    {Environment.NewLine}
                    Invalid username/password provided
                    """;
        }

        public string Package201()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}Package and cards successfully created";
            return $"""
                   HTTP/1.1 201 Package and cards successfully created 
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   Package and cards successfully created
                   """;
        }

        public string Unauthorized401()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}UnauthorizedError";
            return $"""
                   HTTP/1.1 401 UnauthorizedError 
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   UnauthorizedError
                   """;
        }

        public string Package403()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}Provided user is not \"admin\"";
            return $"""
                   HTTP/1.1 403 Wrong User
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   Provided user is not "admin"
                   """;
        }
        public string Package409()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}At least one card in the packages already exists";
            return $"""
                   HTTP/1.1 409 Card exists 
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   At least one card in the packages already exists
                   """;
        }

        public string BuyPackage403()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}Not enough money for buying a card package";
            return $"""
                   HTTP/1.1 403 Wrong User
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   Not enough money for buying a card package
                   """;
        }

        public string noLoginPackage403()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}Given User not logged in";
            return $"""
                   HTTP/1.1 403 Wrong User
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   Given User not logged in
                   """;
        }

        public string testmethod()
        {
            return this.test;
        }
    }
}
