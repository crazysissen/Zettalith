using System;
using System.Threading;

namespace Zettalith
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new XNAController())
                    game.Run();
            }
            catch
            {
                LoadGame.KillThread();
            }
        }
    }
}
