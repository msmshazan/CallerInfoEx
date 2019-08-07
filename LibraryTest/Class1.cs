using System;
using CallerInfoEx;

namespace LibraryTest
{
    public class Class1
    {
        public void Space([CallerInfoExID] ulong? id = null)
        {
            
        }

        public void Space1(int p ,[CallerInfoExID] ulong? id = null)
        {

        }
    }
}
