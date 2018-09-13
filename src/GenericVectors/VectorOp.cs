using MiscUtil;

namespace GenericVectors
{
    /// <summary>
    /// Provides a collection of generic array math operations
    /// </summary>
    public static class VectorOp
    {
        /// <summary>
        /// Element-wise addition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="result">will be set to the resulting array</param>
        public static void Add<T>(T[] x, T[] y, T[] result)
        {
            VectorOp<T>.Add(x, y, result);
        }

        /// <summary>
        /// Adds the scalar to each element of the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="scalar"></param>
        /// <param name="result">will be set to the resulting array</param>
        public static void Add<T>(T[] x, T scalar,  T[] result)
        {
            VectorOp<T>.Add(x, scalar, result);
        }

        /// <summary>
        /// Element-wise subtraction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="result">will be set to the resulting array</param>
        public static void Subtract<T>(T[] x, T[] y, T[] result)
        {
            VectorOp<T>.Subtract(x, y, result);
        }

        
        /// <summary>
        /// Subtracts the scalar from each element in the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="scalar"></param>
        /// <param name="result">will be set to the resulting array</param>
        public static void Subtract<T>(T[] x, T scalar, T[] result) 
        {
            VectorOp<T>.Subtract(x, scalar, result);
        }

        /// <summary>
        /// Element-wise multiplication
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="result">will be set to the resulting aray</param>
        public static void Multiply<T>(T[] x, T[] y, T[] result) 
        {
            VectorOp<T>.Multiply(x, y, result);
        }

        /// <summary>
        /// Multiplies the scalar to each element in x
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="scalar"></param>
        /// <param name="result">will be set to the resulting array</param>
        public static void Multiply<T>(T[] x, T scalar, T[] result)
        {
            VectorOp<T>.Multiply(x, scalar, result);
        }

        /// <summary>
        /// Element-wise division
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">Dividend array</param>
        /// <param name="y">Divisor array</param>
        /// <param name="result">will be set to the resulting array</param>
        public static void Divide<T>(T[] x, T[] y, T[] result)
        {
            VectorOp<T>.Divide(x, y, result);
        }

        
        /// <summary>
        /// Divides each element in the array by the scalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="scalar"></param>
        /// <param name="result">will be set to the resulting array</param>
        public static void Divide<T>(T[] x, T scalar,T[] result)
        {
            VectorOp<T>.Divide(x, scalar, result);
        }


        /// <summary>
        /// Calculates the inner (dot) product of the elements in the given arrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static T Dot<T>(T[] x, T[] y)
        {
            return VectorOp<T>.Dot(x, y);
        }

        /// <summary>
        /// Squares the elements of x and places the results in result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="result">will be set to the resulting array</param>
        public static void Square<T>(T[] x, T[] result)
        {
            Multiply(x, x, result);            
        }

        /// <summary>
        /// Squares the elements of the array in-place
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">The array to square.</param>
        public static void Square<T>(T[] x)
        {
            Multiply(x, x, x);
        }


        /// <summary>
        /// Calculates the sum of the elements in the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <returns>the sum of the elements in the array</returns>
        public static T Sum<T>(T[] x)
        {
            return VectorOp<T>.Sum(x);
        }        

        /// <summary>
        /// Calculates the average of the elements in the given array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <returns>the average of the elements in the array</returns>
        public static T Mean<T>(T[] x) 
        {
            return Operator.DivideInt32(Sum(x), x.Length);            
        }


        public static T Var<T>(T[] x)
        {
            return VectorOp<T>.Var(x);            
        }
       
    }
}
