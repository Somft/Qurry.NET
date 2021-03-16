using System;
using System.Linq.Expressions;

namespace Qurry.Core
{
    public interface IProperty
    {
        Expression Expression { get; }

        Type ReturnType { get; }
    }
}