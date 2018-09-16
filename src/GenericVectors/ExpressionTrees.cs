using System;
using System.Linq.Expressions;

namespace GenericVectors
{
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
            var argLen = Expression.Variable(typeof(int));

            //operation
            var op =
            Expression.Assign(
                Expression.ArrayAccess(resultArray, index),
                binaryOp(
                    Expression.ArrayIndex(xArray, index),
                    Expression.ArrayIndex(yArray, index)
                )
            );

            //function body
            var block =
            Expression.Block(
                //locals
                new[] { index, argLen },
                //statements
                Expression.Assign(argLen, Expression.ArrayLength(xArray)),
                getArrayLoopExpression(op, argLen, index)
            );

            return Expression.Lambda<Action<T[], T[], T[]>>(block, xArray, yArray, resultArray).Compile();
        }


        public static Action<T[], T, T[]> GetElementWiseWithScalarAction<T>(Func<Expression, Expression, BinaryExpression> binaryOp)
        {
            //parameters to function
            var xArray = Expression.Parameter(typeof(T[]));
            var scalar = Expression.Parameter(typeof(T));            
            var resultArray = Expression.Parameter(typeof(T[]));

            //local variables
            var index = Expression.Variable(typeof(int));
            var argLen = Expression.Variable(typeof(int));

            //operation
            var op =
            Expression.Assign(
                Expression.ArrayAccess(resultArray, index),
                binaryOp(                                        
                    Expression.ArrayIndex(xArray, index),
                    scalar
                )
            );

            //function body
            var block =
            Expression.Block(
                //locals
                new[] { index, argLen },
                //statements
                Expression.Assign(argLen, Expression.ArrayLength(xArray)),
                getArrayLoopExpression(op, argLen, index)
            );

            return Expression.Lambda<Action<T[], T, T[]>>(block, xArray, scalar, resultArray).Compile();
        }
       
        

        public static Func<T[], T> CreateSum<T>()
        {
            //parameter to function
            var array = Expression.Parameter(typeof(T[]));

            //local variables
            var sum = Expression.Variable(typeof(T));
            var index = Expression.Variable(typeof(int));
            var argLen = Expression.Variable(typeof(int));

            //expression tree
            var setLen = Expression.Assign(argLen, Expression.ArrayLength(array));
            var op = Expression.AddAssign(sum, Expression.ArrayIndex(array, index));         
            var loop = getArrayLoopExpression(op, argLen, index);            
            var block = Expression.Block(new[] { sum, index, argLen }, setLen, loop, sum);

            return Expression.Lambda<Func<T[], T>>(block, array).Compile();
        }


        public static Func<T[], T> CreateProduct<T>()
        {
            //parameter to function
            var array = Expression.Parameter(typeof(T[]));

            //local variables
            var result = Expression.Variable(typeof(T));
            var index = Expression.Variable(typeof(int));
            var argLen = Expression.Variable(typeof(int));


            //expression tree            
            var op = Expression.MultiplyAssign(result, Expression.ArrayIndex(array, index));
            var loop = getArrayLoopExpression(op, argLen, index);
            var block = Expression.Block(new[] { result, index, argLen },
                Expression.Assign(argLen, Expression.ArrayLength(array)),
                Expression.Assign(result,one<T>()),                
                loop, 
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
            var index = Expression.Variable(typeof(int));
            var argLen = Expression.Variable(typeof(int));

            //expression tree
            var setLen = Expression.Assign(argLen, Expression.ArrayLength(xArray));
            var op =
            Expression.AddAssign(
                sum,
                Expression.Multiply(
                    Expression.ArrayIndex(xArray, index),
                    Expression.ArrayIndex(yArray, index)
                )
            );                        
            var loop = getArrayLoopExpression(op, argLen, index);
            var block = Expression.Block(new[] { sum, index, argLen }, setLen, loop, sum);
            
            return Expression.Lambda<Func<T[], T[], T>>(block, xArray, yArray).Compile();
        }
       
        public static Func<T[],int,T> CreateVar<T>()
        {
            //parameters
            var xArray = Expression.Parameter(typeof(T[]));
            var degOfFreedom = Expression.Parameter(typeof(int));

            //local variables 
            var arrayLen = Expression.Variable(typeof(int));
            var index = Expression.Variable(typeof(int));
            var count = Expression.Variable(typeof(T));
            var sum = Expression.Variable(typeof(T));
            var mean = Expression.Variable(typeof(T));
            var diff = Expression.Variable(typeof(T));
            var df = Expression.Variable(typeof(T));

            //loop labels
            var label1 = Expression.Label(typeof(void));
            var label2 = Expression.Label(typeof(void));



            var block =
            Expression.Block(
                new[] { sum, arrayLen, index, mean, diff, count, df },
                Expression.Assign(arrayLen, Expression.ArrayLength(xArray)),
                Expression.Assign(count, Expression.Convert(arrayLen, typeof(T))),
                Expression.Assign(df, Expression.Convert(degOfFreedom, typeof(T))),
                Expression.Assign(index, zeroInt()),

                //first pass - get the average
                Expression.Loop(
                    Expression.Block(
                        Expression.AddAssign(mean, Expression.ArrayIndex(xArray, index)),
                        Expression.PostIncrementAssign(index),
                        Expression.IfThen(Expression.GreaterThanOrEqual(index, arrayLen), Expression.Break(label1))
                    ),
                label1),
                Expression.DivideAssign(mean, count),

                //second pass - get the variance
                Expression.Assign(index, zeroInt()),
                Expression.Loop(
                    Expression.Block(
                        Expression.Assign(diff,
                            Expression.Subtract(Expression.ArrayIndex(xArray, index), mean)
                        ),
                        Expression.AddAssign(sum, Expression.Multiply(diff, diff)),
                        Expression.PostIncrementAssign(index),
                        Expression.IfThen(Expression.GreaterThanOrEqual(index, arrayLen), Expression.Break(label2))
                    ),
                    label2
                ),

                Expression.DivideAssign(sum, Expression.Subtract(count,df)),
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


        static Expression getArrayLoopExpression(Expression operation, Expression vectorLength, Expression index)
        {
            var label = Expression.Label(typeof(void)); //void when not returning a value

            var loop =
            Expression.Loop(
                Expression.Block(
                    operation,
                    Expression.PostIncrementAssign(index),
                    Expression.IfThen(Expression.GreaterThanOrEqual(index, vectorLength), Expression.Break(label))
                ),
                label
            );
            return loop;
        }
    }
}
