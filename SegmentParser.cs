﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SupSubtitleParser
{
    public class SegmentParser
    {
        public List<DisplaySet> Parse(BinaryReader br)
        {
            try
            {
                List<DisplaySet> displaySets = new List<DisplaySet>();
                DisplaySet set = new DisplaySet();
                while (!br.EOF())
                {
                    var segment = Read(br);
                    if (segment is PCSData)
                    {
                        if (set.PCSData != null)
                        {
                            throw new Exception();
                        }
                        set.PCSData = (PCSData)segment;
                    }
                    else if (segment is WDSData)
                    {
                        set.WDSDatas.Add((WDSData)segment);
                    }
                    else if (segment is PDSData)
                    {
                        set.PDSDatas.Add((PDSData)segment);
                    }
                    else if (segment is ODSData)
                    {
                        set.ODSDatas.Add((ODSData)segment);
                    }
                    else if (segment is ENDData)
                    {
                        displaySets.Add(set);
                        set = new DisplaySet();
                    }
                    else
                    {
                        set = new DisplaySet();
                    }
                }

                return displaySets;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Segment Read(BinaryReader binaryReader)
        {
            if (!CheckMagicNumber(binaryReader, false))
            {
                return null;
            }

            var heaser = ReadSegHeader(binaryReader);

            switch (heaser.Type)
            {

                case SegType.PCS:
                    return ReadPCSData(binaryReader, heaser);
                case SegType.WDS:
                    return ReadWDSData(binaryReader, heaser);
                case SegType.PDS:
                    return ReadPDSData(binaryReader, heaser);
                case SegType.ODS:
                    return ReadODSData(binaryReader, heaser);
                case SegType.END:
                    return new ENDData(heaser);
                default:
                    return heaser;
            }
        }

        private Segment ReadSegHeader(BinaryReader binaryReader)
        {
            Segment seg = new Segment();
            seg.PTS = binaryReader.ReadFourBytes();
            seg.DTS = binaryReader.ReadFourBytes();
            seg.Type = (SegType)(binaryReader.ReadByte());
            seg.DataSize = binaryReader.ReadTwoBytes();

            return seg;
        }

        private PCSData ReadPCSData(BinaryReader binaryReader, Segment header)
        {
            PCSData data = new PCSData(header);
            data.Size = new Size(binaryReader.ReadTwoBytes(), binaryReader.ReadTwoBytes());
            data.FrameRate = binaryReader.ReadByte();
            data.CompNum = binaryReader.ReadTwoBytes();
            data.CompState = (CompState)(binaryReader.ReadByte());
            data.PaletteUpdFlag = binaryReader.ReadByte();
            data.PaletteID = binaryReader.ReadByte();
            data.CompObjCount = binaryReader.ReadByte();

            if (data.CompObjCount > 0)
            {
                data.PCSObjects = new List<PCSObject>();
                for (int i = 0; i < data.CompObjCount; i++)
                {
                    PCSObject compObject = new PCSObject();
                    compObject.ObjectID = binaryReader.ReadTwoBytes();
                    compObject.WindowID = binaryReader.ReadByte();
                    compObject.ObjCroppedFlag = (binaryReader.ReadByte() == 0x40);
                    compObject.Origin = new Point(binaryReader.ReadTwoBytes(), binaryReader.ReadTwoBytes());
                    if (compObject.ObjCroppedFlag)
                    {
                        compObject.CropOrigin = new Point(binaryReader.ReadTwoBytes(), binaryReader.ReadTwoBytes());
                        compObject.CropSize = new Size(binaryReader.ReadTwoBytes(), binaryReader.ReadTwoBytes());
                    }

                    data.PCSObjects.Add(compObject);
                }
            }

            return data;
        }

        private WDSData ReadWDSData(BinaryReader binaryReader, Segment header)
        {
            WDSData data = new WDSData(header);

            data.ObjectCount = binaryReader.ReadByte();

            if (data.ObjectCount > 0)
            {
                data.WDSObjects = new List<WDSObject>();

                for (int i = 0; i < data.ObjectCount; i++)
                {
                    WDSObject obj = new WDSObject();

                    obj.WindowsID = binaryReader.ReadByte();
                    obj.Origin = new Point(binaryReader.ReadTwoBytes(), binaryReader.ReadTwoBytes());
                    obj.Size = new Size(binaryReader.ReadTwoBytes(), binaryReader.ReadTwoBytes());

                    data.WDSObjects.Add(obj);
                }
            }
            return data;
        }

        private PDSData ReadPDSData(BinaryReader binaryReader, Segment header)
        {
            PDSData data = new PDSData(header);
            data.PaletteID = binaryReader.ReadByte();
            data.PaletteVer = binaryReader.ReadByte();
            int entryCount = (header.DataSize - 2) / 5;
            if (entryCount > 0)
            {
                data.EntryObjects = new List<PDSEntryObject>();
                for (int i = 0; i < entryCount; i++)
                {
                    PDSEntryObject obj = new PDSEntryObject();
                    obj.PaletteEntryID = binaryReader.ReadByte();
                    obj.Luminance = binaryReader.ReadByte();
                    obj.ColorDifferenceRed = binaryReader.ReadByte();
                    obj.ColorDifferenceBlue = binaryReader.ReadByte();
                    obj.Transparency = binaryReader.ReadByte();

                    data.EntryObjects.Add(obj);
                }
            }
            return data;
        }

        private ODSData ReadODSData(BinaryReader binaryReader, Segment header)
        {
            ODSData data = new ODSData();
            data.ObjectID = binaryReader.ReadTwoBytes();
            data.ObjectVer = binaryReader.ReadByte();
            data.SeqFlag = binaryReader.ReadByte();
            data.ObjDataLength = binaryReader.ReadThreeBytes();
            data.ImageSize = new Size(binaryReader.ReadTwoBytes(), binaryReader.ReadTwoBytes());
            data.ObjectData = binaryReader.ReadBytes((int)data.ObjDataLength - 4); //4 is Image Size bytes

            return data;
        }

        private bool CheckMagicNumber(BinaryReader binaryReader, bool BackIfFalse = false)
        {
            var bytes = binaryReader.ReadBytes(2);
            if (bytes.Length < 2)
            {
                if (BackIfFalse)
                {
                    binaryReader.Back(bytes.Length);
                }

                return false;
            }

            if (bytes[0] == 0x50 && bytes[1] == 0x47)
            {
                return true;
            }
            else
            {
                if (BackIfFalse)
                {
                    binaryReader.Back(2);
                }
                return false;
            }
        }
    }
}
