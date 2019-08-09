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
            c1.PrintId();
            c1.PrintId();
            c1.PrintId(45);
            c1.PrintId(43);
            Console.ReadKey();
        }
    }
}
