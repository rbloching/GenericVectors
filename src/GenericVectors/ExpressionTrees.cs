using System;
using System.Linq.Expressions;

namespace GenericVectors
{
    internal class ExpressionTrees
    {
        public static Action<T[], T[], T[]> GetElementWiseAction<T>(Func<Expression, Expression, BinaryExpression> binaryOp)
        {
            //parameters to function
            var xArray = Expression.Parameter(typeof(T[]), "x");
            var yArray = Expression.Parameter(typeof(T[]), "y");
            var resultArray = Expression.Parameter(typeof(T[]), "result");

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

        public static Func<T[], T[], T> CreateDot<T>()
        {
            //parameters
            var xArray = Expression.Parameter(typeof(T[]), "x");
            var yArray = Expression.Parameter(typeof(T[]), "y");

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
    }
}
