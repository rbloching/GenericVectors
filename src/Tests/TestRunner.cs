using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;
using Xunit.Runners;

namespace Tests
{
    //Run the xunit tests directly. 
    //see https://github.com/xunit/samples.xunit/blob/master/TestRunner/Program.cs
    public static class TestRunner
    {
        //use console lock because messages arrive in parallel
        static object consoleLock = new object();

        //allows this thread to pause until all test results are in
        static ManualResetEvent finished = new ManualResetEvent(false);


        /// <summary>
        /// Runs all of the Xunit tests in the executing assembly
        /// </summary>
        /// <returns></returns>
        public static bool RunAll()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return Run(assembly);
        }


        /// <summary>
        /// Runs the tests in the given assembly
        /// </summary>
        /// <param name="testAssembly">Assembly with XUnit tests</param>
        /// <returns>True if all tests passed</returns>
        public static bool Run(Assembly testAssembly)
        {
            bool allPass = true;            
            var runner = AssemblyRunner.WithoutAppDomain(testAssembly.Location);

            runner.OnDiscoveryComplete = (info) =>
            {
                write($"Running {info.TestCasesToRun} of {info.TestCasesDiscovered} tests...");
            };

            runner.OnExecutionComplete = (info) =>
            {
                write($"Finished: {info.TotalTests} tests in {Math.Round(info.ExecutionTime, 3)}s ({info.TestsFailed} failed, {info.TestsSkipped} skipped)");
                finished.Set();
            };

            runner.OnTestPassed = (info) =>
            {
                writePass($"[PASS] {info.TestDisplayName}");
                var traits = info.Traits;
            };


            runner.OnTestFailed = (info) =>
            {
                writeFail($"[FAIL] {info.TestDisplayName}: {info.ExceptionMessage}");
                allPass = false;
            };

            runner.OnTestSkipped = (info) =>
            {
                writeWarning($"[SKIP] {info.TestDisplayName}: {info.SkipReason}");
                allPass = false;
            };

            write("Discovering Tests...");
            runner.Start();

            //pause this thread until tests are complete
            finished.WaitOne();
            finished.Dispose();

            return allPass;
        }

        static void write(string message)
        {
            write(message, Console.ForegroundColor);
        }

        static void writePass(string message)
        {
            write(message, ConsoleColor.Green);
        }

        static void writeFail(string message)
        {
            write(message, ConsoleColor.Red);
        }

        static void writeWarning(string message)
        {
            write(message, ConsoleColor.Yellow);
        }

        static void write(string message, ConsoleColor color)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
