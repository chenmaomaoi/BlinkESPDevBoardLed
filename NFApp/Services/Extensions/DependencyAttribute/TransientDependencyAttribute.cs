﻿using System;

namespace NFApp.Services.Extensions.DependencyAttribute
{
    /// <summary>
    /// 瞬态
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TransientDependencyAttribute : Attribute
    {
    }
}
