namespace EchoService.Models
{
    public class ContentRequest : RequestBase
    {
        public string Body { get; set; }

        public static ContentRequest Post(string route, string user, string body)
        {
            return new ContentRequest { Method = "POST", Route = route, User = user, Body = body };
        }

        public static ContentRequest Put(string route, string user, string body)
        {
            return new ContentRequest { Method = "PUT", Route = route, User = user, Body = body };
        }
    }
}