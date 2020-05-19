using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompilingPrinciple
{
    public partial class Form1 : Form
    {
        private string path = "";
        WordAnalyzer wordAnalyse = new WordAnalyzer();
        SentenceAnalyzer sentenceAnalyzer = new SentenceAnalyzer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openHelp(object sender, EventArgs e)
        {
            string fileName = @"../../帮助.chm";
            Help.ShowHelp(this, fileName);
        }

        private void openFile(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "*.txt | *.txt";
            if (openfile.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                this.path = openfile.FileName;
                this.richTextBox1.Text = File.ReadAllText(this.path, Encoding.UTF8);
            }
        }

        private void saveFile(object sender, EventArgs e)
        {
            if(this.path == ""){
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "*.txt | *.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.path = sfd.FileName.ToString();
                    StreamWriter sw = new StreamWriter(this.path);
                    sw.Write(this.richTextBox1.Text);
                    sw.Dispose();
                }
            }
            else
            {
                StreamWriter sw = new StreamWriter(this.path);
                sw.Write(this.richTextBox1.Text);
                sw.Dispose();
            }
        }

        private void saveAs(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.txt | *.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.path = sfd.FileName.ToString();
                StreamWriter sw = new StreamWriter(this.path);
                sw.Write(this.richTextBox1.Text);
                sw.Dispose();
            }
        }

        private void WA(object sender, EventArgs e)
        {
            wordAnalyse.WordAnalyse(this.richTextBox1.Text);
            List<Token> tokens = wordAnalyse.getTokens();
            List<WrongToken> wrongtokens = wordAnalyse.getWrongTokens();
            StringBuilder sb = new StringBuilder();
            sb.Append("--------------------token表信息...--------------------\n");
            foreach (Token token in tokens){
                sb.Append(token.toString());
                sb.Append("\n");
            }
            this.richTextBox2.Text = sb.ToString();

            sb.Clear();
            sb.Append("--------------------词法分析错误信息...--------------------\n");
            sb.Append("词法分析结束 - " + wrongtokens.Count + " error(s)\n");
            foreach (WrongToken token in wrongtokens)
            {
                sb.Append(token.toString());
                sb.Append("\n");
            }
            this.richTextBox3.Text = sb.ToString();
        }

        private void ExpressionAnalyse(object sender, EventArgs e)
        {
            sentenceAnalyzer.ExpressionAnalyse(this.richTextBox1.Text);
            this.richTextBox2.Text = sentenceAnalyzer.getExpressionArgsInfo();
            this.richTextBox3.Text = sentenceAnalyzer.getExpressionWrongInfo();
        }
    }
}
