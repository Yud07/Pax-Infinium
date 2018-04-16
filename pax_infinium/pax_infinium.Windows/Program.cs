using System;

namespace pax_infinium
{
#if WINDOWS || LINUX
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
            /*while (true)
            {
                try
                {*/
                    using (var game = new Game1())
                        game.Run();
                /*}
                catch
                {
                    // Do nothing
                }
            }*/
        }
    }
#endif
}
