using System;
using System.Collections.Generic;
using System.Text;

namespace PNGMask
{
    public struct LinkIndexRow
    {
        public int ImageIndex;
        public string Title, URL;

        public LinkIndexRow(string Title, string URL)
        {
            this.ImageIndex = -1;
            this.Title = Title;
            this.URL = URL;
        }
        public LinkIndexRow(int ImageIndex, string Title, string URL)
            : this(Title, URL)
        {
            this.ImageIndex = ImageIndex;
        }
    }
}
