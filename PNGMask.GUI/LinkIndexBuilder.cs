using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PNGMask.GUI
{
    public partial class LinkIndexBuilder : Form
    {
        public bool Canceled = true;
        //ImageList could cause memory leak?
        public ImageList imglist = new ImageList() { ImageSize = new Size(32, 32), ColorDepth = ColorDepth.Depth32Bit };
        public ListViewItem[] rows;

        ToolStripMenuItem mi2, mi3, mi4, mi5;
        
        public LinkIndexBuilder()
        {
            InitializeComponent();

            this.ResizeBegin += delegate { list.Scrollable = false; };
            this.Resize += delegate { columnHeader1.Width = list.Width - 4; };
            this.ResizeEnd += delegate { list.Scrollable = true; };

            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripMenuItem mi1 = new ToolStripMenuItem("Add link");
            mi1.Click += delegate
            {
                using (LinkEntryEditor editor = new LinkEntryEditor(this))
                {
                    editor.ShowDialog();

                    if (editor.Canceled) return;

                    ListViewItem lvi = new ListViewItem(editor.Title, editor.Key);
                    lvi.SubItems.Add(editor.URL);

                    if (list.SelectedIndices.Count != 1)
                        list.Items.Add(lvi);
                    else
                        list.Items.Insert(list.SelectedIndices[0], lvi);
                }
            };
            cms.Items.Add(mi1);

            cms.Items.Add(new ToolStripSeparator());

            mi2 = new ToolStripMenuItem("Edit link");
            mi2.Enabled = false;
            EventHandler editlink = delegate
            {
                if (list.SelectedIndices.Count != 1) return;
                ListViewItem lvi = list.SelectedItems[0];

                using (LinkEntryEditor editor = new LinkEntryEditor(this, lvi.ImageKey, lvi.SubItems[0].Text, lvi.SubItems[1].Text))
                {
                    editor.ShowDialog();

                    if (editor.Canceled) return;

                    lvi.ImageKey = editor.Key;
                    lvi.SubItems[0].Text = editor.Title;
                    lvi.SubItems[1].Text = editor.URL;
                }
            };
            mi2.Click += editlink;
            cms.Items.Add(mi2);

            mi3 = new ToolStripMenuItem("Move up");
            mi3.Enabled = false;
            mi3.Click += delegate
            {
                if (list.SelectedIndices.Count != 1) return;
                int index = list.SelectedIndices[0];
                if (index < 1) return;

                ListViewItem lvi = list.SelectedItems[0];
                list.Items.RemoveAt(index);
                list.Items.Insert(index - 1, lvi);

            };
            cms.Items.Add(mi3);

            mi4 = new ToolStripMenuItem("Move down");
            mi4.Enabled = false;
            mi4.Click += delegate
            {
                if (list.SelectedIndices.Count != 1) return;
                int index = list.SelectedIndices[0];
                if (index > list.Items.Count - 2) return;

                ListViewItem lvi = list.SelectedItems[0];
                list.Items.RemoveAt(index);
                list.Items.Insert(index + 1, lvi);
            };
            cms.Items.Add(mi4);

            mi5 = new ToolStripMenuItem("Delete link");
            mi5.Enabled = false;
            mi5.Click += delegate
            {
                if (list.SelectedIndices.Count != 1) return;

                list.Items.RemoveAt(list.SelectedIndices[0]);
            };
            cms.Items.Add(mi5);

            list.ContextMenuStrip = cms;
            list.LargeImageList = imglist;
            list.SmallImageList = imglist;

            //list.DoubleClick += delegate
            //{
            //    if (list.SelectedIndices.Count < 1) return;

            //    System.Diagnostics.Process.Start(list.SelectedItems[0].SubItems[1].Text);
            //};
            list.DoubleClick += editlink;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Canceled = false;
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (ListViewItem i in list.Items)
                items.Add(i);
            this.rows = items.ToArray();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LinkIndexBuilder_Load(object sender, EventArgs e)
        {
            columnHeader1.Width = list.Width - 4;
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            mi2.Enabled = mi3.Enabled = mi4.Enabled = mi5.Enabled = (list.SelectedIndices.Count > 0);
        }
    }
}
