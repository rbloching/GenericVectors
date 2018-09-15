using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Numerics;

namespace GenericVectors
{
    internal static partial class ExpressionTrees
    {
		public static Func<T,T> CreateSquareRoot<T>()
        {
            //parameter
            var x = Expression.Parameter(typeof(T));

            //locals
            var result = Expression.Variable(typeof(T));

            var method = typeof(ExpressionTrees).GetMethod("sqrt", BindingFlags.Static|BindingFlags.NonPublic, null,  new Type[] { typeof(T) },null);

            //expressions
            var block =
            Expression.Block(
                new[] { result },                
                Expression.Assign(result, Expression.Call(method,x)),
				result
			);

            return Expression.Lambda<Func<T, T>>(block, x).Compile();
        }

       
        static double sqrt(double x)
        {
            return Math.Sqrt(x);
        }

        static float sqrt(float x)
        {
            return (float)Math.Sqrt(x);
        }

        static Complex sqrt(Complex x)
        {
            return Complex.Sqrt(x);
        }

        //less useful square roots, but needed to 
        // maintain type compatability for all operations

        static decimal sqrt(decimal x)
        {
            return (decimal)Math.Sqrt((double)x);
        }
        
        static ushort sqrt(ushort x)
        {
            return (ushort)Math.Sqrt((double)x);
        }

        static short sqrt(short x)
        {
            return (short)Math.Sqrt((double)x);
        }

        static int sqrt(int x)
        {
            return (int)Math.Sqrt((double)x);
        }

        static uint sqrt(uint x)
        {
            return (uint)Math.Sqrt((double)x);
        }

        static long sqrt(long x)
        {
            return (long)Math.Sqrt((double)x);
        }

        static ulong sqrt(ulong x)
        {
            return (ulong)Math.Sqrt((double)x);
        }        
    }
}
