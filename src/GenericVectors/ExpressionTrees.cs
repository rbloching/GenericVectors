using System;
using System.Linq.Expressions;

namespace GenericVectors
{
    /// <summary>
    /// Methods to generate expression trees for various vector operations
    /// </summary>
    internal static partial class ExpressionTrees
    {
        public static Action<T[], T[], T[]> GetElementWiseAction<T>(Func<Expression, Expression, BinaryExpression> binaryOp)
        {
            //parameters to function
            var xArray = Expression.Parameter(typeof(T[]));
            var yArray = Expression.Parameter(typeof(T[]));
            var resultArray = Expression.Parameter(typeof(T[]));

            //local variables
            var index = Expression.Variable(typeof(int));
                        
            //in-loop operation
            Expression op(Expression loopIndex)
            {
                return
                Expression.Assign(
                    Expression.ArrayAccess(resultArray, loopIndex),
                    binaryOp(
                        Expression.ArrayIndex(xArray, loopIndex),
                        Expression.ArrayIndex(yArray, loopIndex)
                      )
                  );
            };

            var block = Expression.Block(getLoopExpression(op, xArray));                        
            return Expression.Lambda<Action<T[], T[], T[]>>(block, xArray, yArray, resultArray).Compile();
        }


        public static Action<T[], T, T[]> GetElementWiseWithScalarAction<T>(Func<Expression, Expression, BinaryExpression> binaryOp)
        {
            //parameters to function
            var xArray = Expression.Parameter(typeof(T[]));
            var scalar = Expression.Parameter(typeof(T));            
            var resultArray = Expression.Parameter(typeof(T[]));
            
            //in-loop operation
            Expression op(Expression loopIndex)
            {
                return
                Expression.Assign(
                    Expression.ArrayAccess(resultArray, loopIndex),
                    binaryOp(
                        Expression.ArrayIndex(xArray, loopIndex),
                        scalar
                      )
                  );
            };

            var block = Expression.Block(getLoopExpression(op, xArray));
            return Expression.Lambda<Action<T[], T, T[]>>(block, xArray, scalar, resultArray).Compile();
        }
               

        public static Func<T[], T> CreateSum<T>()
        {
            //parameter to function
            var array = Expression.Parameter(typeof(T[]));

            //local variables
            var sum = Expression.Variable(typeof(T));

            //in-loop operation
            Expression op(Expression loopIndex)
            { return Expression.AddAssign(sum, Expression.ArrayIndex(array, loopIndex)); }

            var block = Expression.Block(new[] { sum }, 
                getLoopExpression(op, array),                
                sum);
            return Expression.Lambda<Func<T[], T>>(block, array).Compile();
        }

        public static Func<T[], T> CreateProduct<T>()
        {
            //parameter to function
            var array = Expression.Parameter(typeof(T[]));

            //local variables
            var result = Expression.Variable(typeof(T));            

            //in-loop operation
            Expression op(Expression loopIndex)
            { return Expression.MultiplyAssign(result, Expression.ArrayIndex(array, loopIndex)); }

            var block = Expression.Block(new[] { result },
                Expression.Assign(result,one<T>()),
                getLoopExpression(op, array),
                result);            
            return Expression.Lambda<Func<T[], T>>(block, array).Compile();
        }       

        public static Func<T[], T[], T> CreateDot<T>()
        {
            //parameters
            var xArray = Expression.Parameter(typeof(T[]));
            var yArray = Expression.Parameter(typeof(T[]));

            //local variables
            var sum = Expression.Variable(typeof(T));

            //in-loop operation
            Expression op(Expression loopIndex)
            {
                var expr =
                Expression.AddAssign(
                    sum,
                    Expression.Multiply(
                        Expression.ArrayIndex(xArray, loopIndex),
                        Expression.ArrayIndex(yArray, loopIndex)
                        )
                    );
                return expr;
            }

            var block = Expression.Block(new[] { sum }, getLoopExpression(op, xArray),sum);            
            return Expression.Lambda<Func<T[], T[], T>>(block, xArray, yArray).Compile();
        }
       
        public static Func<T[],int,T> CreateVar<T>()
        {
            //parameters
            var xArray = Expression.Parameter(typeof(T[]));
            var degOfFreedom = Expression.Parameter(typeof(int));

            //local variables             
            var count = Expression.Variable(typeof(T));
            var sum = Expression.Variable(typeof(T));
            var mean = Expression.Variable(typeof(T));
            var diff = Expression.Variable(typeof(T));
            var df = Expression.Variable(typeof(T));
            
            Expression avgLoopOp(Expression loopIndex)
            {
                var expr = Expression.AddAssign(mean, Expression.ArrayIndex(xArray, loopIndex));
                return expr;
            }

            Expression varLoopOp(Expression loopIndex)
            {
                var expr = Expression.Block(
                    Expression.Assign(diff, 
                        Expression.Subtract(Expression.ArrayIndex(xArray, loopIndex), mean)),
                    Expression.AddAssign(sum, Expression.Multiply(diff, diff))
                    );
                return expr;
            }


            var block =
            Expression.Block(
                new[] {sum, mean,diff,count, df},
                Expression.Assign(count, 
                    Expression.Convert(Expression.ArrayLength(xArray), typeof(T))),
                Expression.Assign(df, Expression.Convert(degOfFreedom, typeof(T))),
                
                //first pass - get the average
                getLoopExpression(avgLoopOp,xArray),
                Expression.DivideAssign(mean, count),

                //second pass - get the variance
                getLoopExpression(varLoopOp, xArray),
                Expression.DivideAssign(sum, Expression.Subtract(count, df)),
                sum
                );

            return Expression.Lambda<Func<T[],int, T>>(block, xArray, degOfFreedom).Compile();            
        }

        static Expression zeroInt()
        {
            return Expression.Constant(0, typeof(int));
        }

        static Expression one<T>()
        {
            return Expression.Convert(Expression.Constant(1),typeof(T));
        }

        /// <summary>
        /// Creates a loop expression that will perform the given operation on each iteration
        /// </summary>
        /// <param name="operation">A function representing the operation to perform. This 
        /// should take an expression representing the current index of the loop, and return 
        /// an expression representing the operation to iterate on.</param>
        /// <param name="array">An array representing a parameter to one of the vector operations.
        /// The array given does not matter (some vector operations take 2 arrays), it will
        /// not be altered by this expression. It will only be used to track the index</param>
        /// <returns></returns>
        static Expression getLoopExpression(Func<Expression,Expression> operation, ParameterExpression array)
        {
            var length = Expression.Variable(typeof(int));
            var index = Expression.Variable(typeof(int));
            var label = Expression.Label();

            var block =
                Expression.Block(
                    new[] { index, length },                    
                    Expression.Assign(length,Expression.ArrayLength(array)),
                    Expression.Assign(index, zeroInt()),
                    Expression.Loop(
                        Expression.Block(                            
                            operation(index),
                            Expression.PostIncrementAssign(index),
                            Expression.IfThen(Expression.GreaterThanOrEqual(index,length), 
                                Expression.Break(label)
                                )
                            ),
                        label                            
                        )
                    );

            return block;
        }
    }
}
