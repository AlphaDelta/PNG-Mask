using System;
using System.Collections.Generic;
using System.Text;

namespace PNGMask
{
    public struct PNGChunk
    {
        public string Name;
        public bool Critical, Standard, ValidCRC;
        public uint CRC;
        public byte[] Data, CRCBytes;
    }
}
