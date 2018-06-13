using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EchoService.Models
{
    public class DeleteRequest : RequestBase
    {
        public DeleteRequest()
        {
            Method = "DELETE";
        }
    }
}