using System;

namespace LibraryTest
{
    public class Class1
    {
        public void PrintId([CallerInfoEx.ID] ulong? id = null)
        {
            Console.WriteLine("id = " + id);
        }

        public void PrintId(ulong noAttributeLongParam0 ,[CallerInfoEx.ID] ulong? id = null)
        {
            Console.WriteLine("id = " + id);
        }
    }
}
