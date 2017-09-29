using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace SupSubtitleParser
{
    public class Options
    {
        public string inputFile { get; set; }
        public string outputPath { get; set; }
        public ImageFormat format { get; set; } = ImageFormat.Jpeg;
        public Color bgColor { get; set; } = Color.Transparent;

        public static Options TryParse(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return null;
            }

            if (args[0].StartsWith("-"))
            {
                string arg = args[0].ToLower();
                switch (arg)
                {
                    case "-v":
                    case "--version":
                        PrintVer();
                        return null;
                    case "-h":
                    case "--help":

                        PrintUsage();
                        return null;
                    default:
                        PrintUsage();
                        return null;
                }
            }

            Options opts = new Options();
            opts.inputFile = args[0];

            for (int i = 1; i < args.Length; i += 2)
            {
                if (args.Length < i + 1)
                {
                    PrintError($"no value of arg[{args[i]}].");
                    return null;
                }

                string arg = args[i].ToLower();
                switch (arg)
                {
                    case "-o":
                    case "--out":
                        opts.outputPath = args[i + 1].TrimEnd('\\');
                        break;
                    case "-f":
                    case "--format":
                        var fmt = args[i + 1].ToLower();
                        switch (fmt)
                        {
                            case "bmp":
                                opts.format = ImageFormat.Bmp;
                                break;
                            case "jpeg":
                                opts.format = ImageFormat.Jpeg;
                                break;
                            case "gif":
                                opts.format = ImageFormat.Gif;
                                break;
                            case "tiff":
                                opts.format = ImageFormat.Tiff;
                                break;
                            case "png":
                                opts.format = ImageFormat.Png;
                                break;
                            default:
                                PrintError($"image format is not valid.");
                                return null;
                        }
                        break;
                    case "-b":
                    case "--bgcolor":
                        var color = Utils.TryParseColor(args[i + 1]);
                        if (color == null || !color.HasValue)
                        {
                            PrintError($"The bgcolor value[{args[i + 1]}] is not valid.");
                            return null;
                        }
                        else
                        {
                            opts.bgColor = color.Value;
                        }
                        break;
                    default:
                        PrintUsage();
                        return null;
                }
            }

            return opts;
        }

        public string GetImgExt()
        {
            return format.ToString().ToLower().Replace("jpeg", "jpg");
        }

        private static void PrintError(string message)
        {
            ColorConsole.WriteLine(message, 3);
            Console.WriteLine();
            PrintUsage();
        }

        private static void PrintUsage()
        {
            PrintVer();

            Console.WriteLine($"Usage: {Utils.GetAppName()} input_file -o|-out <path> -f|-format {{bmp|jpeg|gif|tiff|png}} -b|-bgcolor <color>");
            Console.WriteLine("  input_file\t: subtitle file for parse (*.sup) ");
            Console.WriteLine("  -o,--out\t: folder path for time line and images file to save.");
            Console.WriteLine("  -f,--output\t: timage format. (default:jpeg)");
            Console.WriteLine("  -b,--bgcolor\t: image backgroud color.");
            Console.WriteLine("              \t:   named color (like:red lightyellow) or hex rgb value (like:#ff0000 ).");
            Console.WriteLine("              \t:   (default:transparent)");
            Console.WriteLine();
        }

        private static void PrintVer()
        {
            Version ver = Utils.GetAppVer();
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(ver.Build);
            Console.WriteLine($"{Utils.GetAppName()} {ver.Major}.{ver.Minor}-{buildDate.ToString("yyyyMMdd")}\r\n");
        }
    }
}
