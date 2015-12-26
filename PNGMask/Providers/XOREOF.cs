using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PNGMask.Providers
{
    public sealed class XOREOF : XOR
    {
        public XOREOF(Stream svector, bool find = true) : base(svector, find) { }
        public XOREOF(string fvector, bool find = true) : base(fvector, find) { }
        public XOREOF(byte[] bvector, bool find = true) : base(bvector, find) { }

        public XOREOF(PNG png, bool find = true)
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
                PNGChunk eof = image.Chunks[image.Chunks.Count - 1];
                if (eof.Name == "_EOF")
                {
                    vector = (byte[])eof.Data.Clone();

                    string pass = SteganographyProvider.AskPassword();
                    if (pass != null && pass.Length > 0)
                        PrepareKey(Encoding.UTF8.GetBytes(pass));
                }
            }
        }

        protected override void ImprintPNG(byte[] data)
        {
            image.Chunks.Add(new PNGChunk() { Name = "_EOF", Standard = false, Critical = false, CRC = 0, CRCBytes = new byte[4] { 0x00, 0x00, 0x00, 0x00 }, ValidCRC = false, Data = data });
        }
    }
}
