namespace Fulcrum.Models
{
    internal class RequestSettings
    {
        internal string BaseUrl { get; }
        internal IAuthenticationProvider AuthenticationProvider { get; }
        internal HeaderCollection GlobalHeaders { get; }

        internal RequestSettings(string baseUrl, IAuthenticationProvider authenticationProvider = null, HeaderCollection globalHeaders = null)
        {
            BaseUrl = baseUrl;
            AuthenticationProvider = authenticationProvider;
            GlobalHeaders = globalHeaders;
        }
    }
}
