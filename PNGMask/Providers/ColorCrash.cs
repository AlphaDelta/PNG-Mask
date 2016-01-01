using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PNGMask.Providers
{
    public sealed class ColorCrash : Graphical
    {
        public ColorCrash(Stream svector, bool find = true) : base(svector, find) { }
        public ColorCrash(string fvector, bool find = true) : base(fvector, find) { }
        public ColorCrash(byte[] bvector, bool find = true) : base(bvector, find) { }
        public ColorCrash(PNG pngvector, bool find = true) : base(pngvector, find) { }

        ISAAC csprng = null;

        public override void ImprintData(byte[] data)
        {
            string pass = SteganographyProvider.AskPassword();
            if (pass != null && pass.Length > 0)
                csprng = SteganographyProvider.PrepareISAAC(Encoding.UTF8.GetBytes(pass));

            ulong maxsize = (ulong)Math.Floor(((double)BitmapDimensions.Width * (double)BitmapDimensions.Height * 3d * 2d) / 8d);
            if ((ulong)data.LongLength + 4 > maxsize) throw new NotEnoughSpaceException("Not enough space for graphical injection, maximum data size for this image is " + (int)Math.Floor(maxsize / 1024d) + "KB");

            List<byte> tdata = new List<byte>(data);
            byte[] len = BitConverter.GetBytes(data.Length);
            tdata.InsertRange(0, len);
            data = tdata.ToArray();
            tdata = null;

            if(csprng != null)
                for (int i = 0, isaac = 0; i < data.Length; i++, isaac++)
                {
                    if (isaac >= ISAAC.SIZE)
                    {
                        isaac = 0;
                        csprng.Isaac();
                    }

                    data[i] = (byte)(data[i] ^ csprng.rsl[isaac]);
                }

            int temp = 0;
            for (int i = 0, j = 0; i < data.Length && j < BitmapData.Length; i++)
            {
                byte mask1 = (byte)(data[i] & 3);
                byte mask2 = (byte)((data[i] & 12) >> 2);
                byte mask3 = (byte)((data[i] & 48) >> 4);
                byte mask4 = (byte)((data[i] & 192) >> 6);

                BitmapData[j] &= 252;
                BitmapData[j] |= mask1;
                IterateChannel(ref temp, ref j);

                BitmapData[j] &= 252;
                BitmapData[j] |= mask2;
                IterateChannel(ref temp, ref j);

                BitmapData[j] &= 252;
                BitmapData[j] |= mask3;
                IterateChannel(ref temp, ref j);

                BitmapData[j] &= 252;
                BitmapData[j] |= mask4;
                IterateChannel(ref temp, ref j);
            }
        }

        public override byte[] ExtractData()
        {
            string pass = SteganographyProvider.AskPassword();
            if (pass != null && pass.Length > 0)
                csprng = SteganographyProvider.PrepareISAAC(Encoding.UTF8.GetBytes(pass));

            byte[] len = new byte[4];
            int temp = 0;
            int j = 0;
            for (int i = 0; i < 4; i++)
            {
                len[i] |= (byte)(BitmapData[j] & 3);
                IterateChannel(ref temp, ref j);

                len[i] |= (byte)((BitmapData[j] & 3) << 2);
                IterateChannel(ref temp, ref j);

                len[i] |= (byte)((BitmapData[j] & 3) << 4);
                IterateChannel(ref temp, ref j);

                len[i] |= (byte)((BitmapData[j] & 3) << 6);
                IterateChannel(ref temp, ref j);
            }

            int isaac = 0;
            if(csprng != null)
                for (int k = 0; k < len.Length; k++, isaac++)
                    len[k] = (byte)(len[k] ^ csprng.rsl[isaac]);

            int ilen = BitConverter.ToInt32(len, 0);

            ulong maxsize = (ulong)Math.Floor(((double)BitmapDimensions.Width * (double)BitmapDimensions.Height * 3d * 2d) / 8d);
            if (ilen < 1 || (ulong)ilen > maxsize) throw new InvalidPasswordException();

            byte[] data = new byte[ilen];

            for (int i = 0; i < ilen && j < BitmapData.Length; i++)
            {
                data[i] |= (byte)(BitmapData[j] & 3);
                IterateChannel(ref temp, ref j);

                data[i] |= (byte)((BitmapData[j] & 3) << 2);
                IterateChannel(ref temp, ref j);

                data[i] |= (byte)((BitmapData[j] & 3) << 4);
                IterateChannel(ref temp, ref j);

                data[i] |= (byte)((BitmapData[j] & 3) << 6);
                IterateChannel(ref temp, ref j);
            }

            if (csprng != null)
                for (int i = 0; i < data.Length; i++, isaac++)
                {
                    if (isaac >= ISAAC.SIZE)
                    {
                        isaac = 0;
                        csprng.Isaac();
                    }

                    data[i] = (byte)(data[i] ^ csprng.rsl[isaac]);
                }

            return data;
        }

        void IterateChannel(ref int temp, ref int j)
        {
            j++;

            if (bpp == 3) return;
            
            temp++;
            if (temp > 2)
            {
                temp = 0;
                if (BitmapData[j] == 0) BitmapData[j] = 0x01;
                j++;
            }
        }
    }
}
