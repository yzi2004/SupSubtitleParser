﻿using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace SupSubtitleParser
{
    public class Utils
    {

        public static string FormatDatetime(UInt32 times)
        {
            var ms = times / 90;
            var MS = ms % 1000;
            var sec = ms / 1000;

            return $"{(sec / 3600).ToString("00")}:{((sec % 3600) / 60).ToString("00")}:{(sec % 60).ToString("00")},{MS}";
        }

        public static string GetAppName()
        {
            return AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "");
        }

        public static Version GetAppVer()
        {
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        public static Color? TryParseColor(string stringColor)
        {
            if (stringColor.StartsWith("#") && stringColor.Length == 7) //#ffffff
            {
                if (int.TryParse(stringColor.Substring(1, 2), NumberStyles.HexNumber, null, out var r) &&
                        int.TryParse(stringColor.Substring(3, 2), NumberStyles.HexNumber, null, out int g) &&
                       int.TryParse(stringColor.Substring(5, 2), NumberStyles.HexNumber, null, out int b))
                {
                    return Color.FromArgb(r, g, b);
                }
                else
                {
                    return null;
                }
            }

            ColorConverter colorConv = new ColorConverter(); //named color 
            try
            {
                return Color.FromName(stringColor);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetOutputFolder(string inputFile)
        {
            FileInfo fi = new FileInfo(inputFile);
            string path = $"{fi.DirectoryName.TrimEnd('\\')}\\subtitle";
            while (Directory.Exists(path))
            {
                path = $"{fi.DirectoryName.TrimEnd('\\')}\\subtitle_{Path.GetRandomFileName().Substring(0, 4)}";
            }

            Directory.CreateDirectory(path);

            return path;
        }
    }
}
