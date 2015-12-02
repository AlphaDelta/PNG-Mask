using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PNGMask
{
    public abstract class SteganographyProvider
    {
        public SteganographyProvider() { }

        public SteganographyProvider(Stream svector)
        {
            if (svector.CanTimeout) throw new PNGMaskException("Stream must not be able to time-out");
            if (!svector.CanRead) throw new PNGMaskException("Stream must be readable");
            if (!svector.CanSeek) throw new PNGMaskException("Stream must be seekable");

            byte[] buffer = new byte[svector.Length];
            svector.Seek(0, SeekOrigin.Begin);
            if (svector.Read(buffer, 0, buffer.Length) != buffer.Length) throw new PNGMaskException("Could not read entire stream");
            ProcessData(buffer);
        }
        public SteganographyProvider(string fvector)
        {
            if (!File.Exists(fvector)) throw new IOException("File '" + fvector + "' could not be found");

            ProcessData(File.ReadAllBytes(fvector));
        }

        public SteganographyProvider(byte[] bvector) { ProcessData(bvector); }

        public SteganographyProvider(PNG pngvector)
        {
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                pngvector.WriteToStream(ms);
                data = ms.ToArray();
            }
            ProcessData(data);
        }

        public abstract void ProcessData(byte[] s);

        public static string AskPassword(bool CanPasswordBeEmpty = true)
        {
            PasswordEntry entry = new PasswordEntry(CanPasswordBeEmpty);
            entry.ShowDialog();

            if (entry.Canceled) return null;

            return entry.Password;
        }

        static byte[]
        HEADER_TXT = { 0x54, 0x58, 0x54 },
        HEADER_BIN = { 0x42, 0x49, 0x4E },
        HEADER_IMG = { 0x49, 0x4D, 0x47 },
        HEADER_IND = { 0x49, 0x4E, 0x44 };

        Random rnd = new Random();
        public void Imprint(DataType type, object obj)
        {
            List<byte> temp = new List<byte>();

            int igarbage = rnd.Next(0x10, 0xFF);
            temp.Add((byte)igarbage);

            byte[] garbage = new byte[igarbage];
            rnd.NextBytes(garbage);
            temp.AddRange(garbage);

            byte[] data;
            switch (type)
            {
                case DataType.Text:
                    data = Encoding.UTF8.GetBytes((string)obj);
                    temp.AddRange(BitConverter.GetBytes(data.Length));
                    temp.AddRange(HEADER_TXT);
                    temp.AddRange(data);
                    break;
                case DataType.Binary:
                    data = (byte[])obj;
                    temp.AddRange(BitConverter.GetBytes(data.Length));
                    temp.AddRange(HEADER_BIN);
                    temp.AddRange(data);
                    break;
                case DataType.ImageBytes:
                    data = (byte[])obj;
                    temp.AddRange(BitConverter.GetBytes(data.Length));
                    temp.AddRange(HEADER_IMG);
                    temp.AddRange(data);
                    break;
                case DataType.Image:
                    using (MemoryStream stream = new MemoryStream())
                    {
                        ((System.Drawing.Image)obj).Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        data = stream.ToArray();
                    }
                    temp.AddRange(BitConverter.GetBytes(data.Length));
                    temp.AddRange(HEADER_IMG);
                    temp.AddRange(data);
                    break;
                case DataType.Index:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }

            data = temp.ToArray();
            temp = null;
            ImprintData(data);
        }
        public abstract void ImprintData(byte[] data);

        public DataType Extract(out object data)
        {
            byte[] bdata = ExtractData();
            if (bdata == null || bdata.Length < 0)
            {
                data = null;
                return DataType.None;
            }
            data = bdata;

            int index = 1;
            index += bdata[0];

            if (index + 7 > bdata.Length) return DataType.Binary;

            int length = BitConverter.ToInt32(bdata, index); index += 4;
            if (length < 0 || index + 3 + length > bdata.Length) return DataType.Binary;

            string name = Encoding.ASCII.GetString(bdata, index, 3); index += 3;

            byte[] ndata = new byte[length];
            for (int i = 0; i < length && index < bdata.Length; i++, index++)
                ndata[i] = bdata[index];
            switch (name)
            {
                case "TXT":
                    data = Encoding.UTF8.GetString(ndata);
                    return DataType.Text;
                case "BIN":
                    data = ndata;
                    return DataType.Binary;
                case "IMG":
                    System.Drawing.Image img;
                    using (MemoryStream ms = new MemoryStream(ndata))
                        img = System.Drawing.Image.FromStream(ms);
                    data = img; //Must be disposed by caller
                    return DataType.Image;
                default:
                    return DataType.Binary;
            }

            return DataType.Binary;
        }
        public abstract byte[] ExtractData();

        public abstract void WriteToStream(Stream s);

        protected static ISAAC PrepareISAAC(byte[] seed)
        {
            ISAAC rnd = new ISAAC();
            rnd.Isaac();
            for (int i = 0; i < ISAAC.SIZE; i++)
                rnd.mem[i] ^= seed[i % seed.Length];
            for (int i = 0; i < 3; i++)
                rnd.Isaac();

            return rnd;
        }
    }
}
