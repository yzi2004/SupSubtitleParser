using System.IO;

namespace SupSubtitleParser
{
    internal class StreamWriters : StreamWriter
    {
        public StreamWriters(string path) : base(path)
        {
        }
    }
}