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
        //public const string json = "{\"Username\": \"Ninja\", \"Password\": \"Tutel\"}";
        //private string test = $"""
        //    HTTP/1.1 200 OK {Environment.NewLine}
        //    Content-Type: application/json {Environment.NewLine}
        //    Content-Length: {json.Length} {Environment.NewLine}
        //    {Environment.NewLine}
        //    {json} {Environment.NewLine}

        //    """;

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
        { //{Environment.NewLine}
            string content = $"\r\n\r\nUser login successful\r\nToken: {username}-mtcgToken";
            //Console.WriteLine("CONTENT LENGTH" + content.Length);
            //Console.WriteLine("CONTENT: " + content);
            return $"""
                    HTTP/1.1 200 User login successful
                    Content-Type: text/plain
                    Content-Length: {content.Length-2}
                    {Environment.NewLine}{Environment.NewLine}User login successful{Environment.NewLine}Token: {username}-mtcgToken
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

        public string notAuthUser403(string Authuser)
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}Provided user is not \"{Authuser}\"";
            return $"""
                   HTTP/1.1 403 Wrong User
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   Provided user is not "{Authuser}"
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

        public string BuyPackage403() //noMoney
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

        public string buyPackage200()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}A package has been successfully bought";
            return $"""
                   HTTP/1.1 200 OK
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   A package has been successfully bought
                   """;
            //Content-Type: application/json
        }

        public string buyPackage404()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}No card package available for buying";
            return $"""
                   HTTP/1.1 404 Not Found
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   No card package available for buying
                   """;
        }

        public string showCards200(string jsonstr)
        {
            string content = $"\r\n\r\n{jsonstr}";
            return $"""
                   HTTP/1.1 200 OK
                   Content-Type: application/json
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   {jsonstr}
                   """;
        }

        public string showCards203()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}The request was fine, but the user doesn't have any cards";
            return $"""
                   HTTP/1.1 203 No Content
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   The request was fine, but the user doesn't have any cards
                   """;
        }

        public string showDeck200(string jsonstr)
        {
            //Console.WriteLine(jsonstr);
            string content = $"\r\n\r\n{jsonstr}";
            return $"""
                   HTTP/1.1 200 OK
                   Content-Type: application/json
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   {jsonstr}
                   """;
        }

        public string emptyDeck203()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}The request was fine, but the deck doesn't have any cards";
            return $"""
                   HTTP/1.1 203 No Content
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   The request was fine, but the deck doesn't have any cards
                   """;
        }

        public string setDeck200()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}The deck has been successfully configured";
            return $"""
                   HTTP/1.1 204 No Content
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   The deck has been successfully configured
                   """;
        }

        public string notEnoughCardsForDeck400()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}The provided deck did not include the required amount of cards";
            return $"""
                   HTTP/1.1 400 No Content
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   The provided deck did not include the required amount of cards
                   """;
        }

        public string setDeck403()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}At least one of the provided cards does not belong to the user or is not available.";
            return $"""
                   HTTP/1.1 403 Forbidden
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   At least one of the provided cards does not belong to the user or is not available.
                   """;
        }

        public string getUser200(string jsonstr)
        {
            string content = $"\r\n\r\n{jsonstr}";
            return $"""
                   HTTP/1.1 200 OK
                   Content-Type: application/json
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   {jsonstr}
                   """;
        }

        public string getUser404()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}User not found.";
            return $"""
                   HTTP/1.1 404 Not Found
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   User not found.
                   """;
        }

        public string putUser200()
        {
            string content = $"{Environment.NewLine}{Environment.NewLine}User successfully updated.";
            return $"""
                   HTTP/1.1 200 OK
                   Content-Type: text/plain
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   User successfully updated.
                   """;
        }

        public string getStats200(string jsonstr)
        {
            string content = $"\r\n\r\n{jsonstr}";
            return $"""
                   HTTP/1.1 200 OK
                   Content-Type: application/json
                   Content-Length: {content.Length}{Environment.NewLine}
                   {Environment.NewLine}
                   {jsonstr}
                   """;
        }
    }
}
