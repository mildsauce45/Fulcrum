using Fulcrum.Models;
using System;

namespace Fulcrum
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class MethodAttribute : Attribute
    {
        internal string Route { get; }
        internal HttpRequestMethod Method { get; }

        internal MethodAttribute(string route, HttpRequestMethod method)
        {
            if (string.IsNullOrWhiteSpace(route))
                throw new ArgumentException("route cannot be null.");

            Route = route;
            Method = method;
        }
    }
}
