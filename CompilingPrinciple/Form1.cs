using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilingPrinciple
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void openHelp(object sender, EventArgs e)
        {
            string fileName = @"E:\Workspace\VisualStudioProjects\CompilingPrinciple\CompilingPrinciple\帮助.chm";
            System.Console.WriteLine("!");
            Help.ShowHelp(this, fileName);
        }

        private void openFile(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "*.txt | *.txt";//设置文件后缀
                   
        }


    }
}
