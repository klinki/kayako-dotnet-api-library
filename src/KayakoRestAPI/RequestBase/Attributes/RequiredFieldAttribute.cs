using System;

namespace KayakoRestApi.RequestBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class RequiredFieldAttribute : Attribute
    {
        public RequiredFieldAttribute() => this.RequestTypes = new RequestTypes[0];

        public RequiredFieldAttribute(RequestTypes types)
            : base() => this.RequestTypes = new[] { types };

        public RequestTypes[] RequestTypes { get; }
    }
}