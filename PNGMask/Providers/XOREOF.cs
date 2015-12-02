using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PNGMask.Providers
{
    public sealed class XOREOF : XOR
    {
        public XOREOF(Stream svector) : base(svector) { }
        public XOREOF(string fvector) : base(fvector) { }
        public XOREOF(byte[] bvector) : base(bvector) { }

        public XOREOF(PNG png)
        {
            image = png;

            ProcessPNG();
        }

        public override void ProcessData(byte[] s)
        {
            base.ProcessData(s);

            ProcessPNG();
        }

        void ProcessPNG()
        {
            foreach (PNGChunk chunk in image.Chunks)
                if (chunk.Name == "IDAT")
                {
                    key = (byte[])chunk.Data.Clone();
                    break;
                }
            if (key == null) throw new PNGMaskException("PNG has no IDAT chunk for the SteganographyProvider to process.");

            PNGChunk eof = image.Chunks[image.Chunks.Count - 1];
            if (eof.Name == "_EOF")
            {
                vector = (byte[])eof.Data.Clone();

                string pass = SteganographyProvider.AskPassword();
                if (pass != null && pass.Length > 0)
                    PrepareKey(Encoding.UTF8.GetBytes(pass));
            }
        }

        protected override void ImprintPNG(byte[] data)
        {
            image.Chunks.Add(new PNGChunk() { Name = "_EOF", Standard = false, Critical = false, CRC = 0, CRCBytes = new byte[4] { 0x00, 0x00, 0x00, 0x00 }, ValidCRC = false, Data = data });
        }
    }
}
