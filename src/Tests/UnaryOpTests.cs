using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using GenericVectors;
using Xunit;

namespace Tests
{
    public class UnaryOpTests
    {
        [Fact]
        public void TypeInitialization()
        {
            warmupUnary<short>();
            warmupUnary<int>();
            warmupUnary<long>();
            warmupUnary<ushort>();
            warmupUnary<uint>();
            warmupUnary<ulong>();
            warmupUnary<int>();
            warmupUnary<float>();
            warmupUnary<double>();
            warmupUnary<decimal>();
            warmupUnary<Complex>();

        }

        static void warmupUnary<T>() where T: struct
        {
            T x = new T();
            UnaryOp<T>.Sqrt(x);
        }
    }
}
