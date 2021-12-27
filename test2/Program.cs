using System;
using System.Collections;

namespace test2
{
    class Program
    {
        static void Main(string[] args)
        {

            int n = (int)Math.Log(1234567, 2) + 1;
            Console.WriteLine($"Hello World!" + n);
            Console.ReadKey();
        }
    }
}
