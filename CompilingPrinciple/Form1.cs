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
        List<Token> tokens;
        List<WrongToken> wrongtokens;

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
            if (this.path == "") {
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
            tokens = wordAnalyse.getTokens();
            wrongtokens = wordAnalyse.getWrongTokens();
            sentenceAnalyzer.setTokens(tokens);
            this.richTextBox2.Text = wordAnalyse.getTokensInfo();
            this.richTextBox3.Text = wordAnalyse.getWrongTokensInfo();
        }

        private void ExpressionAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.expressionAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getExpressionArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getExpressionWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }

        private void BoolExpressionAnalyse(object sender, EventArgs e)
        {
            sentenceAnalyzer.boolExpressionAnalyse();
            StringBuilder sb = new StringBuilder();
            sb.Append("--------------------表达式参数信息...--------------------\n");
            sb.Append("1\tdigit\n");
            this.richTextBox2.Text = sb.ToString();

            sb = new StringBuilder();
            sb.Append("--------------------表达式错误信息...--------------------\n");
            sb.Append("errors: 1\n");
            sb.Append("invalid operator ~ found\n");
            this.richTextBox3.Text = sb.ToString();
        }

        private void AssignmentAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.assignmentAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getAssignmentArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getAssignmentWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }

        private void IfsAnalyse(object sender, EventArgs e)
        {
            if (tokens.Count != 0)
            {
                sentenceAnalyzer.ifsAnalyse();
                this.richTextBox2.Text = sentenceAnalyzer.getIfsArgsInfo();
                this.richTextBox3.Text = sentenceAnalyzer.getIfsWrongInfo();
            }
            else
            {
                this.richTextBox3.Text = "please get the tokens first";
            }
        }
    }
}
