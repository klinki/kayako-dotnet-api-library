using System;

namespace KayakoRestApi.RequestBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class EitherFieldAttribute : Attribute
    {
        /// <param name="dependsOn">Pipe separated list of dependant properties</param>
        public EitherFieldAttribute(string dependsOn) => this.DependsOn = dependsOn.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        public EitherFieldAttribute(string[] dependsOn) => this.DependsOn = dependsOn;

        public string[] DependsOn { get; }
    }
}