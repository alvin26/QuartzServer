using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyQuartzClient
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        //add job
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jobs");
            dlg.InitialDirectory = path;
            if (dlg.ShowDialog() == DialogResult.OK)
            {

            }
        }
    }
}
