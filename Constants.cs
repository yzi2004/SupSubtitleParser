using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupSubtitleParser
{
    public enum SegType : UInt16
    {
        PDS = 0x14,
        ODS = 0x15,
        PCS = 0x16,
        WDS = 0x17,
        END = 0x80
    }

    public enum CompState : UInt16
    {
        Normal = 0x00,
        AcquisitionPoint = 0x40,
        EpochStart = 0x80
    }

    public class Constants
    {
    }


}
