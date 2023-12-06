using System;

namespace ViolatingCode
{
    public class SampleClass
    {
        // Naming convention violation: Method name should start with an uppercase letter
        public void wrongMethodName()
        {
            // Unused variable - triggering an 'unused variable' rule violation
            int unusedVariable = 10;

            // Magic number - triggering a 'magic number' rule violation
            int result = 5 * 2;

            // This line contains an empty statement, triggering an 'empty statement' rule violation
            ;

            // Commented-out code - triggering a 'commented-out code' rule violation
            // int someValue = 100;

            // Code duplication - triggering a 'code duplication' rule violation
            int x = 5;
            int y = 5;
            int z = x + y;

            // Nested if-else statements - triggering a 'deeply nested control structure' rule violation
            if (x > 0)
            {
                if (y > 0)
                {
                    if (z > 0)
                    {
                        Console.WriteLine("Deeply nested if-else statements.");
                    }
                }
            }
        }
    }
}