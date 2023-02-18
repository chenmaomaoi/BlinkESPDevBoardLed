using System;

namespace NFApp.Services.Extensions.DependencyAttribute
{
    /// <summary>
    /// 单例
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SingletonDependencyAttribute : Attribute
    {
    }
}
