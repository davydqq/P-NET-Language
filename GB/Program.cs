using System;

namespace GB
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new MyGCCollectClass();
            t.Run();
        }
    }

    public class MyGCCollectClass
    {
        private const int maxGarbage = 100000;

        public void Run()
        {
            // Put some objects in memory.
            MakeSomeGarbage();
            Console.WriteLine("Memory used before collection:       {0:N0}", GC.GetTotalMemory(false));

            // Collect all generations of memory.
            // GC.Collect();
            Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
        }

        static void MakeSomeGarbage()
        {
            Version vt;
            // Create objects and release them to fill up memory with unused objects.
            for (int i = 0; i < maxGarbage; i++)
            {
                vt = new Version();
            }
        }
    }
}
