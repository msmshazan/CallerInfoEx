using System;
using System.Runtime.CompilerServices;
namespace LibraryTest
{
    public class Class1
    {
        public void Test2(int p,float q)
        {

        }
        public void lotsofargs([CallerInfoEx.ID]ulong? a = null, [CallerInfoEx.ID]ulong? b = null, [CallerInfoEx.ID]ulong? c = null, [CallerInfoEx.ID]ulong? d = null, [CallerInfoEx.ID]ulong? e = null)
        {

        }
        public void PrintAllArguments([CallerInfoEx.ID] ulong? id0 = null)
        {
            Console.WriteLine("id = " + id0);
        }

        public void PrintAllArguments(ulong? otherParam0 , [CallerInfoEx.ID] ulong? id0 = null)
        {
            Console.WriteLine("id = " + id0);
        }

        public void PrintAllArguments(ulong? otherParam0, [CallerInfoEx.ID] ulong? id0 = null , ulong? id1 = null)
        {
            Console.WriteLine("id = " + id0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PrintAllArgumentsInlined(ulong? otherParam0, [CallerInfoEx.ID] ulong? id0 = null)
        {
            Console.WriteLine("id = " + id0);
        }

    }
}
