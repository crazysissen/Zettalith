using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ZettalithDebugConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (PipeStream client = new AnonymousPipeClientStream(PipeDirection.Out, args[0].Split(':')[1]))
            {
                using (StreamWriter sw = new StreamWriter(client))
                {
                    sw.AutoFlush = true;

                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("> ");
                        Console.ForegroundColor = ConsoleColor.White;

                        sw.WriteLine(Console.ReadLine());
                    }
                }
            }
        }
    }
}
