using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PNGMask
{
    public class LinkIndex : IDisposable
    {
        public List<LinkIndexRow> Rows = new List<LinkIndexRow>();
        public List<Image> Images = new List<Image>();

        public void Dispose()
        {
            foreach (Image img in Images)
                img.Dispose();
        }
    }
}
