/***
 *  http://blog.thescorpius.com/index.php/2017/07/15/presentation-graphic-stream-sup-files-bluray-subtitle-format/
*/

using System;
using System.Collections.Generic;
using System.Drawing;

namespace SupSubtitleParser
{
    public class Segment
    {
        /// <summary>
        /// Presentation Timestamp
        /// </summary>
        public UInt32 PTS { get; set; }

        /// <summary>
        /// Decoding Timestamp
        /// </summary>
        public UInt32 DTS { get; set; }

        /// <summary>
        /// 0x14: PDS
        /// 0x15: ODS
        /// 0x16: PCS
        /// 0x17: WDS
        /// 0x80: END
        /// </summary>
        public SegType Type { get; set; }

        /// <summary>
        /// Size of the segment
        /// </summary>
        public UInt16 DataSize { get; set; }

        public Segment()
        {

        }

        public Segment(Segment seg)
        {
            this.DTS = seg.DTS;
            this.PTS = seg.PTS;
            this.DataSize = seg.DataSize;
            this.Type = seg.Type;
        }
    }

    /// <summary>
    /// Presentation Composition Segment 
    /// </summary>
    public class PCSData : Segment
    {
        /// <summary>
        /// Video width in pixels 
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Video height in pixels
        /// </summary>
        public UInt16 Height { get; set; }

        /// <summary>
        /// Always 0x10. Can be ignored.
        /// </summary>
        public UInt16 FrameRate { get; set; }

        /// <summary>
        /// Composition Number
        /// </summary>
        public UInt16 CompNum { get; set; }

        /// <summary>
        /// Composition State
        /// </summary>
        public CompState CompState { get; set; }

        /// <summary>
        /// Palette Update Flag
        /// </summary>
        public UInt16 PaletteUpdFlag { get; set; }

        /// <summary>
        /// ID of the palette to be used in the Palette only Display Update
        /// </summary>
        public UInt16 PaletteID { get; set; }

        /// <summary>
        /// Number of Composition Objects
        /// </summary>
        public UInt16 CompObjCount { get; set; }

        public List<PCSObject> PCSObjects { get; set; }

        public PCSData()
        {
            this.Type = SegType.PCS;
        }

        public PCSData(Segment seg) : base(seg)
        {

        }
    }

    public class PCSObject
    {
        /// <summary>
        /// ID of the ODS segment that defines the image to be shown
        /// </summary>
        public UInt16 ObjectID { get; set; }

        /// <summary>
        /// Id of the WDS segment to which the image is allocated in the PCS. Up to two images may be assigned to one window
        /// </summary>
        public UInt16 WindowID { get; set; }
        /// <summary>
        /// Object Cropped Flag
        /// 0x40: Force display of the cropped image object
        /// 0x00: Off
        /// </summary>
        public bool ObjCroppedFlag { get; set; }

        /// <summary>
        /// Point from the top left pixel of the image on the screen
        /// </summary>
        public Point Origin { get; set; }
        /// <summary>
        /// Point from the top left pixel of the cropped object in the screen. 
        /// Only used when the Object Cropped Flag is set to 0x40.
        /// </summary>
        public Point? CropOrigin { get; set; }

        /// <summary>
        /// Size of the cropped object in the screen.
        /// Only used when the Object Cropped Flag is set to 0x40.
        /// </summary>
        public Size? CropSize { get; set; }
    }

    /// <summary>
    /// Window Definition Segment
    /// </summary>
    public class WDSData : Segment
    {
        public UInt16 ObjectCount { get; set; }

        public List<WDSObject> WDSObjects { get; set; }

        public WDSData()
        {
            Type = SegType.WDS;
        }

        public WDSData(Segment seg) : base(seg)
        {

        }
    }

    public class WDSObject
    {
        /// <summary>
        /// 	ID of this window
        /// </summary>
        public UInt16 WindowsID { get; set; }

        /// <summary>
        /// Origin from the top left pixel of the window in the screen.
        /// </summary>
        public Point Origin { get; set; }

        /// <summary>
        /// Size of the window
        /// </summary>
        public Size Size { get; set; }
    }


    /// <summary>
    /// Palette Definition Segment 
    /// </summary>
    public class PDSData : Segment
    {
        /// <summary>
        /// 	ID of the palette
        /// </summary>
        public UInt16 PaletteID { get; set; }

        /// <summary>
        /// Version of this palette within the Epoch
        /// </summary>
        public UInt16 PaletteVer { get; set; }

        public List<PDSEntryObject> EntryObjects { get; set; }


        public PDSData()
        {
            Type = SegType.PDS;
        }

        public PDSData(Segment seg) : base(seg)
        {
        }
    }

    public class PDSEntryObject
    {
        /// <summary>
        ///	Entry number of the palette
        /// </summary>
        public UInt16 PaletteEntryID { get; set; }

        /// <summary>
        /// Luminance (Y value)
        /// </summary>
        public UInt16 Luminance { get; set; }

        /// <summary>
        /// Color Difference Red (Cr value)
        /// </summary>
        public UInt16 ColorDifferenceRed { get; set; }

        /// <summary>
        /// Color Difference Blue (Cb value)
        /// </summary>
        public UInt16 ColorDifferenceBlue { get; set; }

        /// <summary>
        ///	Transparency (Alpha value)
        /// </summary>
        public UInt16 Transparency { get; set; }
    }

    public class ODSData : Segment
    {
        /// <summary>
        /// ID of this object
        /// </summary>
        public UInt16 ObjectID { get; set; }

        /// <summary>
        /// Version of this object
        /// </summary>
        public UInt16 ObjectVer { get; set; }

        /// <summary>
        /// If the image is split into a series of consecutive fragments, the last fragment has this flag set. Possible values:
        /// 0x40: Last in sequence
        /// 0x80: First in sequence
        /// 0xC0: First and last in sequence(0x40 | 0x80)
        /// </summary>
        public UInt16 SeqFlag { get; set; }

        /// <summary>
        /// The length of the Run-length Encoding (RLE) data buffer with the compressed image data.
        /// </summary>
        public UInt32 ObjDataLength { get; set; }

        /// <summary>
        /// Size of the image
        /// </summary>
        public Size ImageSize { get; set; }

        /// <summary>
        /// This is the image data compressed using Run-length Encoding (RLE). 
        /// The size of the data is defined in the Object Data Length field.
        /// </summary>
        public byte[] ObjectData { get; set; }

        public ODSData()
        {
            Type = SegType.ODS;
        }

        public ODSData(Segment seg) : base(seg)
        {

        }
    }

    public class ENDData : Segment
    {
        public ENDData()
        {
            Type = SegType.END;
        }

        public ENDData(Segment seg) : base(seg)
        {

        }
    }
}
