using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PNGMask
{
    public sealed class PNG
    {
        static byte[] MAGIC_NUMBER = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        static string[] STANDARD_CHUNKS = { "IHDR", "IDAT", "IEND", "acTL", "bkGD", "cHRM", "fcTL", "gAMA", "oFFs", "pHYs", "PLTE", "sBIT", "sCAL", "sRGB", "sTER", "tEXt", "zTXt", "iTXt", "tIME", "tRNS" };
        static string[] CRITICAL_CHUNKS = { "IHDR", "IDAT", "IEND", "PLTE" };

        public List<PNGChunk> Chunks;

        public uint Width, Height;

        public PNG(string file)
        {
            using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                ProcessStream(fs);
        }
        public PNG(Stream data)
        {
            ProcessStream(data);
        }

        void ProcessStream(Stream data)
        {
            if (data.CanTimeout) throw new PNGMaskException("Stream must not be able to time-out");
            if (!data.CanRead) throw new PNGMaskException("Stream must be readable");
            if (!data.CanSeek) throw new PNGMaskException("Stream must be seekable");

            if (data.Length < MAGIC_NUMBER.Length) throw new PNGMaskException("File is smaller than magic number");

            byte[] buffer = new byte[MAGIC_NUMBER.Length];
            if (data.Read(buffer, 0, buffer.Length) < buffer.Length) throw new PNGMaskException("Could not read magic number");

            for (int i = 0; i < buffer.Length; i++)
                if (buffer[i] != MAGIC_NUMBER[i]) throw new PNGMaskException("Incorrect magic number, this file is not a PNG.");

            Chunks = new List<PNGChunk>();
            buffer = new byte[4];
            bool first = true;
            string name;
            do
            {
                ReadData(data, ref buffer);
                SwapEndianness(ref buffer);
                uint length = BitConverter.ToUInt32(buffer, 0);

                ReadData(data, ref buffer);
                byte[] nameb = (byte[])buffer.Clone();
                name = Encoding.ASCII.GetString(buffer);
                if (first)
                {
                    if (name != "IHDR") throw new PNGMaskException("First chunk must be IHDR, PNG is corrupt.");
                    if (length < 8) throw new PNGMaskException("IHDR chunk is too small, PNG is corrupt");
                    first = false;
                }

                byte[] chunkdata = new byte[length];
                ReadData(data, ref chunkdata);

                ReadData(data, ref buffer);
                byte[] crcb = (byte[])buffer.Clone();
                SwapEndianness(ref buffer);
                uint CRC = BitConverter.ToUInt32(buffer, 0);

                uint[] crcTable = null;
                uint CRC2 = CRC32(nameb, 0, nameb.Length, 0, ref crcTable);
                CRC2 = CRC32(chunkdata, 0, chunkdata.Length, CRC2, ref crcTable);

                bool standard = false;
                foreach (string s in STANDARD_CHUNKS)
                    if (name == s) { standard = true; break; }

                bool critical = false;
                if (standard)
                    foreach (string s in CRITICAL_CHUNKS)
                        if (name == s) { critical = true; break; }

                Chunks.Add(new PNGChunk()
                {
                    Name = name,
                    Standard = standard,
                    Critical = critical,
                    Data = chunkdata,
                    CRC = CRC,
                    CRCBytes = crcb,
                    ValidCRC = (CRC == CRC2)
                });
            } while (name != "IEND");

            long EOF = data.Length - data.Position;
            if (EOF > 0)
            {
                buffer = new byte[EOF];
                ReadData(data, ref buffer);

                Chunks.Add(new PNGChunk()
                {
                    Name = "_EOF",
                    Standard = false,
                    Critical = false,
                    Data = buffer,
                    CRC = 0,
                    CRCBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 },
                    ValidCRC = false
                });
            }

            PNGChunk IHDR = Chunks[0];

            Width = BitConverter.ToUInt32(new byte[4] { IHDR.Data[3], IHDR.Data[2], IHDR.Data[1], IHDR.Data[0] }, 0);
            Height = BitConverter.ToUInt32(new byte[4] { IHDR.Data[7], IHDR.Data[6], IHDR.Data[5], IHDR.Data[4] }, 0);
        }

        public void WriteToFile(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                WriteToStream(fs);
        }
        public void WriteToStream(Stream s, bool standardonly = false, bool critonly = false)
        {
            s.Write(MAGIC_NUMBER, 0, MAGIC_NUMBER.Length);

            PNGChunk? eof = null;
            foreach (PNGChunk chunk in Chunks)
            {
                if ((critonly && !chunk.Critical) || (standardonly && !chunk.Standard)) continue;

                if (chunk.Name == "_EOF")
                {
                    eof = chunk;
                    continue;
                }

                byte[] length = BitConverter.GetBytes(chunk.Data.Length);
                SwapEndianness(ref length);
                WriteData(s, length);

                WriteData(s, Encoding.ASCII.GetBytes(chunk.Name));
                WriteData(s, chunk.Data);

                byte[] crc = BitConverter.GetBytes(chunk.CRC);
                SwapEndianness(ref crc);
                WriteData(s, crc);
            }

            if (eof.HasValue && eof != null)
                WriteData(s, eof.Value.Data);
        }

        public void RemoveNonCritical()
        {
            List<PNGChunk> toremove = new List<PNGChunk>();
            bool idatfound = false;
            foreach (PNGChunk chunk in Chunks)
            {
                if (!chunk.Critical)
                    toremove.Add(chunk);
                else if (chunk.Name == "IDAT")
                {
                    if (idatfound == true)
                        toremove.Add(chunk);
                    else
                        idatfound = true;
                }
            }
            foreach (PNGChunk chunk in toremove)
                Chunks.Remove(chunk);
        }

        static byte[] temp = new byte[4];
        static void SwapEndianness(ref byte[] data)
        {
            temp[0] = data[3];
            temp[1] = data[2];
            temp[2] = data[1];
            temp[3] = data[0];

            data[0] = temp[0];
            data[1] = temp[1];
            data[2] = temp[2];
            data[3] = temp[3];
        }

        static void ReadData(Stream data, ref byte[] buffer)
        {
            if (data.Read(buffer, 0, buffer.Length) < buffer.Length)
                throw new PNGMaskException("Unexpected EOF, PNG corrupt?");
        }
        static void WriteData(Stream data, byte[] buffer)
        {
            data.Write(buffer, 0, buffer.Length);
        }

        public static uint CRC32(byte[] stream, int offset, int length, uint crc, ref uint[] crcTable)
        {
            uint c;
            if (crcTable == null)
            {
                crcTable = new uint[256];
                for (uint n = 0; n <= 255; n++)
                {
                    c = n;
                    for (var k = 0; k <= 7; k++)
                    {
                        if ((c & 1) == 1)
                            c = 0xEDB88320 ^ ((c >> 1) & 0x7FFFFFFF);
                        else
                            c = ((c >> 1) & 0x7FFFFFFF);
                    }
                    crcTable[n] = c;
                }
            }
            c = crc ^ 0xffffffff;
            var endOffset = offset + length;
            for (var i = offset; i < endOffset; i++)
            {
                c = crcTable[(c ^ stream[i]) & 255] ^ ((c >> 8) & 0xFFFFFF);
            }
            return c ^ 0xffffffff;
        }
    }
}
