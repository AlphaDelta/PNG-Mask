using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PNGMask
{
    public partial class PasswordEntry : Form
    {
        bool CanPasswordBeEmpty;
        public bool Canceled = true;
        public string Password = "";
        public PasswordEntry(bool CanPasswordBeEmpty = true)
        {
            this.CanPasswordBeEmpty = CanPasswordBeEmpty;

            InitializeComponent();

            txtPassword.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter) btnOk_Click(null, null);
            };
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!CanPasswordBeEmpty && txtPassword.Text.Length < 1)
            {
                MessageBox.Show("Password cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Password = txtPassword.Text;
            Canceled = false;
            this.Close();
        }

        private void PasswordEntry_Load(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }
    }
}
