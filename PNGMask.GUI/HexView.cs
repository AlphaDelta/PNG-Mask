using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Clearbytes
{
    internal class HexViewScroll : VScrollBar
    {
        public void PerformMouseWheel(MouseEventArgs e)
        {
            this.OnMouseWheel(e);
        }
    }

    /*
     * Prepare yourself for some spaghetti code.
     */
    public class HexView : Control
    {
        public const long HV_MAX_SIZE = 100 * 1024 * 1024; //100MB
        const int HPADDING = 8, VPADDING = 0, COLUMNS = 16;

        HexViewScroll scrollbar;
        public HexView()
        {
            this.SetStyle(ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);

            //I dare you to find an easier way to do this
            scrollbar = new HexViewScroll();
            scrollbar.Minimum = 0;
            scrollbar.Visible = true;
            scrollbar.Dock = DockStyle.Right;

            scrollbar.Scroll += OnScroll;

            this.Controls.Add(scrollbar);

            RecalcWrapper();
        }

        void OnScroll(object sender, ScrollEventArgs e)
        {
            int tempoffset = (e.NewValue + RowHeight / 2) / RowHeight;
            bool diff = (tempoffset != rowoffset);
            rowoffset = (tempoffset < 0 ? 0 : tempoffset);
            if (diff) this.Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            scrollbar.PerformMouseWheel(e);
        }

        int visiblerows = 0;
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            scrollbar.LargeChange = this.Height;

            RecalcScroll();

            RecalcVisible();
        }

        public Color
            TextColor = Color.FromArgb(0x11, 0x11, 0x11),
            WrapperColor = Color.FromArgb(0x00, 0x00, 0xBF);

        public SolidBrush
            Background = (SolidBrush)Brushes.White;

        public new Font Font { get { return _Font; } set { _Font = value; RecalcWrapper(); } }
        Font _Font = new Font("Courier New", 9.75f);
        //Font _Font = new Font("Lucida Console", 9.75f); //For sans-serif, VPADDING would need to be >0

        int HeaderOffset = 0, RowOffset = 0, RowHeight = 0;
        Point ptHeader, ptRows, ptHex, ptASCII;
        void RecalcWrapper()
        {
            Size fmeasure = TextRenderer.MeasureText("AAAAAAAA", _Font);
            Size rowmeasure = TextRenderer.MeasureText("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00", _Font);

            RowHeight = fmeasure.Height;

            HeaderOffset = HPADDING * 2 + fmeasure.Width + 1;
            RowOffset = VPADDING * 3 + fmeasure.Height + 4;

            ptHeader = new Point(HeaderOffset, VPADDING);
            ptRows = new Point(HPADDING, RowOffset);
            ptHex = new Point(HeaderOffset, RowOffset);
            ptASCII = new Point(HeaderOffset + rowmeasure.Width + HPADDING + 1, RowOffset);

            scrollbar.Maximum = this.Height + RowHeight;
            scrollbar.LargeChange = this.Height;
            scrollbar.SmallChange = RowHeight;
        }
        int rowoffset = 0;

        void RecalcVisible()
        {
            visiblerows = (int)Math.Ceiling((float)(this.Height - ptRows.Y) / (float)(RowHeight));
        }

        void RecalcScroll()
        {
            if (rows < 2)
            {
                scrollbar.Visible = false;
                scrollbar.Maximum = this.Height;
            }
            else
            {
                scrollbar.Visible = true;
                scrollbar.Maximum = this.Height + (rows - 1) * RowHeight;
            }
        }

        byte[] Bytes = null;
        int rows = 0;
        const int TEMPHEX_BUFFER = COLUMNS * 3;
        StringBuilder temphex = new StringBuilder(TEMPHEX_BUFFER);
        StringBuilder tempascii = new StringBuilder(COLUMNS);
        public static Encoding ANSI = Encoding.GetEncoding("Windows-1252");
        public Encoding Encoding = ANSI;
        Point ptoff;
        byte[] asciibytes = new byte[COLUMNS];
        int offset = 0;
        int target = 0;
        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(Background, e.ClipRectangle);

            TextRenderer.DrawText(e.Graphics,
                "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F",
                _Font, ptHeader, WrapperColor);

            if (rows < 1 || Bytes.Length < 1)
            {
                TextRenderer.DrawText(e.Graphics,
                    "00000000",
                    _Font, ptRows, WrapperColor);

                if (this.DesignMode)
                {

                    TextRenderer.DrawText(e.Graphics,
                        "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00",
                        _Font, ptHex, TextColor);

                    TextRenderer.DrawText(e.Graphics,
                        "................",
                        _Font, ptASCII, TextColor);
                }
                return;
            }

            offset = ptRows.Y;
            target = rows - rowoffset;
            if (target > visiblerows) target = visiblerows;
            target += rowoffset;
            for (int i = rowoffset, irow = 0; i < target; i++, irow++)
            {
                asciibytes = new byte[COLUMNS];
                temphex.Length = 0;
                temphex.Capacity = TEMPHEX_BUFFER;
                tempascii.Length = 0;
                tempascii.Capacity = TEMPHEX_BUFFER;

                ptoff = new Point(ptRows.X, offset);
                TextRenderer.DrawText(e.Graphics,
                    i.ToString("X7") + "0",
                    _Font, ptoff, WrapperColor);

                int byteoffset = rowoffset * COLUMNS + irow * COLUMNS;
                int indexoffset = 0;
                
                for (int j = 0; j < COLUMNS && (indexoffset = byteoffset + j) < Bytes.Length; j++)
                {
                    byte bt = Bytes[indexoffset];
                    //char asciibyte = '.';
                    if (Encoding != ANSI ||
                        bt > 0x1F &&
                        bt != 0x7F &&
                        bt != 0x81 &&
                        bt != 0x8D &&
                        bt != 0x8F &&
                        bt != 0x90 &&
                        bt != 0x98 &&
                        bt != 0x9D &&
                        bt != 0xAD) asciibytes[j] = bt;//asciibyte = (char)bt;
                    else asciibytes[j] = 0x2E;

                    //tempascii.Append(asciibyte);
                    temphex.Append((j == 0 ? bt.ToString("X2") : " " + bt.ToString("X2")));
                }
                ptoff = new Point(ptHeader.X, offset);
                TextRenderer.DrawText(e.Graphics,
                    temphex.ToString(),
                    _Font, ptoff, TextColor);
                ptoff = new Point(ptASCII.X, offset);

                TextRenderer.DrawText(e.Graphics,
                    Encoding.GetString(asciibytes),//tempascii.ToString(),
                    _Font, ptoff, TextColor, Background.Color, TextFormatFlags.Default | TextFormatFlags.NoPrefix);

                offset += RowHeight + VPADDING;
            }
        }

        public void ReadFile(string loc)
        {
            ReadFile(File.Open(loc, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }
        public void ReadFile(FileStream stream)
        {
            if (!stream.CanRead) throw new Exception("Cannot read from file stream");

            if (stream.Length > HV_MAX_SIZE)
                throw new Exception("File length exceeds HV_MAX_SIZE");

            byte[] bytes = new byte[stream.Length];
            if (stream.Read(bytes, 0, (int)stream.Length) < stream.Length)
                throw new IOException("Failed to read entire file");

            ReadBytes(bytes);

            stream.Close();
        }
        public void ReadBytes(byte[] bytes)
        {
            if (bytes.Length > HV_MAX_SIZE) throw new Exception("Byte array length exceeds HV_MAX_SIZE");

            Bytes = bytes;
            rows = (int)Math.Ceiling((float)bytes.Length / (float)0x10);

            RecalcScroll();

            scrollbar.Value = 0;
            OnScroll(null, new ScrollEventArgs(ScrollEventType.EndScroll, 0));
            this.Invalidate();
        }
    }
}
