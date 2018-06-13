using System.Collections.Generic;

namespace Fulcrum.UnitTests.Models
{
    public class Response
    {
        public string Method { get; set; }
        public string User { get; set; }
        public string Route { get; set; }
        public string Body { get; set; }
        public IDictionary<string, string> QueryParams { get; set; }
        public IDictionary<string, string> Headers { get; set; }
    }
}
