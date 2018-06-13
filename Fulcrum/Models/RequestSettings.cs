using System;
using System.Collections.Generic;

namespace Fulcrum.Models
{
    internal class RequestSettings
    {
        public string BaseUrl { get; }
        public IAuthenticationProvider AuthenticationProvider { get; }
        public HeaderCollection GlobalHeaders { get; }

        public RequestSettings(string baseUrl, IAuthenticationProvider authenticationProvider = null, HeaderCollection globalHeaders = null)
        {
            BaseUrl = baseUrl;
            AuthenticationProvider = authenticationProvider;
            GlobalHeaders = globalHeaders;
        }
    }
}
