using System;

namespace Fulcrum
{
    public class MissingHttpMethodException : Exception
    {
        public MissingHttpMethodException()
            : base("Interface is missing at least one method marked up with a MethodAttribute (GetAttribute, PostAttribute, etc)")
        {
        }        
    }
}
