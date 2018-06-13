using System.Net.Http;

namespace Fulcrum
{
    public class HttpRequestState
    {
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }        
    }
}
