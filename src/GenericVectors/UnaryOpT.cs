using System;
using System.Collections.Generic;
using System.Text;

namespace GenericVectors
{
    /// <summary>
    /// Defines the expressions for generic unary operators
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class UnaryOp<T>
    {
        static readonly Func<T, T> sqrt;

        static UnaryOp()
        {            
            sqrt = ExpressionTrees.CreateSquareRoot<T>();
        }

        public static T Sqrt(T x)
        {
            return sqrt(x);
        }

    }
}
