using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SupSubtitleParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = Options.TryParse(args);
            if (options == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(options.inputFile))
            {
                Console.WriteLine("Input file is not set.");
                return;
            }

            if (!File.Exists(options.inputFile))
            {
                Console.WriteLine("Input file is not exists.");
                return;
            }

            if (string.IsNullOrWhiteSpace(options.outputPath))
            {
                options.outputPath = Utils.GetOutputFolder(options.inputFile);

            }
            else if (!Directory.Exists(options.outputPath))
            {
                try
                {
                    Directory.CreateDirectory(options.outputPath);
                }
                catch (Exception)
                {
                    Console.WriteLine("Can't create output folder.");
                    return;
                }
            }
            else if (Directory.EnumerateFileSystemEntries(options.outputPath).Any())
            {
                Console.WriteLine("output folder is not empty.");
                return;
            }

            SegmentParser parser = new SegmentParser();
            ImageDecoder decoder = new ImageDecoder();
            string timeLineFile = $"{options.outputPath}\\timeline.txt";
            StreamWriter sw = new StreamWriter(timeLineFile);
            StringBuilder sbImgs = new StringBuilder();

            using (BinaryReader br = new BinaryReader(new FileStream(options.inputFile, FileMode.Open)))
            {
                var lstDS = parser.Parse(br);
                int dsIdx = 0;
                int objIdx = 0;

                if (lstDS == null)
                {
                    return;
                }

                DisplaySet startDS = null;

                lstDS.ForEach(ds =>
                {
                    if (ds.PCSData.CompState == CompState.Normal)
                    {
                        if (ds != null)
                        {
                            sw.WriteLine(dsIdx);
                            sw.WriteLine(Utils.FormatDatetime(startDS.PCSData.PTS) + " - " + Utils.FormatDatetime(ds.PCSData.PTS));
                            sw.WriteLine(sbImgs.ToString());
                            sbImgs = new StringBuilder();
                        }
                        return;
                    }

                    startDS = ds;

                    Console.Write("+");

                    var ptn = ds.PDSDatas.Where(t => t.PaletteID == ds.PCSData.PaletteID).FirstOrDefault();
                    if (ptn == null && ds.PDSDatas.Count > 0)
                    {
                        ptn = ds.PDSDatas[0];
                    }
                    dsIdx++;
                    objIdx = 0;
                    ds.PCSData.PCSObjects.ForEach(pcsObj =>
                    {
                        var obj = ds.ODSDatas.Where(t => t.ObjectID == pcsObj.ObjectID).FirstOrDefault();

                        if (obj != null)
                        {
                            string fileName = $"{(dsIdx).ToString()}_{(++objIdx).ToString()}.{options.GetImgExt()}";
                            sbImgs.AppendLine(fileName);
                            var bmp = decoder.Decode(obj.ObjectData, obj.ImageSize, ptn.EntryObjects, options.bgColor);
                            bmp.Save($"{options.outputPath}\\{fileName}", options.format);
                        }
                    });
                });
            }

            sw.Close();
        }


    }
}
