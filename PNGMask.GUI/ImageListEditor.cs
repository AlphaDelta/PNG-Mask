using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PNGMask.GUI
{
    public partial class ImageListEditor : Form
    {
        public bool Canceled = true;
        public string Key = "";
        ImageList imglist;
        public ImageListEditor(ImageList imglist)
        {
            this.imglist = imglist;

            InitializeComponent();

            list.SmallImageList = imglist;
            list.LargeImageList = imglist;

            list.Items.Add("None", null);

            foreach(string key in imglist.Images.Keys)
                list.Items.Add(key, key);

            list.DoubleClick += delegate { btnOk_Click(null, null); };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string file;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.png, *.jpg, *.jpeg, *.gif)|*.png;*.jpg;*.jpeg;*.gif";

                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                file = ofd.FileName;
            }

            string name = Path.GetFileNameWithoutExtension(file);
            string rname = name;
            int num = 0;
            while (imglist.Images.ContainsKey(rname))
            {
                num++;
                rname = String.Format("{0}_{1}", name, num);
            }

            imglist.Images.Add(rname, Image.FromFile(file));
            list.Items.Add(rname, rname);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (list.SelectedIndices.Count != 1) return;

            int index = list.SelectedIndices[0];
            if (index < 1) return;

            string key = list.Items[index].Text;
            list.Items.RemoveAt(index);
            imglist.Images.RemoveByKey(key);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (list.SelectedIndices.Count != 1) return;

            int index = list.SelectedIndices[0];
            //if (index < 1)
            //{
            //    Key = "none";
            //    Canceled = false;
            //    this.Close();
            //    return;
            //}

            Key = list.Items[index].Text;

            Canceled = false;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isindex = (list.SelectedIndices.Count > 0);
            btnOk.Enabled = isindex;
            btnRemove.Enabled = (isindex && list.SelectedIndices[0] > 0);
        }
    }
}
