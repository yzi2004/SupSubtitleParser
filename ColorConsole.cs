using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupSubtitleParser
{
    public static class ColorConsole
    {
        public static void ColorWrite(string value, int theme = 0)
        {
            SetTheme(theme);
            Console.Write(value);
            Console.ResetColor();
        }
        public static void WriteLine(string value, int theme = 0)
        {
            SetTheme(theme);
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void SetTheme(int theme)
        {
            switch (theme)
            {
                case 0:
                    return;
                case 1:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 4:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;

            }
        }

        public static void Reset()
        {
            Console.ResetColor();
        }

    }
}
