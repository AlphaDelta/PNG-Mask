using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PNGMask.Providers
{
    public sealed class XORTEXT : XOR
    {
        public XORTEXT(Stream svector, bool find = true) : base(svector, find) { }
        public XORTEXT(string fvector, bool find = true) : base(fvector, find) { }
        public XORTEXT(byte[] bvector, bool find = true) : base(bvector, find) { }

        public XORTEXT(PNG png, bool find = true)
        {
            image = png;

            ProcessPNG(find);
        }

        public override void ProcessData(byte[] s, bool find = true)
        {
            base.ProcessData(s);

            ProcessPNG(find);
        }

        void ProcessPNG(bool find = true)
        {
            foreach (PNGChunk chunk in image.Chunks)
                if (chunk.Name == "IDAT")
                {
                    key = (byte[])chunk.Data.Clone();
                    break;
                }
            if (key == null) throw new PNGMaskException("PNG has no IDAT chunk for the SteganographyProvider to process.");

            if (find)
            {
                foreach (PNGChunk chunk in image.Chunks)
                    if (chunk.Name == "tEXt")
                    {
                        vector = (byte[])chunk.Data.Clone();

                        string pass = SteganographyProvider.AskPassword();
                        if (pass != null && pass.Length > 0)
                            PrepareKey(Encoding.UTF8.GetBytes(pass));
                        break;
                    }
            }
        }

        static byte[] PNG_TEXT_HEADER = { 0x74, 0x45, 0x58, 0x74 };
        protected override void ImprintPNG(byte[] data)
        {
            uint[] crcTable = null;
            uint crc = PNG.CRC32(PNG_TEXT_HEADER, 0, PNG_TEXT_HEADER.Length, 0, ref crcTable);
            crc = PNG.CRC32(data, 0, data.Length, crc, ref crcTable);
            byte[] crcb = BitConverter.GetBytes(crc);

            image.Chunks.Insert(1, new PNGChunk() { Name = "tEXt", Standard = false, Critical = false, CRC = crc, CRCBytes = new byte[4] { crcb[3], crcb[2], crcb[1], crcb[0] }, ValidCRC = true, Data = data });
        }
    }
}
