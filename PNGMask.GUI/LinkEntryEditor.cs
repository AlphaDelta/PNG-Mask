using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PNGMask.GUI
{
    public partial class LinkEntryEditor : Form
    {
        public bool Canceled = true;
        public string Key = "";
        public string Title = "";
        public string URL = "";

        LinkIndexBuilder parent;
        public LinkEntryEditor(LinkIndexBuilder parent)
        {
            this.parent = parent;

            InitializeComponent();

            img.Cursor = Cursors.Hand;
            lblEdit.Cursor = Cursors.Hand;

            lblEdit.Click += img_Click;

            KeyEventHandler handler = delegate (object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter && urlvalid && btnOk.Enabled) btnOk_Click(null, null); };
            txtTitle.KeyDown += handler;
            txtURL.KeyDown += handler;
        }
        public LinkEntryEditor(LinkIndexBuilder parent, string title, string url)
            : this(parent)
        {
            txtTitle.Text = title;
            txtURL.Text = url;

            btnOk.Enabled = true;
        }
        public LinkEntryEditor(LinkIndexBuilder parent, string key, string title, string url)
            : this(parent)
        {
            //img.Image = image;
            Key = key;
            img.Image = parent.imglist.Images[key];
            txtTitle.Text = title;
            txtURL.Text = url;
            lblEdit.Visible = false;

            btnOk.Enabled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Canceled = false;

            this.Title = txtTitle.Text;
            this.URL = txtURL.Text;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void img_Click(object sender, EventArgs e)
        {
            using (ImageListEditor editor = new ImageListEditor(parent.imglist))
            {
                editor.ShowDialog();

                if (editor.Canceled) return;
                if (editor.Key == "None")
                {
                    Key = "";
                    img.Image = null;
                    lblEdit.Visible = true;
                    return;
                }
                if (!parent.imglist.Images.ContainsKey(editor.Key)) return;
                
                Image eimg = parent.imglist.Images[editor.Key];

                img.Image = eimg;
                Key = editor.Key;

                lblEdit.Visible = false;
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            CheckValidity();
        }

        bool urlvalid = false;
        Regex regex = new Regex(@"^((https?|ftp)://([\w-]+[\.:@])*[\w-]+\.\w+(:\d+)?(/\S*)?|steam://\w+((/\S+)*/?)?|(skype|magnet):\S+)$", RegexOptions.Compiled);
        Color bad = Color.FromArgb(0xFF, 0xAA, 0xAA);
        private void txtURL_TextChanged(object sender, EventArgs e)
        {
            if (!regex.IsMatch(txtURL.Text))
            {
                urlvalid = false;
                txtURL.BackColor = bad;
                CheckValidity();
                return;
            }

            urlvalid = true;
            txtURL.BackColor = SystemColors.Window;

            CheckValidity();
        }

        void CheckValidity()
        {
            btnOk.Enabled = (urlvalid && txtTitle.Text.Length > 0);
        }
    }
}
