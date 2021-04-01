using System.Drawing;
using System.Drawing.Imaging;

namespace SupSubtitleParser
{
    public class AppSettings
    {
        public string inputFile { get; set; }
        public string outputPath { get; set; }
        public ImageFormat imageFormat { get; set; }
        public Color BGColor { get; set; } = Color.Transparent;

        public Border border { get; set; } = new Border();

        public string GetImgExt()
        {
            return imageFormat.ToString().ToLower().Replace("jpeg", "jpg");
        }
    }

    public class Border
    {
        public Color color { get; set; } = Color.Transparent;
        public int Width { get; set; }
        public int Padding { get; set; }

        public bool HasBorder => Padding > 0 || Width > 0;

        public int EdgeWidth => Width + Padding;
    }
}
