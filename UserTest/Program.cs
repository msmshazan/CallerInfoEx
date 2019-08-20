using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryTest;

namespace UserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var c1 = new Class1();
            c1.PrintAllArguments();
            c1.PrintAllArguments();
            c1.PrintAllArguments(54,id1:32);
            c1.PrintAllArguments(45);
            c1.PrintAllArguments(43);
            c1.PrintAllArgumentsInlined(45);
            c1.PrintAllArgumentsInlined(null,43);
            c1.PrintAllArgumentsInlined(32,43);
           // c1.Test2(74, 78.75f);
            c1.lotsofargs();
            Console.ReadKey();
        }
    }
}
