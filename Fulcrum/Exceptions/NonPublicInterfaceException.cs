using System;

namespace Fulcrum
{
    public class NonPublicInterfaceException : Exception
    {
        public NonPublicInterfaceException()
            : base("Cannot emit a non-public interface")
        {
        }
    }
}
