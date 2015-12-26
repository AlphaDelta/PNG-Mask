using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace PNGMask
{
    public abstract class SteganographyProvider
    {
        public SteganographyProvider() { }

        public SteganographyProvider(Stream svector, bool find = true)
        {
            if (svector.CanTimeout) throw new PNGMaskException("Stream must not be able to time-out");
            if (!svector.CanRead) throw new PNGMaskException("Stream must be readable");
            if (!svector.CanSeek) throw new PNGMaskException("Stream must be seekable");

            byte[] buffer = new byte[svector.Length];
            svector.Seek(0, SeekOrigin.Begin);
            if (svector.Read(buffer, 0, buffer.Length) != buffer.Length) throw new PNGMaskException("Could not read entire stream");
            ProcessData(buffer, find);
        }
        public SteganographyProvider(string fvector, bool find = true)
        {
            if (!File.Exists(fvector)) throw new IOException("File '" + fvector + "' could not be found");

            ProcessData(File.ReadAllBytes(fvector), find);
        }

        public SteganographyProvider(byte[] bvector, bool find = true) { ProcessData(bvector); }

        public SteganographyProvider(PNG pngvector, bool find = true)
        {
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                pngvector.WriteToStream(ms);
                data = ms.ToArray();
            }
            ProcessData(data, find);
        }

        public abstract void ProcessData(byte[] s, bool find = true);

        public static string AskPassword(bool CanPasswordBeEmpty = true)
        {
            using (PasswordEntry entry = new PasswordEntry(CanPasswordBeEmpty))
            {
                entry.ShowDialog();

                if (entry.Canceled) return null;

                return entry.Password;
            }
        }

        static byte[]
        HEADER_TXT = { 0x54, 0x58, 0x54 }, //Text
        HEADER_BIN = { 0x42, 0x49, 0x4E }, //Binary
        HEADER_IMG = { 0x49, 0x4D, 0x47 }, //Image
        HEADER_IDX = { 0x49, 0x44, 0x58 }, //Index
        HEADER_IDI = { 0x49, 0x44, 0x49 }, //Index - Image array (4b image length, < image data)
        HEADER_IDL = { 0x49, 0x44, 0x4C }; //Index - Link data array (4b image index, 4b title length, < title string, 4b URL length, < URL string)

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
                    LinkIndex index = (LinkIndex)obj;
                    byte[] buffer;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        /* Image array serialization */
                        using (MemoryStream imgliststream = new MemoryStream())
                        {
                            foreach (Image img in index.Images)
                                using (MemoryStream imgstream = new MemoryStream())
                                {
                                    img.Save(imgstream, System.Drawing.Imaging.ImageFormat.Png);

                                    buffer = BitConverter.GetBytes((int)imgstream.Length);
                                    imgliststream.Write(buffer, 0, buffer.Length);
                                    buffer = imgstream.ToArray();
                                    imgliststream.Write(buffer, 0, buffer.Length);
                                }

                            if (imgliststream.Length > 0)
                            {
                                buffer = BitConverter.GetBytes((int)imgliststream.Length);
                                stream.Write(buffer, 0, buffer.Length);
                                stream.Write(HEADER_IDI, 0, HEADER_IDI.Length);
                                buffer = imgliststream.ToArray();
                                stream.Write(buffer, 0, buffer.Length);
                            }
                        }

                        /* Link item array serialization */
                        using (MemoryStream linkliststream = new MemoryStream())
                        {
                            foreach (LinkIndexRow row in index.Rows)
                            {
                                //Image index (4b integer)
                                buffer = BitConverter.GetBytes(row.ImageIndex);
                                linkliststream.Write(buffer, 0, buffer.Length);

                                //Title (4b integer (string length), < string)
                                byte[] charbuffer = Encoding.UTF8.GetBytes(row.Title);
                                buffer = BitConverter.GetBytes(charbuffer.Length);
                                linkliststream.Write(buffer, 0, buffer.Length);
                                linkliststream.Write(charbuffer, 0, charbuffer.Length);

                                //Title (4b integer (string length), < string)
                                charbuffer = Encoding.UTF8.GetBytes(row.URL);
                                buffer = BitConverter.GetBytes(charbuffer.Length);
                                linkliststream.Write(buffer, 0, buffer.Length);
                                linkliststream.Write(charbuffer, 0, charbuffer.Length);
                            }

                            buffer = BitConverter.GetBytes((int)linkliststream.Length);
                            stream.Write(buffer, 0, buffer.Length);
                            stream.Write(HEADER_IDL, 0, HEADER_IDL.Length);
                            buffer = linkliststream.ToArray();
                            stream.Write(buffer, 0, buffer.Length);
                        }

                        data = stream.ToArray();
                    }
                    temp.AddRange(BitConverter.GetBytes(data.Length));
                    temp.AddRange(HEADER_IDX);
                    temp.AddRange(data);
                    break;
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
                case "IDX":
                    using (MemoryStream stream = new MemoryStream(ndata))
                    {
                        LinkIndex lindex = new LinkIndex();

                        byte[] ibuffer = new byte[4], sbuffer = new byte[3];
                        do
                        {
                            if (stream.Read(ibuffer, 0, 4) < 4) throw new PNGMaskException("IDX chunk is corrupt");
                            uint len = BitConverter.ToUInt32(ibuffer, 0);

                            if (stream.Read(sbuffer, 0, 3) < 3) throw new PNGMaskException("IDX chunk is corrupt");
                            string chunkname = Encoding.ASCII.GetString(sbuffer);

                            byte[] dbuffer = new byte[len];
                            if (stream.Read(dbuffer, 0, (int)len) < (int)len) throw new PNGMaskException("IDX chunk is corrupt");
                            using (MemoryStream ms = new MemoryStream(dbuffer))
                            {
                                if (chunkname == "IDI")
                                {
                                    do
                                    {
                                        if (ms.Read(ibuffer, 0, 4) < 4) throw new PNGMaskException("IDI chunk is corrupt");
                                        int clen = BitConverter.ToInt32(ibuffer, 0);

                                        byte[] imgbuffer = new byte[clen];
                                        if (ms.Read(imgbuffer, 0, clen) < clen) throw new PNGMaskException("IDI chunk is corrupt");

                                        using (MemoryStream imgstream = new MemoryStream(imgbuffer))
                                            lindex.Images.Add(Image.FromStream(imgstream));
                                    } while (ms.Position < ms.Length - 1);
                                }
                                else if (chunkname == "IDL")
                                {
                                    do
                                    {
                                        if (ms.Read(ibuffer, 0, 4) < 4) throw new PNGMaskException("IDL chunk is corrupt");
                                        int imgindex = BitConverter.ToInt32(ibuffer, 0);

                                        if (ms.Read(ibuffer, 0, 4) < 4) throw new PNGMaskException("IDL chunk is corrupt");
                                        int slen = BitConverter.ToInt32(ibuffer, 0);

                                        byte[] strbuffer = new byte[slen];
                                        if (ms.Read(strbuffer, 0, slen) < slen) throw new PNGMaskException("IDL chunk is corrupt");
                                        string title = Encoding.UTF8.GetString(strbuffer);

                                        if (ms.Read(ibuffer, 0, 4) < 4) throw new PNGMaskException("IDL chunk is corrupt");
                                        slen = BitConverter.ToInt32(ibuffer, 0);

                                        strbuffer = new byte[slen];
                                        if (ms.Read(strbuffer, 0, slen) < slen) throw new PNGMaskException("IDL chunk is corrupt");
                                        string url = Encoding.UTF8.GetString(strbuffer);

                                        lindex.Rows.Add(new LinkIndexRow(imgindex, title, url));
                                    } while (ms.Position < ms.Length - 1);
                                }
                            }
                        } while (stream.Position < stream.Length - 1);

                        data = lindex;
                    }
                    return DataType.Index;
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
