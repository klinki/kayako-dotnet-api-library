using System;

namespace KayakoRestApi.RequestBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class ResponsePropertyAttribute : Attribute
    {
        public ResponsePropertyAttribute(string repsonseProperty) => this.RepsonseProperty = repsonseProperty;

        public string RepsonseProperty { get; }
    }
}