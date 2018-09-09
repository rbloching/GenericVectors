using System;
using System.Numerics;
using GenericVectors;
using Xunit;

namespace Tests
{
    public class VectorOpTests
    {
        
        [Fact]
        public void AddTest()
        {
            int[] x = { 1, 2, 3 };
            int[] y = { 10, 20, 30 };
            int[] expected = {11,22,33 };
            int[] actual = new int[x.Length];

            VectorOp.Add(x, y, actual);
            Assert.Equal(expected, actual);
        }

        
        [Fact]
        public void ThrowsExceptionWhenLengthsUnequal()
        {
            int[] x = { 1, 2, 3 };
            int[] y = { 10, 20};
            int[] result = new int[x.Length];

            Action action = () => { VectorOp.Add(x, y, result); };
            bool thrown = AssertUtils.TestException<IndexOutOfRangeException>(action);
            Assert.True(thrown);
        }


        [Fact]
        public void MultiplyWithScalar()
        {
            float scalar = 10.0f;
            float[] x = { 1, 2, 3 };
            float[] result = new float[x.Length];
            float[] expected = {10f,20f,30f };

            VectorOp.Multiply(x,scalar, result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Multiply()
        {            
            float[] x = { 1, 2, 3 };
            float[] y = { 10, 10, 10 };
            float[] result = new float[x.Length];
            float[] expected = { 10f, 20f, 30f };

            VectorOp.Multiply(x, y, result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DivideScalar()
        {
            float[] x = { 10f,20f,30f };
            float scalar = 10f;
            float[] result = new float[x.Length];
            float[] expected = { 1f, 2f, 3f };

            VectorOp.Divide(x,scalar, result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DotTest()
        {
            float[] x = { 1, 2, 3 };
            float[] y = { 10, 10, 10 };            
            float expected = 60f;

            float result = VectorOp.Dot(x, y);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SquareInPlaceTest()
        {
            decimal[] x = { 1, 2, 3, 4 };
            decimal[] expected = { 1, 4, 9, 16 };
            VectorOp.Square(x);
            Assert.Equal(expected, x);
        }
        

        [Fact]
        public void SumTest()
        {
            int[] x = {10,20,30,40 };
            int expected = 100;

            int result = VectorOp.Sum(x);
            Assert.Equal(expected, result);
        }

        
        [Fact]
        public void MeanTest()
        {
            BigInteger[] x = {100,200,300 };
            BigInteger expected = new BigInteger(200);
            BigInteger result = VectorOp.Mean(x);
            Assert.Equal(expected, result);            
        } 
    }
}
