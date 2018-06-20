using System;

namespace Fulcrum
{
    public class SynchronousEndpointException : Exception
    {
        public SynchronousEndpointException(string methodName)
            : base($"Method {methodName} is decorated with Fulcrum attribute but does not return a Task or Task<T>. Synchronous returns are not allowed.")
        {
        }
    }
}
