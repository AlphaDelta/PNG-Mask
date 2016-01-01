using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PNGMask
{
    public abstract class XOR : SteganographyProvider
    {
        public XOR() { }
        public XOR(Stream svector, bool find = true) : base(svector, find) { }
        public XOR(string fvector, bool find = true) : base(fvector, find) { }
        public XOR(byte[] bvector, bool find = true) : base(bvector, find) { }
        public XOR(PNG pngvector, bool find = true) : base(pngvector, find) { }

        protected byte[] key = null, vector = null;
        protected PNG image;
        public override void ProcessData(byte[] s, bool find = true)
        {
            using (MemoryStream stream = new MemoryStream(s))
                image = new PNG(stream);
        }

        public override byte[] ExtractData()
        {
            //if (key == null) throw new PNGMaskException("PNG has no data to extract.");
            if (key == null) return null;
            byte[] tempdata = (byte[])vector.Clone();

            for (int i = 0; i < tempdata.Length; i++)
                tempdata[i] ^= key[i % key.Length];

            return tempdata;
        }

        public override void ImprintData(byte[] data)
        {
            string pass = SteganographyProvider.AskPassword();
            if (pass != null && pass.Length > 0)
                PrepareKey(Encoding.UTF8.GetBytes(pass));

            for (int i = 0; i < data.Length; i++)
                data[i] ^= key[i % key.Length];

            vector = data;

            image.RemoveNonCritical();
            ImprintPNG(data);
        }

        protected abstract void ImprintPNG(byte[] data);

        public override void WriteToStream(Stream s)
        {
            image.WriteToStream(s);
        }

        protected ISAAC rnd = null;
        protected void PrepareKey(byte[] pass)
        {
            rnd = SteganographyProvider.PrepareISAAC(pass);

            for (int i = 0, isaac = 0; i < key.Length; i++, isaac++)
            {
                if (isaac >= ISAAC.SIZE)
                {
                    isaac = 0;
                    rnd.Isaac();
                }

                key[i] = (byte)(key[i] ^ rnd.rsl[isaac]);
            }
        }
    }
}
