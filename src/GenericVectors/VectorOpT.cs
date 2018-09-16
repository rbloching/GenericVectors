using System;
using System.Linq.Expressions;

namespace GenericVectors
{
    /// <summary>
    /// Defines the expressions for each vector operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class VectorOp<T>
    {
        static readonly Action<T[], T[], T[]> addArray;
        static readonly Action<T[], T[], T[]> subtractArray;
        static readonly Action<T[], T[], T[]> multArray;
        static readonly Action<T[], T[], T[]> divArray;

        static readonly Action<T[], T, T[]> addScalar;
        static readonly Action<T[], T, T[]> subtractScalar;
        static readonly Action<T[], T, T[]> multScalar;
        static readonly Action<T[], T, T[]> divScalar;

        static readonly Func<T[], T> sum;
        static readonly Func<T[], T> prod;
        static readonly Func<T[], T[], T> dot;
        static readonly Func<T[],int, T> variance;
        
        static VectorOp()
        {            
            addArray = ExpressionTrees.GetElementWiseAction<T>(Expression.Add);
            subtractArray = ExpressionTrees.GetElementWiseAction<T>(Expression.Subtract);
            multArray = ExpressionTrees.GetElementWiseAction<T>(Expression.Multiply);
            divArray = ExpressionTrees.GetElementWiseAction<T>(Expression.Divide);
            
            addScalar = ExpressionTrees.GetElementWiseWithScalarAction<T>(Expression.Add);
            subtractScalar = ExpressionTrees.GetElementWiseWithScalarAction<T>(Expression.Subtract);
            multScalar = ExpressionTrees.GetElementWiseWithScalarAction<T>(Expression.Multiply);
            divScalar = ExpressionTrees.GetElementWiseWithScalarAction<T>(Expression.Divide);
            
            sum = ExpressionTrees.CreateSum<T>();
            prod = ExpressionTrees.CreateProduct<T>();
            dot = ExpressionTrees.CreateDot<T>();
            variance = ExpressionTrees.CreateVar<T>();
        }


        public static T Sum(T[] x)
        {
            return sum(x);
        }

        public static T Prod(T[] x)
        {
            return prod(x);
        }

        public static T Dot(T[] x, T[] y)
        {
            return dot(x, y);
        }

        public static void Add(T[] x, T[] y, T[] result)
        {
            addArray(x, y, result);
        }

        public static void Add(T[] x, T scalar, T[] result)
        {
            addScalar(x, scalar, result);
        }

        public static void Subtract(T[] x, T[] y, T[] result)
        {
            subtractArray(x, y, result);
        }

        public static void Subtract(T[] x, T scalar, T[] result)
        {
            subtractScalar(x, scalar, result);
        }

        public static void Multiply(T[] x, T[] y, T[] result)
        {
            multArray(x, y, result);
        }

        public static void Multiply(T[] x, T scalar, T[] result)
        {
            multScalar(x, scalar, result);
        }

        public static void Divide(T[] x, T[] y, T[] result)
        {
            divArray(x, y, result);
        }

        public static void Divide(T[] x, T scalar,  T[] result)
        {
            divScalar(x, scalar, result);
        }       
       
        public static T Variance(T[] x, int degOfFreedom)
        {
            return variance(x, degOfFreedom);
        }

    }
}
