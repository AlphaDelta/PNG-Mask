using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PNGMask.GUI
{
    public partial class SelectProvider : Form
    {
        public Provider SelectedProvider = null;
        Provider[] providers = null;
        public SelectProvider(Provider[] providers)
        {
            InitializeComponent();

            this.providers = providers;

            foreach (Provider p in providers)
                list.Items.Add(p.Name);

            list.DoubleClick += delegate { btnOk_Click(null, null); };
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int index = (list.SelectedIndices.Count < 1 ? -1 : list.SelectedIndices[0] - 1);

            if (index > -1)
                SelectedProvider = providers[index];

            this.Close();
        }
    }
}
