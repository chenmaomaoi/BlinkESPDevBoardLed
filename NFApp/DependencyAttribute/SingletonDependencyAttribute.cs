using System;

namespace NFApp.DependencyAttribute
{
    /// <summary>
    /// 单例
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SingletonDependencyAttribute : Attribute
    {
    }
}
