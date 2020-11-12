using System;

namespace KayakoRestApi.RequestBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class OptionalFieldAttribute : Attribute { }
}