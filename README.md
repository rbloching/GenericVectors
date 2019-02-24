Generic Vectors
===================

GenericVectors is a C# library of vectorized math operations for use with generically-typed arrays. 
It extends the concept of LINQ expression building for generic math operators from [MiscUtil Libary](http://jonskeet.uk/csharp/miscutil/),
and applies it to aggregate array operations. This allows performance closer to the non-generic array operations.



Benchmarks
----------
The benchmark included in the project compares the mean time of calculating the dot product 
between two arrays, using three different methods: a non-generic function, 
a generic method using MiscUtil generic operators in the loop, and finally using GenericVectors.
For all methods, the arrays are of type double.

``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.523 (1803/April2018Update/Redstone4)
Unknown processor
Frequency=3312656 Hz, Resolution=301.8726 ns, Timer=TSC
.NET Core SDK=2.2.100-preview1-009349
  [Host]     : .NET Core 2.1.3 (CoreCLR 4.6.26725.06, CoreFX 4.6.26725.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.3 (CoreCLR 4.6.26725.06, CoreFX 4.6.26725.05), 64bit RyuJIT


```
|                                Method | ArrayLength |         Mean |        Error |       StdDev |
|-------------------------------------- |------------ |-------------:|-------------:|-------------:|
|             **&#39;Non-generic dot product&#39;** |        **1000** |     **814.9 ns** |     **6.752 ns** |     **6.316 ns** |
| &#39;Dot product using generic operators&#39; |        1000 |   4,631.9 ns |    43.085 ns |    40.301 ns |
|     &#39;Dot product using GenericVector&#39; |        1000 |     818.9 ns |     1.831 ns |     1.712 ns |
|             **&#39;Non-generic dot product&#39;** |       **10000** |   **8,171.7 ns** |    **57.593 ns** |    **51.055 ns** |
| &#39;Dot product using generic operators&#39; |       10000 |  46,095.2 ns |   140.806 ns |   131.710 ns |
|     &#39;Dot product using GenericVector&#39; |       10000 |   8,128.5 ns |    23.103 ns |    20.480 ns |
|             **&#39;Non-generic dot product&#39;** |      **100000** |  **81,150.8 ns** |   **165.123 ns** |   **154.457 ns** |
| &#39;Dot product using generic operators&#39; |      100000 | 465,606.7 ns | 4,901.236 ns | 4,344.818 ns |
|     &#39;Dot product using GenericVector&#39; |      100000 |  81,247.5 ns |   168.728 ns |   157.828 ns |





Types Supported
---------------
GenericVectors will work with arrays of these types:
* short
* int
* long
* ushort
* uint
* ulong
* float
* double
* decimal
* System.Numerics.Complex
