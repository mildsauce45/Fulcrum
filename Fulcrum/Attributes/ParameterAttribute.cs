using Fulcrum.Models;
using System;
using System.Reflection;

namespace Fulcrum
{
    /// <summary>
    /// A base class for an attribute that should be attached to a paramter passed to a configured endpoint method
    /// </summary>
    public abstract class ParameterAttribute : Attribute
    {
        internal abstract void ConfigureParameter(ParameterConfig config, ParameterInfo parameter);
    }
}
