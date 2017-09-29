using System;
using System.IO;

namespace SupSubtitleParser
{
    public static class BigEndianBinaryReaderExtension
    {
        public static UInt16 ReadTwoBytes(this BinaryReader reader)
        {
            byte[] buffer = reader.ReadBytes(2);

            if (buffer.Length < 2)
            {
                return 0;
            }

            return (UInt16)((buffer[0] << 8) + buffer[1]);
        }

        public static UInt32 ReadThreeBytes(this BinaryReader reader)
        {
            byte[] buffer = reader.ReadBytes(3);

            if (buffer.Length < 3)
            {
                return 0;
            }

            return (uint)((buffer[0] << 16) + (buffer[1] << 8) + buffer[2]);
        }

        public static UInt32 ReadFourBytes(this BinaryReader reader)
        {
            byte[] buffer = reader.ReadBytes(4);

            if (buffer.Length < 4)
            {
                return 0;
            }

            return (uint)((buffer[0] << 24) + (buffer[1] << 16) + (buffer[2] << 8) + (buffer[3]));
        }

        public static bool EOF(this BinaryReader binaryReader)
        {
            var bs = binaryReader.BaseStream;
            return (bs.Position == bs.Length);
        }

        public static bool Back(this BinaryReader binaryReader, int Count)
        {
            if (!binaryReader.BaseStream.CanSeek)
            {
                return false;
            }

            if (binaryReader.BaseStream.Position <= Count)
            {
                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                binaryReader.BaseStream.Seek(Count * -1, SeekOrigin.Current);
            }

            return true;
        }
    }
}
