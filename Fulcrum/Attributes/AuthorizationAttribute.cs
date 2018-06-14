using System;

namespace Fulcrum
{
    /// <summary>
    /// <para>
    /// A specialized HeaderAttribute that contains information about how to use OAuth to communicate with the API
    /// The passed in value should be the bearer token or the basic api/secret (encrypted if needs be).
    /// </para>
    /// <para>
    /// If you'd prefer not to do this, pass in a IAuthenticationProvider instance when calling Connect.To&lt;T&gt;
    /// </para>
    /// </summary>
    public sealed class AuthorizationAttribute : HeaderAttribute
    {
        public TokenType TokenType { get; }

        public AuthorizationAttribute()
            : base("Authorization")
        {
            TokenType = TokenType.Bearer;
        }

        public AuthorizationAttribute(string header)
            : base(header)
        {
            TokenType = TokenType.Bearer;
        }

        public AuthorizationAttribute(string header, TokenType tokenType)
            : base(header)
        {
            TokenType = TokenType;
        }

        internal override Tuple<string, string> GetHeader(object liveValue) =>
            Tuple.Create(Header, $"{TokenType} {liveValue?.ToString()}");
    }
}
