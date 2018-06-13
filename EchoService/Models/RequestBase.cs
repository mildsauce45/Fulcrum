using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EchoService.Models
{
    public class RequestBase
    {
        public string Method { get; set; }
        public string User { get; set; }
        public string Route { get; set; }
        public IDictionary<string, string> QueryParams { get; set; }
        public IDictionary<string, string> Headers { get; set; }
    }
}