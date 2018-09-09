using System;
using BenchmarkDotNet.Attributes;
using GenericVectors;
using MiscUtil;

namespace Benchmarks
{
    public class GenericVectorBenchmark
    {
        double[] x;
        double[] y;
            

        [GlobalSetup]
        public void Setup()
        {
            x = randomArray(ArrayLength);
            y = randomArray(ArrayLength);
        }

        [Params(1_000, 10_000, 100_000)]
        public int ArrayLength { get; set; }

        [Benchmark]
        public void DotArrayDouble()
        {
            dot(x, y);
        }

        [Benchmark]
        public void DotGenericOpInLoop()
        {
            dot<double>(x, y);
        }

        [Benchmark]
        public void DotGenericVector()
        {
            VectorOp.Dot(x, y);
        }



        static double dot(double[] x, double[] y)
        {
            int len = x.Length;
            double sum = 0.0;
            for(int i=0; i<len; i++)
            {
                sum += (x[i] * y[i]);
            }
            return sum;
        }

        //perform dot product with generic operator in the loop
        static T dot<T>(T[] x, T[] y)
        {
            int len = x.Length;
            T sum = Operator<T>.Zero;
            for (int i = 0; i < len; i++)
            {
                var prod = Operator.Multiply(x[i], y[i]);
                sum = Operator.Add(sum, prod);
            }
            return sum;
        }


        static double[] randomArray(int length)
        {
            double[] result = new double[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = rand.NextDouble();
            }
            return result;
        }
        static Random rand = new Random();
    }
}
