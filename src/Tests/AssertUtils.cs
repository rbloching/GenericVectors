using System;

namespace Tests
{
    /// <summary>
    /// Helper methods for assertions
    /// </summary>
    public static class AssertUtils
    {
        
        /// <summary>
        /// Checks that an exception of the type given is thrown when the action 
        /// is executed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns>True if the given exception was thrown, false otherwise</returns>
        /// <remarks>Use this method to avoid the IDE debugger from breaking on unhandled
        /// exception when using Xunit.Assert.Throws() directly for a passing test.</remarks>
        public static bool TestException<T>(Action action) where T:Exception
        {
            bool result;

            try
            {
                action();
                //no exception thrown
                result = false;                
            }
            catch (T)
            {
                //ok        
                result = true;
            }
            catch (Exception)
            {
                //some other exception was thrown
                result = false;
            }
            return result;
        }
    }
}
