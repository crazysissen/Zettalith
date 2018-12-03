using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Zettalith
{
    static class Test
    {
        public static string Category { private get; set; }

        public static void Log(string format, params object[] args)
        {
            if (Category != "")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Category + ": ");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine(format, args);
        }

        public static void Log(object obj) => Console.WriteLine(obj);
    }
}
