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
            Process parent = Process.GetProcessById(int.Parse(args[0]));
            parent.Exited += OnParentExit;

            //using (PipeStream client = new AnonymousPipeClientStream(PipeDirection.Out, args[0]))
            //{
            //    using (StreamWriter sw = new StreamWriter(client))
            //    {
                    //sw.AutoFlush = true;

            List<string> arguments = new List<string>();
            int currentLine = 2;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.White;

                string input = Console.ReadLine();

                arguments.Add(input);

                Console.SetCursorPosition(0, ++currentLine);
                Console.WriteLine(input);
                Console.SetCursorPosition(0, 0);
                Console.Write("  ".PadLeft(input.Length * 2));
                Console.SetCursorPosition(0, 0);
            }

            //    }
            //}
        }

        static void OnParentExit(object s, EventArgs a)
        {
            Process.GetCurrentProcess().Close();
        }
    }
}
