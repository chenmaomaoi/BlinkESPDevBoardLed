using System;

namespace NFApp.DependencyAttribute
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonDependencyAttribute : Attribute
    {
    }
}
