using Fulcrum.Models;
using System;

namespace Fulcrum
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class MethodAttribute : Attribute
    {
        public string Route { get; }
        public HttpRequestMethod Method { get; }

        public MethodAttribute(string route, HttpRequestMethod method)
        {
            if (string.IsNullOrWhiteSpace(route))
                throw new ArgumentException("route cannot be null.");

            Route = route;
            Method = method;
        }
    }
}
