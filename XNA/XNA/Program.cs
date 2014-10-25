using System;
using System.IO;

namespace XNA
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Game1 game = new Game1(true))
            {
                game.Run();
            }
        }
    }
#endif
}

